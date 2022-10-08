using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Services.Interfeces
{
    //var claimsIdentity = (ClaimsIdentity)User.Identity;
    //var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
    public interface IClaimUserId
    {
        string GetUserId(ClaimsIdentity identity);
    }
}
