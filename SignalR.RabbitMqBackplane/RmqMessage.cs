namespace SignalR.RabbitMq
{
    internal class RmqMessage
    {
        public int StreamIndex { get; set; }
        public ulong Id { get; set; }
        public byte[] Body { get; set; }
    }
}