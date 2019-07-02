using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using SMIC.PhoneBooks.Persons;

namespace SMIC.PhoneBooks.Persons.DomainServices
{
    /// <summary>
    /// Person领域层的业务管理
    /// </summary>
    public class PersonManager : SMICDomainServiceBase, IPersonManager
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        private readonly IRepository<Person, int> _personRepository;
        /// <summary>
        /// Person的构造方法
        /// </summary>
        public PersonManager(IRepository<Person, int> personRepository)
        {
            _personRepository = personRepository;
        }

        //TODO:编写领域业务代码
        /// <summary>
        ///     初始化
        /// </summary>
        public void InitPerson()
        {
            throw new NotImplementedException();
        }

    }

}