using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SSO.Models;
using SSO.Models.Errors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace SSO
{
    /// <summary>
    /// Class for handling appearing exceptions.
    /// </summary>
    public static class ExceptionMiddlewareExtension
    {
        /// <summary>
        /// Method for handling appearing exceptions.
        /// </summary>
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = (int)GetErrorCode(contextFeature.Error);
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorModel
                        {
                            Success = (context.Response.StatusCode==200?true:false),
                            Message = contextFeature.Error.Message
                        })) ;
                    }
                });
            });
        }

        /// <summary>
        /// Method for identifying error status code.
        /// </summary>
        /// <param name="e">Occurred exception.</param>
        /// <returns>Identified exception.</returns>
        private static HttpStatusCode GetErrorCode(Exception e)
        {
            return e switch
            {
                ValidationException _ => HttpStatusCode.BadRequest,
                AuthenticationException _ => HttpStatusCode.Forbidden,
                NotImplementedException _ => HttpStatusCode.NotImplemented,
                ArgumentNullException _ => HttpStatusCode.BadRequest,
                SuccessCustomException _ => HttpStatusCode.OK,
                _ => HttpStatusCode.InternalServerError,
            };
        }

    }
}
