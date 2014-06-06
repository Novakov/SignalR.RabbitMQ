SignalR.RabbitMQ
================

A RabbitMQ backplane for SignalR.


Usage
----

Map SignalR in the following way:

    app.MapSignalR(new HubConfiguration().UseRabbitMq(new RabbitMqScaleoutConfiguration()
        {
            RabbitMqUri = new Uri("amqp://guest:guest@localhost:5672"),
            QueueName = "node_name",
            ExchangeName = "RabbitMQSignalRScaleOut",
        }));
