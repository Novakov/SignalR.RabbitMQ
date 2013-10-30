using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Messaging;

namespace SignalR.RabbitMq
{
    public static class HubConfigurationExtensions
    {
        public static HubConfiguration UseRabbitMq(this HubConfiguration @this, RabbitMqScaleoutConfiguration scaleoutConfiguration)
        {
            var bus = new Lazy<RabbitMqMessageBus>(() => new RabbitMqMessageBus(@this.Resolver, scaleoutConfiguration));

            @this.Resolver.Register(typeof (IMessageBus), () => bus.Value);

            return @this;
        }
    }
}
