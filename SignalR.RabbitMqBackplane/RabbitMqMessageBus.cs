using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing.v0_9_1;
using IConnection = RabbitMQ.Client.IConnection;

namespace SignalR.RabbitMq
{
    public class RabbitMqMessageBus : ScaleoutMessageBus
    {
        private static long payloadId = 0;

        private readonly IModel model;
        private readonly string consumerTag;
        private readonly IConnection connection;
        private readonly RabbitMqScaleoutConfiguration configuration;

        public RabbitMqMessageBus(IDependencyResolver resolver, RabbitMqScaleoutConfiguration configuration)
            : base(resolver, configuration)
        {
            Open(0);
            
            var connectionFactory = CreateConnectionFactory(configuration);

            connection = connectionFactory.CreateConnection();

            model = connection.CreateModel();

            model.QueueDeclare(configuration.QueueName, false, false, true, new Hashtable());
            model.ExchangeDeclare(configuration.ExchangeName, ExchangeType.Fanout);
            model.QueueBind(configuration.QueueName, configuration.ExchangeName, "");

            consumerTag = model.BasicConsume(configuration.QueueName, true, new Consumer(this.OnReceived));

            this.configuration = configuration;
        }

        private static ConnectionFactory CreateConnectionFactory(RabbitMqScaleoutConfiguration configuration)
        {
            if (configuration.RabbitMqUri != null)
            {
                return new ConnectionFactory()
                {
                    Uri = configuration.RabbitMqUri.ToString(),
                };
            }
            else
            {
                return new ConnectionFactory
                {
                    HostName = configuration.HostName,
                    UserName = configuration.UserName,
                    Password = configuration.Password,
                    VirtualHost = configuration.VirtualHost,
                    Protocol = new Protocol(),
                    Port = configuration.Port,
                };
            }
        }

        protected override Task Send(int streamIndex, IList<Message> messages)
        {
            return Task.Run(() =>
            {
                var scaleoutMsg = new ScaleoutMessage(messages);

                var msg = new RmqMessage
                {
                    StreamIndex = streamIndex,
                    Id = 0,
                    Body = scaleoutMsg.ToBytes()
                };

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg));

                model.BasicPublish(this.configuration.ExchangeName, "", true, new BasicProperties(), body);
            });
        }

        protected override void OnReceived(int streamIndex, ulong id, ScaleoutMessage message)
        {
            base.OnReceived(streamIndex, (ulong)Interlocked.Increment(ref payloadId), message);
        }

        protected override void Dispose(bool disposing)
        {
            this.model.BasicCancel(this.consumerTag);

            this.connection.Close();

            base.Dispose(disposing);
        }
    }
}