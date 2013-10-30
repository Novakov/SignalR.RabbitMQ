using Microsoft.AspNet.SignalR;

namespace SignalR.RabbitMqBackplaceTest
{
    public class Test : Hub
    {
        public void Broadcast(string msg)
        {
            this.Clients.All.Message(msg);
        }
    }
}