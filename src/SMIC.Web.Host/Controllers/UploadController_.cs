using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SMIC.Controllers;

namespace SMIC.Web.Host.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : Controller
    {   public async Task<ActionResult> TestNotification([FromBody]IFormFile file)
        //public void UploadFile ([FromBody]IFormFile file)
        {            
            var stream = file.OpenReadStream();
            var filename = "logo_1" +  ".png";
            var path = $"{filename.Substring(0, 2)}/{filename.Substring(2, 2)}";
            var dir = $"{Environment.CurrentDirectory}\\wwwroot\\upload\\{path}";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            using (var fileStream = new FileStream($"{dir}\\{filename}", FileMode.Create))
            {                
                await file.CopyToAsync(fileStream);
            }            
            
            using (var image = Image.Load<Rgba32>($"{dir}\\{filename}"))
            {
                image.Mutate(x => x
                    .Resize(160, 160));
                image.Save($"{dir}\\thumbnail_{filename}");
            }            
            
            return Content("Sent notification");
        }

    }
}
