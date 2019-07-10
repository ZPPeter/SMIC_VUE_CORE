using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using SMIC.Authorization.Roles;
using SMIC.Authorization.Users;
using SMIC.MultiTenancy;
using Microsoft.Extensions.Logging;
using SMIC.Members;
using SMIC.MyTasks;
using SMIC.PhoneBooks.PhoneNumbers;
using SMIC.PhoneBooks.Persons;
using SMIC.EntityMapper.Tasks;
using SMIC.Utils; //EFLogger
namespace SMIC.EntityFrameworkCore
{
    public class SMICDbContext : AbpZeroDbContext<Tenant, Role, User, SMICDbContext>
    {

        /* Define a DbSet for each entity of the application */
        public virtual DbSet<MemberUser> MemberUsers { get; set; }
        public virtual DbSet<AbpUser> AbpUserExs { get; set; }
        public virtual DbSet<MyTask> Tasks { get; set; }

        public virtual DbSet<Person> Persons { get; set;}
        //public virtual DbSet<MyUser> MyUsers { get; set; }
        public virtual DbSet<PhoneNumber> PhoneNumbers { get; set; }

        public SMICDbContext(DbContextOptions<SMICDbContext> options)
            : base(options)
        {
            // this.Database.SetCommandTimeout(0);//设置SqlCommand永不超时
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {                        
            optionsBuilder.UseLoggerFactory(new EFLoggerFactory()); // OK - 输出来源在 ASP.NET Core Web 服务器
            //方案二：在 VS2017 【企业版】 IntelliTrace 窗口 查看 ADO.NET 事件

            /*
            // 下面也可以 
            //输出来源选择 ASP.NET Web 服务器
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new EFLoggerProvider());
            optionsBuilder.UseLoggerFactory(loggerFactory);

            base.OnConfiguring(optionsBuilder);
                        
            //if (!optionsBuilder.IsConfigured)  // 不能有此句
            //{
                // 输出来源在 ASP.NET Core Web 服务器
                optionsBuilder.UseLoggerFactory(new EFLoggerFactory());//将EFLoggerFactory类的实例注入给EF Core，这样所有DbContext的Log信息，都会由EFLogger类输出到Visual Studio的输出窗口了
                //optionsBuilder.UseSqlServer("Server=localhost;User Id=sa;Password=1qaz!QAZ;Database=TestDB");
            //}
            */
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<User>().Ignore(a => a.Name);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // MemberUserConfiguration 等

            modelBuilder.ApplyConfiguration(new TaskCfg());

            modelBuilder.Entity<MyTask>().Ignore(o => o.AssignedPerson);
            // [NotMapped] 
        }
    }
}
