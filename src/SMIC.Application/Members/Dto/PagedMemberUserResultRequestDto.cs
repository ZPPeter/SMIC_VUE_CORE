using System;
using Abp.Application.Services.Dto;

namespace Abplus.ZeroDemo.Members.Dto
{
    public class PagedMemberUserResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
        public DateTimeOffset? From { get; set; }
        public DateTimeOffset? To { get; set; }
    }
}
