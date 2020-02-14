using Microsoft.Extensions.Configuration;

namespace UserService.Configurations
{
    public static class RabbitMqConfiguration
    {
        public static string AmqpUrl { get; private set; }

        public static void Configure(IConfiguration configuration)
        {
            AmqpUrl = configuration.GetValue<string>("RabbitMq:AmqpUrl");
        }
    }
}