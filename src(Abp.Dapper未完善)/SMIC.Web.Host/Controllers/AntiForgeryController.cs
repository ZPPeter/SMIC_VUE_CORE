using Microsoft.AspNetCore.Antiforgery;
using SMIC.Controllers;

namespace SMIC.Web.Host.Controllers
{
    public class AntiForgeryController : SMICControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
