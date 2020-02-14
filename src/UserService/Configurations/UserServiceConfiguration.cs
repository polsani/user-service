using Microsoft.Extensions.Configuration;

namespace UserService.Configurations
{
    public static class UserServiceConfiguration
    {
        public static string ConnectionString { get; private set; }

        public static void Configure(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}