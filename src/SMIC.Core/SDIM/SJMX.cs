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
        public string xhggmc { get; set; }     //型号规格 - xhgg
        public string xhggbm { get; set; }     //型号规格编码
        public string ccbh { get; set; }       //出厂编号   
        public string jdzt { get; set; }       // 仪器检定状态 - int
        public string zzc { get; set; }        //制造厂
        public string img { get; set; }        // 0leica 1topcon 2trimble 3sokkia 4south 5nikkom
        public DateTime yqjcrq { get; set; }   //要求检出日期 
        public string wtdw { get; set; }      //委托单位
        public DateTime jdrq { get; set; }    //jdrq - dbo.DPII_JDRQ
        public DateTime jwrq { get; set; }    //jwrq - dbo.DPII_JDRQ

        //public jbcs jbcs { get; set; }
        public string jdy { get; set; }       //检定员
        public string bzsm { get; set; }      //备注说明        
    }
}
