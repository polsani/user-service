using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using UserService.Domain.Mappers;
using UserService.Domain.Services;
using UserService.ViewModels;

namespace UserService.Services
{
    public class UserConsumerService : DefaultBasicConsumer
    {
        private readonly MessagingService _messagingService;
        private readonly IApplicationBuilder _application;

        public UserConsumerService(IApplicationBuilder app)
        {
            _messagingService = MessagingServiceSingleton.Instance;
            _application = app;
        }
        
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered,
            string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            if (properties.ContentType != "application/json")
                throw new ArgumentException($"Can't handle content type {properties.ContentType}");
            
            var message = Encoding.UTF8.GetString(body);

            try
            {
                var userRequest = JsonConvert.DeserializeObject<UserImportRequest>(message);

                using (var scope = _application.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var mapper = scope.ServiceProvider.GetService<IUserMapper>();
                    var user = mapper.ConvertToEntity(userRequest);
                    
                    var userService = scope.ServiceProvider.GetService<IUserService>();
                    userService.PersistUser(user, userRequest.Id);
                }
                
                _messagingService.SendAck(deliveryTag);
            }
            catch
            {
                _messagingService.SendNack(deliveryTag);
            }
            
            GC.Collect();
        }
    }
}