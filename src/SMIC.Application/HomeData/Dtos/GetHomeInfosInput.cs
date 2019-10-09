
using Abp.Runtime.Validation;
using SMIC.Dtos;
using SMIC.HomeData;
using System;

namespace SMIC.HomeData.Dtos
{
    public class GetHomeInfosInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
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
        public DateTime? LastDate { get; set; }
    }
}
