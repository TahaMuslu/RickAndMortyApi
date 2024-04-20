using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicaton.Dto.Auth
{
    public class LoginDto
    {
        [DefaultValue("admin@admin.com")]
        public string Email { get; set; }
        [DefaultValue("P@ssw0rd")]
        public string Password { get; set; }
    }
}
