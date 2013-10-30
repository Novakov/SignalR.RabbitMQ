using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;
using Owin;
using SignalR.RabbitMq;

namespace SignalR.RabbitMqBackplaceTest
{
    class Startup
    {
        private readonly string nodeName;

        public Startup(string nodeName)
        {
            this.nodeName = nodeName;
        }

        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR(new HubConfiguration()
                .UseRabbitMq(new RabbitMqScaleoutConfiguration
                {
                    QueueName = nodeName,
                    UserName = "signalr",
                    Password = "signalr",
                    VirtualHost = "signalr"
                })
                );
        }
    }
}