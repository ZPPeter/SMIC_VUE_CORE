using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace SMIC.Web.Host.Controllers.Dto
{
    public class UploadDto
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}
