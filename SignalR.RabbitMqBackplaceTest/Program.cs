using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.Owin.Hosting;

namespace SignalR.RabbitMqBackplaceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var nodeName = args[0];            

            Console.Title = nodeName;

            using (var node = new Node(nodeName).Start().Result)
            {
                node.Message += msg => Console.WriteLine("{1} got: {0}", msg, nodeName);

                while (true)
                {
                    var c = Console.ReadKey();

                    if (c.KeyChar == 'q')
                    {
                        break;
                    }

                    Console.WriteLine("Broadcast by {0}", nodeName);
                    node.Broadcast("Msg by " + nodeName);
                }
            }
        }

        private static void RunConti(string nodeName, Node node)
        {
            var cancel = new CancellationTokenSource();

            Task.Run(() =>
            {
                while (true)
                {
                    cancel.Token.ThrowIfCancellationRequested();

                    Console.WriteLine("Broadcasting via {0}", nodeName);
                    node.Broadcast("Msg by " + nodeName);

                    Task.Delay(TimeSpan.FromSeconds(1), cancel.Token).Wait(cancel.Token);
                }
            }, cancel.Token);

            Console.ReadLine();
            cancel.Cancel();
        }
    }

    class Node : IDisposable
    {
        private readonly string nodeName;
        private readonly string url;
        private IDisposable webApp = null;
        private HubConnection connection;
        private IHubProxy hub;

        public event Action<string> Message;

        public Node(string nodeName)
        {
            this.nodeName = nodeName;
            this.url = string.Format("http://localhost:9999/{0}", nodeName);
        }

        public async Task<Node> Start()
        {
            this.webApp = WebApp.Start(url, app => new Startup(nodeName).Configuration(app));

            Thread.Sleep(TimeSpan.FromSeconds(5));

            connection = new HubConnection(url);
            hub = connection.CreateHubProxy("Test");

            hub.On<string>("Message", msg =>
            {
                if (this.Message != null)
                {
                    this.Message(msg);
                }
            });
           
            await connection.Start();

            return this;
        }

        public void Broadcast(string message)
        {
            hub.Invoke("Broadcast", message);
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Dispose();
                connection = null;
            }

            if (webApp != null)
            {
                webApp.Dispose();
                webApp = null;
            }
        }
    }
}
