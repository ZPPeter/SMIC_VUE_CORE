using System;
using Abp.Runtime.Validation;
using SMIC.Dto;

namespace SMIC.Members.Dto
{
    public class PagedMemberUserResultRequestDto : PagedAndSortedInputDto, IShouldNormalize  //PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public DateTimeOffset? From { get; set; }
        public DateTimeOffset? To { get; set; }

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
