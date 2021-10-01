using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServerJWT.Models
{
    public class AuthRequest
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }
}
