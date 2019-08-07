using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing;

namespace Jiamiao.x.RabbitMq.Routing.Receive
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("========== Routing.Receive ==========");
            var workedId = Guid.NewGuid().ToString();

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare("direct_logs", ExchangeType.Direct);
            var queueName = channel.QueueDeclare().QueueName;

            if (args.Length < 1)
            {
                Console.WriteLine($"Usage:{Environment.GetCommandLineArgs()[0]} [info] [warning] [error]");
                Console.WriteLine("Press [enter] to exit.");
                Console.ReadLine();
                Environment.ExitCode = 1;
                return;
            }

            foreach (var logType in args)
            {
                channel.QueueBind(queue:queueName, exchange:"direct_logs", routingKey:logType);
                Console.WriteLine($"Bind routingKey -> {logType}");
            }

            Console.WriteLine("waiting for message...");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.WriteLine($"[{workedId}]  Receive Content:[{message}]  RoutingKey:[{routingKey}]");
            };
            channel.BasicConsume(queueName, true, consumer);

            Console.WriteLine("Press anyKey to exit");
            Console.ReadKey();

        }
    }
}
