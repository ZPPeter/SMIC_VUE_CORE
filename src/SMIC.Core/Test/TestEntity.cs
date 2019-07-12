using DapperExtensions.Mapper;
namespace SMIC.Test
{
    public class TestEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
    }

    public class TestEntityMapper : ClassMapper<TestEntity>
    {
        public TestEntityMapper()
        {
            Table("Persons");
            Map(p => p.Id).Key(KeyType.Assigned);
            AutoMap();
        }
    }
}
