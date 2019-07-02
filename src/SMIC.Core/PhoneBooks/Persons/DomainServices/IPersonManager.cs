
using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using SMIC.PhoneBooks.Persons;

namespace SMIC.PhoneBooks.Persons.DomainServices
{
    public interface IPersonManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        /// </summary>
        void InitPerson();

    }
}
