using System;
using Abp.Domain.Entities;

namespace SMIC.SDIM
{
    public class SJMX : Entity<long>
    {
        //public int id { get; set; }         //ID - 继承自 Entity
        public string sjdid { get; set; }     
        public string qjmc { get; set; }      
        public DateTime djrq { get; set; }    
        public string xhggmc { get; set; }    
        public string ccbh { get; set; }      
        public string jdzt { get; set; }

    }
}
