using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Jiamiao.x.RabbitMq.Publish_Subscribe.Subscribe
{
    class Program
    {
        // 来自logs交换器的数据转发到了两个由服务器随机分配名称的队列
        static void Main(string[] args)
        {
            Console.WriteLine("========== Publish/Subscribe.Subscribe ==========");

            var factory = new ConnectionFactory() {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare("logs","fanout");

            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, "logs", "");

            Console.WriteLine("Waiting for logs...");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine("---------- RECEIVE MESSAGE ----------");
                Console.WriteLine($"{message}   -->  queueName={queueName}");
                Console.WriteLine("-------------------------------------");
                
                Thread.Sleep(1000);
            };

            channel.BasicConsume(queueName, true, consumer);

            Console.WriteLine("========== Press anyKey to exit =========="); Console.WriteLine("");
            Console.ReadKey();
        }
    }
}
