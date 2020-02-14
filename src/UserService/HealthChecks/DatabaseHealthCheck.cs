using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using UserService.Configurations;

namespace UserService.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, 
            CancellationToken cancellationToken = new CancellationToken())
        {
            using (var connection = new SqlConnection(UserServiceConfiguration.ConnectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SELECT 1;";
                        command.ExecuteScalar();
                    }
                }
                catch
                {
                    return Task.FromResult(HealthCheckResult.Unhealthy());
                }
                return Task.FromResult(HealthCheckResult.Healthy());
            }  
        }
    }
}