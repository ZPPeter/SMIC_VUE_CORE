﻿using System.Collections.Generic;
using Abp.Application.Services.Dto;
using SMIC.PhoneBooks.PhoneNumbers.Dtos;

namespace SMIC.PhoneBooks.Persons.Dtos
{
    public class PersonListDto : FullAuditedEntityDto
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }


        public List<PhoneNumberListDto> PhoneNumbers { get; set; }

    }
}