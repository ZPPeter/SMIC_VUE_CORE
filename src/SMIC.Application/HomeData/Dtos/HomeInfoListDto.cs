

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using SMIC.HomeData;

namespace SMIC.HomeData.Dtos
{
    public class HomeInfoListDto : EntityDto<int> 
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime? CreationTime { get; set; }
    }


}