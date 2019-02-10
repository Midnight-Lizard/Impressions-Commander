using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleware> loger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> loger)
        {
            this.next = next;
            this.loger = loger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await this.next(httpContext);
            }
            catch (Exception ex)
            {
                loger.LogError(ex, httpContext.Request.Path);

                httpContext.Response.ContentType = MediaTypeNames.Text.Plain;
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                if (ex is ApplicationException)
                {
                    await httpContext.Response.WriteAsync(ex.Message);
                }
            }
        }
    }
}
