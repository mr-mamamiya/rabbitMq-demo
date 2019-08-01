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

            // 不要向一个Worker发送新的消息，直到它处理并确认了前一个消息
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

                // 手动发送消息确认信号
                channel.BasicAck(ea.DeliveryTag, false);
            };

            // autoAck:false - 关闭自动消息确认，调用`BasicAck`方法进行手动消息确认。
            // autoAck:true  - 开启自动消息确认，当消费者接收到消息后就自动发送ack信号，无论消息是否正确处理完毕。
            channel.BasicConsume("task_queue", false, consumer); 
            Console.WriteLine("Press anyKey to exit");

            Console.ReadKey();


        }
    }
}
