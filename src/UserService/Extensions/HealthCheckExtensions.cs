using System;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace UserService.Extensions
{
    public static class HealthCheckExtensions
    {
        public static void UseHealthChecks(this IApplicationBuilder app)
        {
            var options = new HealthCheckOptions();
            options.ResultStatusCodes[HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable;

            options.ResponseWriter = async (ctx, rpt) =>
            {
                var result = JsonConvert.SerializeObject(new
                {
                    status = rpt.Status.ToString(),
                    dependencies = rpt.Entries.Select(e => new
                    {
                        dependency = e.Key, 
                        healthStatus = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                    })
                }, Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                ctx.Response.ContentType = MediaTypeNames.Application.Json;
                await ctx.Response.WriteAsync(result);
            };

            app.UseHealthChecks("/health", options);
        }
    }
}