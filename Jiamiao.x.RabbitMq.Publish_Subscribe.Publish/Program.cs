using System;
using System.Text;
using RabbitMQ.Client;

namespace Jiamiao.x.RabbitMq.Publish_Subscribe.Publish
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== Publish/Subscribe.Publish ==========");

            var factory = new ConnectionFactory() {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            // 定义名叫logs的广播类型交换器
            channel.ExchangeDeclare("logs","fanout");

            for (int i = 0; i < 50; i++)
            {

                var message = $"Publish message-{i} -> {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("logs", "", null, body);
                Console.WriteLine($"Send Content [{message}]");
            }


            

            Console.WriteLine("========== Press anyKey to exit ==========");
            Console.ReadKey();
        }
    }
}
