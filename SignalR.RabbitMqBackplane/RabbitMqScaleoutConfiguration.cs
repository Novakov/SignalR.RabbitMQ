using Microsoft.AspNet.SignalR.Messaging;
using System;

namespace SignalR.RabbitMq
{
    public class RabbitMqScaleoutConfiguration : ScaleoutConfiguration
    {
        public RabbitMqScaleoutConfiguration()
            : base()
        {
            this.HostName = "localhost";
            this.ExchangeName = "messages";
            this.Port = 5672;
            this.VirtualHost = "/";
        }

        /// <summary>
        /// When set, a URI takes precedence over HostName, Port, Username and Password.
        /// </summary>
        public Uri RabbitMqUri { get; set; }

        /// <summary>
        /// The hostname name of the RabbitMQ server to connect to.  e.g. myrabbitserver.domain.com  
        /// Defaults to "localhost".
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// The TCP port to connect to RabbitMQ on.
        /// Defaults to 5672.
        /// </summary>
        public int Port { get; set; }

        public string QueueName { get; set; }

        /// <summary>
        /// The RabbmitMQ virtual host to use.
        /// Defaults to "/".
        /// </summary>
        public string VirtualHost { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// The Exchange name to publish SignalR messages to.
        /// Defaults to "messages".
        /// </summary>
        public string ExchangeName { get; set; }
    }
}