using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServerJWT.Services.Authentication
{
    // Key to create SIGNATURE  (private)
    public interface IJwtSigningEncodingKey
    {
        string SigningAlgorithm { get; }

        Microsoft.IdentityModel.Tokens.SecurityKey GetKey();
    }
}
