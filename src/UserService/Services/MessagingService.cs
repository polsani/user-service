using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using RabbitMQ.Client;
using UserService.Configurations;
using UserService.Domain.Mappers;
using UserService.Domain.Services;

namespace UserService.Services
{
    public static class MessagingServiceSingleton
    {
        private static MessagingService _instance;
        private static readonly object Padlock = new object();

        public static MessagingService Instance
        {
            get
            {
                lock (Padlock)
                {
                    return _instance ?? (_instance = new MessagingService());
                }
            }
        }
    }
    
    public class MessagingService
    {
        private const string UserServiceExchange = "polsani.user.exchange";
        private const string UserServiceRegisteredQueue = "polsani.user.queue.registered";
        private const string UserServiceRoutingKey = "polsani.user.registered";
        
        private const string UserServiceDqlRegistered = "polsani.user.dql.registered";
        private const string UserServiceDlqExchange = "polsani.user.dql.exchange";
        private const string UserServiceDqlRoutingKey = "polsani.user.dql.registered";
        
        private readonly IModel _channel;

        public MessagingService()
        {
            var connectionFactory = new ConnectionFactory { Uri = new Uri(RabbitMqConfiguration.AmqpUrl) };
            var connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
        }

        public void SendUsersImports(IEnumerable<string> serializedUsersImports)
        {
            SetupRabbitStructure();

            var messageProperties = _channel.CreateBasicProperties();
            messageProperties.ContentType = "application/json";
            
            var batchPublish = _channel.CreateBasicPublishBatch();

            foreach (var serializedUserImport in serializedUsersImports)
            {
                batchPublish.Add(
                    exchange: UserServiceExchange,
                    routingKey: UserServiceRoutingKey,
                    mandatory: false,
                    properties: messageProperties,
                    body: Encoding.UTF8.GetBytes(serializedUserImport));
            }
            
            batchPublish.Publish();
        }
        
        public void ListenUserRegistered(IApplicationBuilder app)
        {
            SetupRabbitStructure();

            _channel.BasicQos(
                prefetchSize: 0,
                prefetchCount: 1,
                global: false);

            var consumer = new UserConsumerService(app);

            _channel.BasicConsume(
                queue: UserServiceRegisteredQueue,
                autoAck: false,
                consumer: consumer);
        }
        
        public void SendAck(ulong deliveryTag)
        {
            _channel.BasicAck(
                deliveryTag: deliveryTag,
                multiple: false);
        }

        public void SendNack(ulong deliveryTag)
        {
            _channel.BasicNack(
                deliveryTag: deliveryTag,
                multiple: false,
                requeue: false);
        }
        
        private void SetupRabbitStructure()
        {
            _channel.ExchangeDeclare(
                exchange: UserServiceExchange,
                durable: true,
                type: ExchangeType.Topic);

            _channel.ExchangeDeclare(
                exchange: UserServiceDlqExchange,
                durable: true,
                type: ExchangeType.Topic);
            
            _channel.QueueDeclare(
                queue: UserServiceRegisteredQueue,
                exclusive: false,
                durable: true,
                autoDelete: false,
                arguments: new Dictionary<string, object>
                {
                    {"x-dead-letter-exchange", UserServiceDlqExchange },
                    {"x-dead-letter-routing-key", UserServiceDqlRoutingKey }
                });

            _channel.QueueDeclare(
                queue: UserServiceDqlRegistered,
                exclusive: false,
                durable: true,
                autoDelete: false,
                arguments: null);
            
            _channel.QueueBind(
                queue: UserServiceRegisteredQueue,
                exchange: UserServiceExchange,
                routingKey: UserServiceRoutingKey);

            _channel.QueueBind(
                queue: UserServiceDqlRegistered,
                exchange: UserServiceDlqExchange,
                routingKey: UserServiceDqlRoutingKey);
        }
    }
}