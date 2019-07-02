using AutoMapper;
using SMIC.PhoneBooks.PhoneNumbers;
using SMIC.PhoneBooks.PhoneNumbers.Dtos;

namespace SMIC.PhoneBooks.Persons.Dtos.LTMAutoMapper
{
    /// <summary>
    /// 配置Person的AutoMapper
    /// </summary>
    internal static class CustomerPersonMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            //    configuration.CreateMap <Person, PersonDto>();
            configuration.CreateMap<Person, PersonListDto>();
            configuration.CreateMap<PersonEditDto, Person>();
            // configuration.CreateMap<CreatePersonInput, Person>();
            //        configuration.CreateMap<Person, GetPersonForEditOutput>();


            configuration.CreateMap<PhoneNumber, PhoneNumberListDto>();
            configuration.CreateMap<PhoneNumberEditDto, PhoneNumber>();
        }
    }
}