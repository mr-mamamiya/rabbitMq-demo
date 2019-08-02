using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Jiamiao.x.RabbitMq.Topic.Receive
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== Topic.Send ==========");

            var workerId = Guid.NewGuid().ToString();

            var factory = new ConnectionFactory() {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare("topic_logs", ExchangeType.Topic);
            var queueName = channel.QueueDeclare().QueueName;

            if (args.Length < 1)
            {
                Console.Error.WriteLine("Usage: {0} [binding_key...]", Environment.GetCommandLineArgs()[0]);
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
                Environment.ExitCode = 1;
                return;
            }

            foreach (var routingKey in args)
            {
                Console.WriteLine($"Bind routing key -> {routingKey}");
                channel.QueueBind(queueName, "topic_logs",routingKey);
            }

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.WriteLine($"[{workerId}]  Receive Content:[{message}]  RoutingKey:[{routingKey}]");
            };
            channel.BasicConsume(queueName, true, consumer);

            Console.WriteLine("Press anyKey to exit");
            Console.ReadKey();
        }
    }
}
