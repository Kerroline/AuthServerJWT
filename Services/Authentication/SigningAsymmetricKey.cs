using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServerJWT.Services.Authentication
{
    public class SigningAsymmetricKey : IJwtSigningEncodingKey, IJwtSigningDecodingKey
    {
        private readonly Microsoft.IdentityModel.Tokens.AsymmetricSecurityKey _secretKey;

        public string SigningAlgorithm { get; } = Microsoft.IdentityModel.Tokens.SecurityAlgorithms.RsaSha512;

        //public SigningAsymmetricKey(string key)
        //{
        //    this._secretKey = new Microsoft.IdentityModel.Tokens.AsymmetricSecurityKey(); 

        //    (System.Text.Encoding.UTF8.GetBytes(key));
        //}

        public Microsoft.IdentityModel.Tokens.SecurityKey GetKey() => this._secretKey;
    }
}
