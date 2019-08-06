using System;
using System.Text;
using System.Threading;
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
            channel.ExchangeDeclare("logs",ExchangeType.Fanout);

            for (int i = 0; i < 50; i++)
            { 
                var message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange:"logs", routingKey:"", basicProperties:null, body:body);
                Console.WriteLine($"Send Content [{message}]");
                Thread.Sleep(1000);
            }


            

            Console.WriteLine("========== Press anyKey to exit ==========");
            Console.ReadKey();
        }
    }
}
