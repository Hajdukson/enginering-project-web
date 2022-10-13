using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Models.DTOs
{
    public class UserPanelDTO
    {
        public IEnumerable<ItemDTO> Items { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalOutcome { get; set; }
        public UserDTO? AppUser { get; set; }
    }
}
