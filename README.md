SignalR.RabbitMQ
================

RabbitMQ backplane for SignalR


Usage
----

Map SignalR in following way:

    app.MapSignalR(new HubConfiguration()
                  .UseRabbitMq(new RabbitMqScaleoutConfiguration
                  {
                      QueueName = "node_name",
                      UserName = "signalr_user",
                      Password = "signalr_password",
                      VirtualHost = "signalr"
                  }));
