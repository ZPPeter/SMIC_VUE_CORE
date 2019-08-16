

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SMIC.HomeData;

namespace SMIC.HomeData.Dtos
{
    public class CreateOrUpdateHomeInfoInput
    {
        [Required]
        public HomeInfoEditDto HomeInfo { get; set; }

    }
}