using System;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SSO.Database;
using SSO.Models;
using SSO.Services.Abstract;
using SSO.Services.Realisation;

namespace SSO
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkNpgsql().AddDbContext<SSOContext>();

            services.AddCors();
            services.AddHttpClient();
            services.AddTransient<IGetToken, GetToken>();
            services.AddTransient<ITokenCheck, TokenCheck>();
            services.AddTransient<ITokenRefresh, TokenRefresh>();
            services.AddTransient<IGetUserInfo, GetUserInfo>();
            services.AddTransient<ISetPermissionsToRole, SetPermissionsToRole>();
            services.AddTransient<ISetUserRoles, SetUserRoles>();
            services.AddTransient<IPermissionCheck, PermissionCheck>();
            services.AddTransient<IPermissionAccessCheck, PermissionAccessCheck>();
            //setting authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                //if we need ssl, then change to true, otherwise we are using http
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration.GetValue<string>("issuer"), //ConfigurationExtensions.GetConfig().AuthConfig.Issuer,
                    ValidateAudience = true,
                    ValidAudience = Configuration.GetValue<string>("audience"),
                    ValidateLifetime = true,

                    IssuerSigningKey = ConfigurationExtensions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            }
            );

            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }



            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseHttpsRedirection();

            //handling 404error
            app.UseStatusCodePages(async context =>
            {
                if (context.HttpContext.Response.StatusCode == 404)/*additional conditions*/
                {
                    await context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorModel
                    {
                        Message = "404 Page not found."
                    }));
                }
            });
            app.UseRouting();

            app.UseCors(builder => builder.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
              );

            app.UseAuthorization();

            ///handling all other errors by using custom <see cref="ExceptionMiddlewareExtension"/> method
            app.ConfigureExceptionHandler();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default",
                                             pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
