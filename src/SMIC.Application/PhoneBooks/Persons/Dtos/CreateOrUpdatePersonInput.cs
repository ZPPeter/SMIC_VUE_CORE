using System.ComponentModel.DataAnnotations;

namespace SMIC.PhoneBooks.Persons.Dtos
{
    public class CreateOrUpdatePersonInput
{
////BCC/ BEGIN CUSTOM CODE SECTION
////ECC/ END CUSTOM CODE SECTION
        [Required]
        public PersonEditDto Person { get; set; }

}
}