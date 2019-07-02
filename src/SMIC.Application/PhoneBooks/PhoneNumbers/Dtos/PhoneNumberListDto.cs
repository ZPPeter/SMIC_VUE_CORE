using System;
using Abp.Application.Services.Dto;
using SMIC.Enums.Extensions;

namespace SMIC.PhoneBooks.PhoneNumbers.Dtos
{
    public class PhoneNumberListDto : EntityDto<long>
    {
        ////BCC/ BEGIN CUSTOM CODE SECTION
        ////ECC/ END CUSTOM CODE SECTION
        public string Number { get; set; }
        public PhoneNumberType Type { get; set; }
        public int PersonId { get; set; }
        public DateTime CreationTime { get; set; }

        public string TypeDescription => Type.ToDescription();



    }
}