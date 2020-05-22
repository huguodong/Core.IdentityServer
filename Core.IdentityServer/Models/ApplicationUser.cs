using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.IdentityServer.Models
{
    public class ApplicationUser : IdentityUser<int>
    {

        public string LoginName { get; set; }

        public string RealName { get; set; }

        public int Sex { get; set; } = 0;

        public int Age { get; set; }

        public DateTime Birth { get; set; } = DateTime.Now;

        public string Addr { get; set; }

        public bool TdIsDelete { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
