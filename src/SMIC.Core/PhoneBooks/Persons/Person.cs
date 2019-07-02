using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using SMIC.PhoneBooks.PhoneNumbers;

namespace SMIC.PhoneBooks.Persons
{
    /// <summary>
    /// 人员
    /// </summary>

    public class Person : FullAuditedEntity
    {

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [EmailAddress]
        [MaxLength(64)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// 地址信息
        /// </summary>
        [MaxLength(120)]
        public string Address { get; set; }


        /// <summary>
        /// 电话号码的导航属性
        /// </summary>
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }

    }
}
