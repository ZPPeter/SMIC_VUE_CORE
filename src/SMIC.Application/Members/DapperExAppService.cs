using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using Abp.Dapper.Repositories;
using System.Linq;
using System.Data;
using SMIC.PhoneBooks.Persons;
using DapperExtensions;
using DapperExtensions.Mapper;
using System.Reflection;
using System.Data.SqlClient;
using Dapper;
using DapperExtensions.Sql;
using SMIC.Test;
using SMIC;

using Microsoft.Extensions.Configuration;
using SMIC.Configuration;
using SMIC.Web;
using System.Diagnostics;

namespace DapperEx
{
    public class DapperExAppService : SMIC.SMICAppServiceBase
    {
        static IConfigurationRoot _appConfiguration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
        string connectionString = _appConfiguration.GetConnectionString(SMICConsts.ConnectionStringName);

        protected IDbConnection conn;  // Dapper 用
        protected IDatabase Db;        // DapperExtensions 用

        private readonly IDapperRepository<Person, int> _personDapperRepository;

        public DapperExAppService(IDapperRepository<Person, int> personDapperRepository)
        {
            _personDapperRepository = personDapperRepository;
        }


        public dynamic GetTestAbpDapper()  // 需要设置 SMIC.EntityFrameworkCore\EntityMapper\PersonMapper.cs
        {
            // Person -> PhoneNumbers
            dynamic ret = _personDapperRepository.GetAllPaged(x => x.Address != "SD.JN", 0, 10, "ID").ToDynamicList<dynamic>(); //OK
            return ret;
        }

        /// <summary>
        /// DapperExtensions 没有实现一对多
        /// // 多表联查在这个库里没有提供，多表联查的话可以自己用Dapper的Query<T>(TFirst, TSecond, TResult,...)这个方法来实现
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TestPersonEntity> TestDapperExtensions()
        {
            // Map 文件 TestPersonEntityMapper
            SqlConnection connection = new SqlConnection(connectionString);
            var config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(), new SqlServerDialect());
            // new List<Assembly>()  --> Count =0
            var sqlGenerator = new SqlGeneratorImpl(config);
            Db = new Database(connection, sqlGenerator);

            // 拼接条件 查询
            IList<IPredicate> predList = new List<IPredicate>();
            predList.Add(Predicates.Field<TestEntity>(p => p.Name, Operator.Like, "A%"));
            predList.Add(Predicates.Field<TestEntity>(p => p.Id, Operator.Eq, 2));
            IPredicateGroup predGroup = Predicates.Group(GroupOperator.And, predList.ToArray());
            var ret1 = Db.GetList<TestEntity>(predGroup);

            var ret2 = Db.GetList<TestPersonEntity>();

            connection.Close();

            return ret2;

            //TestEntity p2 = Db.Get<TestEntity>(1);
            //return p2.Address;
        }

        /// <summary>
        /// Dapper 一对多
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TestPersonEntity> TestDapper()
        {
            /*
                一对多的关系用数组
                多对多加一层 mapping
             */
            // 实现 1--n 的查询操作
            // int id = 1;
            // string query = "SELECT * FROM Persons p LEFT JOIN PhoneNumbers pn ON pn.PersonId = p.Id  where p.id = @id";
            conn = new SqlConnection(connectionString);
            string query = "SELECT * FROM Persons p LEFT JOIN PhoneNumbers pn ON p.Id = pn.PersonId";
            TestPersonEntity lookup = null;
            var b = conn.Query<TestPersonEntity, PhoneNumbers, TestPersonEntity>(query,
                       (p, pn) =>
                       {
                           //扫描第一条记录，判断非空和非重复
                           if (lookup == null || lookup.Id != p.Id)
                               lookup = p;
                           //对应的PN非空，加入当前P的PN.List中，最后把重复的P去掉。
                           if (pn != null)
                               lookup.PhoneNumbers.Add(pn);
                           return lookup;
                       }).Distinct();  //.SingleOrDefault(); -> 一条记录
                                       // 没有 .Distinct() 会有重复记录
                                       //}, new { id = id }).Distinct().SingleOrDefault();

            // 方法二
            var orderDictionary = new Dictionary<int, TestPersonEntity>();
            b = conn.Query<TestPersonEntity, PhoneNumbers, TestPersonEntity>(query,
            map: (order, orderDetail) =>  // map: 可忽略                        
            {
                TestPersonEntity orderEntry;
                if (!orderDictionary.TryGetValue(order.Id, out orderEntry))
                {
                    orderEntry = order;
                    orderEntry.PhoneNumbers = new List<PhoneNumbers>();
                    orderDictionary.Add(orderEntry.Id, orderEntry);
                }
                orderEntry.PhoneNumbers.Add(orderDetail);
                return orderEntry;
            },
                splitOn: "PersonId")  // 参数 orderDetail 的主键
                .Distinct()
                .ToList();

            conn.Close();

            using (conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var brand = conn.Query<TestPersonEntity>("SELECT * FROM Persons").ToList();
                // 用Console.WriteLine("xxxxx")，在asp.net Web程序，在输出窗口是不会输出结果的，应该用Debug.WriteLine("xxxxx");
                foreach (var item in brand)
                {
                    //Console.Write(item.Id + " : " + item.Name); // Console类表示 控制台 应用程序的标准输入流、输出流和错误流
                    Debug.WriteLine(item.Id + " : " + item.Name);
                }
            }

            return b;
        }

    }
}