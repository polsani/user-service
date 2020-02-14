using Microsoft.AspNetCore.Builder;
using UserService.Services;

namespace UserService.Extensions
{
    public static class UserConsumerExtensions
    {
        public static void UseUserConsumer (this IApplicationBuilder app, MessagingService messagingService)
        {
            messagingService.ListenUserRegistered(app);
        }
    }
}