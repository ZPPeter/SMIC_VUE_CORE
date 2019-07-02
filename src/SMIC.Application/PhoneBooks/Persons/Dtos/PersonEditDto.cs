using System.ComponentModel.DataAnnotations;

namespace SMIC.PhoneBooks.Persons.Dtos
{
    public class PersonEditDto
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public int? Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [MaxLength(SMICConsts.MaxNameLength)]
        public string Name { get; set; }
        [MaxLength(SMICConsts.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }


        /// <summary>
        /// 地址信息
        /// </summary>
        [MaxLength(SMICConsts.MaxAddressLength)]
        public string Address { get; set; }
    }
}