
using DapperExtensions.Mapper;
using SMIC.PhoneBooks.Persons;
namespace SMIC.EntityFrameworkCore.EntityMapper
{
    public class PersonMapper : ClassMapper<Person>
    {
        public PersonMapper()
        {
            Table("Persons");
            //Map(x => x.Id).Key(KeyType.Identity); //设置主键， ABP 已经默认设置
            //Map(x => x.Roles).Ignore();
            AutoMap();

            /*
            public class MultikeyMapper : ClassMapper<Multikey>
            {
                public MultikeyMapper()
                {
                    Map(p => p.Key1).Key(KeyType.Identity);
                    Map(p => p.Key2).Key(KeyType.Assigned);
                    //Map(p => p.Date).Ignore();
                    AutoMap();
                }
            }          

                */



        }
    }
}
