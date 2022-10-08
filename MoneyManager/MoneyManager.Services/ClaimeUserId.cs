using MoneyManager.Services.Interfeces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Services
{
    public class ClaimeUserId : IClaimUserId
    {
        public string GetUserId(ClaimsIdentity identity) => identity.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
