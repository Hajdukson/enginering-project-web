using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MoneyManager.WWW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManageController : Controller
    {
        public UserManager<IdentityUser> UserManager { get; private set; }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }
        public UserManageController(UserManager<IdentityUser> userManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register()
        {
            return View();  
        }
    }
}
