using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Jiamiao.x.RabbitMq.Routing.Send
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== Routing.Send ==========");

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare("direct_logs", ExchangeType.Direct);
            string[] logTypes = { "error", "info", "monitor", "warning" };
            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                var logType = logTypes[random.Next(0, logTypes.Length)];
                var message = $"{logType} : {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("direct_logs", logType, null, body);
                Console.WriteLine($"Send Content:{message} to {logType}");

                Thread.Sleep(1000);
            }

            Console.WriteLine("Press anyKey to exit");
            Console.ReadKey();
        }
    }
}
