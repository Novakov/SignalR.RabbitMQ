using System;
using System.Text;
using Microsoft.AspNet.SignalR.Messaging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace SignalR.RabbitMq
{
    internal class Consumer : DefaultBasicConsumer
    {
        private readonly Action<int, ulong, ScaleoutMessage> onReceived;

        public Consumer(Action<int, ulong, ScaleoutMessage> onReceived)
        {
            this.onReceived = onReceived;
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {
            var msg = JsonConvert.DeserializeObject<RmqMessage>(Encoding.UTF8.GetString(body));

            this.onReceived(msg.StreamIndex, msg.Id, ScaleoutMessage.FromBytes(msg.Body));
        }
    }
}