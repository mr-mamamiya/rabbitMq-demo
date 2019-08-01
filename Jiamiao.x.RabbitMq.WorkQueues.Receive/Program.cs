using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Jiamiao.x.RabbitMq.WorkQueues.Receive
{
    class Program
    {
        static void Main(string[] args)
        {
            var receiverName = args[0];
            Console.WriteLine($"========== WorkQueues.Receive-{receiverName} ==========");

            var factory = new ConnectionFactory() {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("task_queue", true, false, false, null);

            channel.BasicQos(0, 1, false);

            Console.WriteLine("Waiting for message...");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine("---------- RECEIVE MESSAGE ----------");
                Console.WriteLine($"${message}");
                Console.WriteLine("-------------------------------------");

                Thread.Sleep(5000);

                channel.BasicAck(ea.DeliveryTag, false);
            };

            channel.BasicConsume("task_queue", false, consumer);
            Console.WriteLine("Press anyKey to exit");

            Console.ReadKey();


        }
    }
}
