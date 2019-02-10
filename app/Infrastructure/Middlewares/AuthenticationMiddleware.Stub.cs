using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MidnightLizard.Impressions.Commander.Infrastructure.Middlewares
{
    public class AuthenticationMiddlewareStub
    {
        private readonly RequestDelegate next;

        public AuthenticationMiddlewareStub(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.User = new ClaimsPrincipal(
                new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, "test-user-name"),
                    new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
                    new Claim("sub", "test-user-id")
                }, "TestCookieAuthentication"));

            await this.next(httpContext);
        }
    }
}
