using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServerJWT.Services.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace AuthServerJWT
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            

            var publicKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA1d+xELavibwwrTfsj+D1ZLKUjYnmhYpVgP1g4BmxU0pfJlJzdyCkhuPZAhIq+l9Mt539kA1KfocI2GRWgNYxjcXK6Jma0VDLSOdMznrmp2XR+w8FctF0lsL+0NIkosAsFAqMpJidt5iF0pxN40iGCYo10X0gFG1FFQn5p5AU/rGOeo3Y1P4licCVRH9Is9IyQHDLwJl9KxDsc0fsASYZeWOTpCzcWBDrjCQsVebIGgNV4OhinKuvv1Dqwt1R1JkRRz9F030/x1jAwZoteJRY/s9tXLZVCgMLrbTpCpSi3DspBjwXuMk6m2MrjOHr2lwwNsbKqXO3hIlA2fn+PZqMcQIDAQAB";
            //SigningAsymmetricKey signingKey = new SigningAsymmetricKey(signingSecurityKey);
            //services.AddSingleton<IJwtSigningEncodingKey>(signingKey);
            /*
             * Configure validation of JWT signed with a private asymmetric key.
             * 
             * We'll use a public key to validate if the token was signed
             * with the corresponding private key.
             */
            services.AddSingleton<RsaSecurityKey>(provider => {
                // It's required to register the RSA key with depedency injection.
                // If you don't do this, the RSA instance will be prematurely disposed.

                RSA rsa = RSA.Create();
                rsa.ImportSubjectPublicKeyInfo(
                    source: Convert.FromBase64String(publicKey),
                    bytesRead: out _
                );

                return new RsaSecurityKey(rsa);
            });
            //opt => {
            //    opt.DefaultAuthenticateScheme = jwtSchemeName;
            //    opt.DefaultChallengeScheme = jwtSchemeName;
            //}
            //const string jwtSchemeName = "JwtBearer";
            const string jwtSchemeName = "Asymmetric";
            _ = services.AddAuthentication(opt => { opt.DefaultAuthenticateScheme = jwtSchemeName; })
                .AddJwtBearer(jwtSchemeName, options =>
                {
                    SecurityKey rsa = services.BuildServiceProvider().GetRequiredService<RsaSecurityKey>();

                    options.RequireHttpsMetadata = true;
                    options.IncludeErrorDetails = true; // <- great for debugging

                    // Configure the actual Bearer validation
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = rsa,
                        ValidAudience = "jwt-test",
                        ValidIssuer = "AuthServerJWT",
                        RequireSignedTokens = true,
                        RequireExpirationTime = true, // <- JWTs are required to have "exp" property set
                        ValidateLifetime = true, // <- the "exp" will be validated
                        ValidateAudience = true,
                        ValidateIssuer = true,

                        ClockSkew = TimeSpan.FromSeconds(5)
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 
            app.UseAuthentication();

            app.UseRouting();

            //allows the use of[Authorize] on controllers and actions
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            var serviceProvider = app.ApplicationServices.GetService<IServiceProvider>();

        }
    }
}
