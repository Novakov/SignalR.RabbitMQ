using Microsoft.AspNet.SignalR.Messaging;

namespace SignalR.RabbitMq
{
    public class RabbitMqScaleoutConfiguration : ScaleoutConfiguration
    {
        public string QueueName { get; set; }

        public string VirtualHost { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string ExchangeName { get; set; }        
    }
}