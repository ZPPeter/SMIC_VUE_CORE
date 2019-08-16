using System;
using Abp.Domain.Entities;

namespace SMIC.SDIM
{
    public class WTD : Entity<long>
    {
        //public int id { get; set; }         //送检单ID - 继承自 Entity
        public string sjdid { get; set; }     //委托单号
        public string dwmc { get; set; }      //送检单位
        public DateTime sjrq { get; set; }    //送检日期
        public int yqjs { get; set; }         //送检单ID

    }
}
