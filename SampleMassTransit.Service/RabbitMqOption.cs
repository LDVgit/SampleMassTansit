namespace SampleMassTransit.Service
{
    public class RabbitMqOption
    {
        public string HostAddress { get; set; }
        public string VirtualHost { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public ushort Port { get; set; }
    }
}