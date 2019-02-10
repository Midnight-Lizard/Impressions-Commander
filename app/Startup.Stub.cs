using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MidnightLizard.Impressions.Commander.Infrastructure.Middlewares;

namespace MidnightLizard.Impressions.Commander
{
    public class StartupStub : Startup
    {
        public StartupStub(IConfiguration configuration) : base(configuration)
        {
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<AuthenticationMiddlewareStub>();
            base.Configure(app, env);
        }
    }
}
