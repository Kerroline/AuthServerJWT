using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServerJWT.Models;
using AuthServerJWT.Services.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace AuthServerJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            this.configuration = configuration; // Needed to access the stored  JWT secret key
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult<string> Post(
            AuthRequest authRequest
         //   [FromServices] IJwtSigningEncodingKey signingEncodingKey)
        )
        {
            //const string signingSecurityKey = "MIIBCgKCAQEAsXoUruE3QybI3ygaARBUl0e663kxvylbSqlLBPf/lONUpWNucph5RQK/9WepNS/Z42Y79x+jf6MyPkgpMoLiiYB6Nzm5ssmZHg0ImzLmdyc3enCA0/TNrX8QBqieeLmm4Qja2pgsqtX7ae4En2Mr38qLrrpiMOXRqtxgVYqi+Lv9UxfVkRwHB4C+wc9FkM0IhmCja+AvpvtG7UBskPYLRB8o9gELVggKpV9t48yIEtJXG97BzmH3anEYZJY611NylZ1VUbnFbmRGJjAqF6Gy4bcGJbJrRlEQSGZT7mKKlxEijBR1VjADaQc8YVQXI76q1B3NiBcfvPSOJ+MSszd36QIDAQAB";

            var privateKey = "MIIEowIBAAKCAQEA1d+xELavibwwrTfsj+D1ZLKUjYnmhYpVgP1g4BmxU0pfJlJzdyCkhuPZAhIq+l9Mt539kA1KfocI2GRWgNYxjcXK6Jma0VDLSOdMznrmp2XR+w8FctF0lsL+0NIkosAsFAqMpJidt5iF0pxN40iGCYo10X0gFG1FFQn5p5AU/rGOeo3Y1P4licCVRH9Is9IyQHDLwJl9KxDsc0fsASYZeWOTpCzcWBDrjCQsVebIGgNV4OhinKuvv1Dqwt1R1JkRRz9F030/x1jAwZoteJRY/s9tXLZVCgMLrbTpCpSi3DspBjwXuMk6m2MrjOHr2lwwNsbKqXO3hIlA2fn+PZqMcQIDAQABAoIBAHrFx0o15LynaDX3dXf+hj/A99/sOoWMLJs4WIUsmoualNZV3GyVM30vjKJQtZU2Yb7CFg28Y3gQojY+Gx4oVxfwZBN0IdhojdmS1ZduG7waihiZveH0dP4af9Z124wFzwfOKoBOD7OV/bQx/9AqEK+nRE+2iVTQiSiTqlnTLwoo7GEGOt+1ie9cTSP3qwuVPt4dC2m7iovtrQJOjUfjZ1+qb0lATGZOjAlqAWOxRK7iW+PUVNZ+YVFMhkR7YuuxtR3lL89T/n2RWPtL1BK8Mwk52o6mcg8Kxg0s/euAA136uVfyCM5eLaf8Mi8UQLw2WGyI8rDEZdMezDN46jfq7AECgYEA6mzM2mU+xHYuLRX+f85ja2aEl7xP1DgJQLe3dtX61bD93tJ5bL+Ffn8VZUW1Q1962+B/oDtHK0ywanvU6L0iPs2rjMQTuRlS1AegO/jRWwYOAHYCKRh0Vvfj0BWa+QlHrzraoxDTHmLOL5+I2rOY63zSPHjWP2Hys3GFSGcypUECgYEA6Y6xM2scWri2u+C0xA5t2uU/DcMejNs2Q1r3tVdCRQxKCCpVyPp6d1+/jvldr28YhupoeA3EoN3NXhOzUlmKWJ5t5cN+TxTbZfmn9ulqpCrDnjRmY5UW4ZE1B+Mq82dF0T55FVU+UJpB/iFHS1sR7TR8NK7EXGjBCDC89v5nKzECgYAq0B4FkNIKzU6Xm1m0e7v6hGmY0KJ/rpA6CpNuF5xM5Jso06Wrb7rwpEMDEN0O5eQpPfHxEIJp7QKXH/B9ZPVZJPS8q56ygmZ3JMFl0oZhqlRqTyML4FR3AaTQfvGjo8c8wHHnsA3ukbr4RZmX348BCvXWZ3cxcjECBqyB0Z8VQQKBgQCMaHtBJShzpc49XpZtsxr52I2ykVXBalvR3FNEkoPFjODIzeKqo2KSd15q7qESwaAmI6/QVL6b0xwObZuFa00Pf5cj5QX/NtO7F36Roru4m/jkGC5huZR64NfXuQruL2y6oWsblxaSUFpSE+GPfN1nFAGDjnl/8H3zlZFUj5aoUQKBgE3XjGylO971CIip6OAFm3CgjqvRYD9OTaNk+r5DxyzrUFTx3Akkj+Zy7nP40KZ5pWBuuQ7OLpuEGLm82A5+NS1VO6zhPRfNKCMKoe4mdUtaRceVeb2saf+Be2g+FAXa65xui23sKtQJ3CqIisFuwg0+ZP3O2N/XKTh5OkIZ+kGk";
            var rsa = RSA.Create();
  
            
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);

           var signingCredentials = new SigningCredentials(
                key: new RsaSecurityKey(rsa),
                algorithm: SecurityAlgorithms.RsaSha256 // Important to use RSA version of the SHA algo 
            );
            DateTime jwtDate = DateTime.Now;

            Claim[] claims = new Claim[]
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType,"Tes"),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin"),
                    };

            var jwt = new JwtSecurityToken(
                audience: "jwt-test",
                issuer: "AuthServerJWT",
                claims: claims,
                notBefore: jwtDate,
                expires: jwtDate.AddSeconds(60),
                signingCredentials: signingCredentials
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new
            {
                jwt = token,
                unixTimeExpiresAt = new DateTimeOffset(jwtDate).ToUnixTimeMilliseconds(),
            });





            // 1. Проверяем данные пользователя из запроса.
            // ...

            // 2. Создаем утверждения для токена.


            //var claims = new Claim[]
            //{
            //    new Claim(ClaimTypes.NameIdentifier, authRequest.Login),
            //};

            //// 3. Генерируем JWT.
            //var token = new JwtSecurityToken(
            //    issuer: "AuthServerJWT",
            //    audience: "DemoAppClient",
            //    claims: claims,
            //    expires: DateTime.Now.AddMinutes(5),
            //    signingCredentials: new SigningCredentials(
            //            signingEncodingKey.GetKey(),
            //            signingEncodingKey.SigningAlgorithm)
            //);



            //string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            //return jwtToken;
        }

        //[Authorize]
        //[HttpGet]
        ////[Authorize(Roles = "Admin")]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    var UserName = this.HttpContext.User.Claims
        //        .FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType);

        //    var UserRole = this.HttpContext.User.Claims
        //        .FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultRoleClaimType);

        //    return new string[] { "UserName : " + UserName.Value, "Role : " + UserRole?.Value };
        //}

        [HttpGet]
        //[Authorize(AuthenticationSchemes = "Asymmetric")] // Use the "Asymmetric" authentication scheme
        [Authorize]
        public ActionResult<IEnumerable<string>> Get()
        {
            var UserName = this.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType);

            var UserRole = this.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimsIdentity.DefaultRoleClaimType);

            return new string[] { "UserName : " + UserName.Value, "Role : " + UserRole?.Value };
        }






        public IActionResult GenerateTokenAsymmetric()
        {
            using RSA rsa = RSA.Create();
            rsa.ImportRSAPrivateKey( // Convert the loaded key from base64 to bytes.
                source: Convert.FromBase64String(configuration["Jwt:Asymmetric:PrivateKey"]), // Use the private key to sign tokens
                bytesRead: out int _); // Discard the out variable 

            var signingCredentials = new SigningCredentials(
                key: new RsaSecurityKey(rsa),
                algorithm: SecurityAlgorithms.RsaSha256 // Important to use RSA version of the SHA algo 
            );

            DateTime jwtDate = DateTime.Now;

            var jwt = new JwtSecurityToken(
                audience: "jwt-test",
                issuer: "jwt-test",
                claims: new Claim[] { new Claim(ClaimTypes.NameIdentifier, "some-username") },
                notBefore: jwtDate,
                expires: jwtDate.AddSeconds(10),
                signingCredentials: signingCredentials
            );

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new
            {
                jwt = token,
                unixTimeExpiresAt = new DateTimeOffset(jwtDate).ToUnixTimeMilliseconds(),
            });
        }





    }
}
