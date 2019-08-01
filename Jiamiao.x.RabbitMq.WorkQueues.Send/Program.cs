using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace Jiamiao.x.RabbitMq.WorkQueues.Send
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== WorkQueues.Send ==========");
            var factory = new ConnectionFactory() {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("task_queue", true, false, false, null);

            for (int i = 0; i < 50; i++)
            {
                var message = $"Send message -> {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                var body = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();

                properties.Persistent = true;
                channel.BasicPublish("", "task_queue", properties, body);
                Console.WriteLine($"[{message}]  --> Send");
                Thread.Sleep(2000);
            }
            
            Console.WriteLine("Press anyKey to exit");
            Console.ReadKey();
        }

    }
}
