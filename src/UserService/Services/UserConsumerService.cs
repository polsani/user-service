using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using UserService.Domain.Mappers;
using UserService.ViewModels;

namespace UserService.Services
{
    public class UserConsumerService : DefaultBasicConsumer
    {
        private readonly MessagingService _messagingService;
        private readonly IUserMapper _mapper;

        public UserConsumerService(IUserMapper mapper)
        {
            _messagingService = MessagingServiceSingleton.Instance;
            _mapper = mapper;
        }
        
        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered,
            string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            if (properties.ContentType != "application/json")
                throw new ArgumentException($"Can't handle content type {properties.ContentType}");
            
            var message = Encoding.UTF8.GetString(body);

            try
            {
                var userRequest = JsonConvert.DeserializeObject<User>(message);
                var user = _mapper.ConvertToEntity(userRequest);
                
                //_notifierService.Notify(notification);
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