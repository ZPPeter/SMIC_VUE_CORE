
using DapperExtensions.Mapper;
namespace SMIC
{
    public class PersonMapper : ClassMapper<Person>
    {
        public PersonMapper()
        {
            Table("Persons");
            //Map(x => x.Roles).Ignore();
            AutoMap();
        }
    }
}
