using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServerJWT.Services.Authentication
{
    // Key to check SIGNATURE (public)
    public interface IJwtSigningDecodingKey
    {
        Microsoft.IdentityModel.Tokens.SecurityKey GetKey();
    }
}
