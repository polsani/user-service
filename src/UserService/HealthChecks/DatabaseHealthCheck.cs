using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UserService.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
            CancellationToken cancellationToken = new CancellationToken())
        {
            var healthCheckResultHealthy = true;

            return Task.FromResult(healthCheckResultHealthy ? 
                HealthCheckResult.Healthy() : 
                HealthCheckResult.Unhealthy());
        }
    }
}