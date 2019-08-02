using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace Jiamiao.x.RabbitMq.Topic.Send
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== Topic.Send ==========");

            var factory = new ConnectionFactory() {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            string[] projectNames = {"A","B","C" };
            string[] featureNames = {"Order", "Customer", "Product"};
            string[] logTypes = {"Monitor", "Info", "Warning", "Error"};

            channel.ExchangeDeclare("topic_logs", ExchangeType.Topic);

            var random = new Random();
            for (int i = 0; i < 300; i++)
            {
                var projectName = projectNames[random.Next(0, projectNames.Length)];
                var featureName = featureNames[random.Next(0, featureNames.Length)];
                var logType = logTypes[random.Next(0, logTypes.Length)];

                var routingKey = $"{projectName}.{featureName}.{logType}";
                var message = $"{routingKey} : {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("topic_logs", routingKey, null, body);
                Console.WriteLine($"Send Content:{message} to {routingKey}");
                Thread.Sleep(1000);
            }

            Console.WriteLine("Press anyKey to exit");
            Console.ReadKey();
        }
    }
}
