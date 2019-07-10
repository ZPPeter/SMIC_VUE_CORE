﻿using Abp.Application.Services.Dto;
using SMIC.PhoneBooks.Persons;

namespace SMIC.Dto
{
    public class PagedAndSortedInputDto : PagedInputDto, ISortedResultRequest
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public string Sorting { get; set; }
        public string Order { get; set; }

        public PagedAndSortedInputDto()
        {
            MaxResultCount = AppConsts.DefaultPageSize;            
        }
    }
}