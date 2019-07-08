using Abp.Runtime.Validation;
using SMIC.Dto;
using System;

namespace SMIC.PhoneBooks.Persons.Dtos
{
    public class GetPersonsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        /// <summary>
        /// 模糊搜索使用的关键字
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }

    }

    public class GetVwSjmxsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        /// <summary>
        /// 模糊搜索使用的关键字
        /// </summary>
        public string QJMC { get; set; }
        public string WTDH { get; set; }
        public string WTDW { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public string Order { get; set; }
        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
            if (string.IsNullOrEmpty(Order))
            {
                Order = "asc";
            }
        }

    }

}
