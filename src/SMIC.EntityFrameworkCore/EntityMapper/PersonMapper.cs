
using DapperExtensions.Mapper;
using SMIC.PhoneBooks.Persons;
namespace SMIC.EntityFrameworkCore.EntityMapper
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
