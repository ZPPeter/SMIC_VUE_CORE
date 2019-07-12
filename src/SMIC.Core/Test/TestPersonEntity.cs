using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
namespace SMIC.Test
{
    public class TestPersonEntity
    {   public TestPersonEntity()
        {
            PhoneNumbers = new List<PhoneNumbers>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string Address { get; set; }

        public ICollection<PhoneNumbers> PhoneNumbers { get; set; }
        // public virtual List<PhoneNumbers> PhoneNumbers { get; set; } // OK

    }
        
    public class PhoneNumbers //表名字一致
    {
        public string Number { get; set; }

        public int PersonId { get; set; }
        
        public TestPersonEntity Person { get; set; }
        
        public DateTime CreationTime { get; set; }
    }

    public class TestPersonEntityMapper : ClassMapper<TestPersonEntity>
    {
        public TestPersonEntityMapper()
        {
            Table("Persons");            
            Map(m => m.PhoneNumbers).Ignore();
            AutoMap();
        }
    }

    // var ret1 = Db.GetList<TestEntity>(predGroup);
    // var ret2 = Db.GetList<TestPersonEntity>();
    // PhoneNumber 没用到
    public class PhoneNumberMapper : ClassMapper<PhoneNumbers>
    {
        public PhoneNumberMapper()
        {
            Table("PhoneNumberssssssss");
            //Map(m => m.TestPersonEntity).Ignore();
            AutoMap();
        }
    }
}
