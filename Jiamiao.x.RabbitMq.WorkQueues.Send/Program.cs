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

            // 声明队列，通过指定durable参数为true，对消息进行持久化处理。 
            // 生产者和消费者声明队列的时候都需要指定durable参数为true，才能对消息进行持久化处理
            channel.QueueDeclare("task_queue", true, false, false, null);

            for (int i = 0; i < 50; i++)
            {
                var message = $"Send message -> {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                var body = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();
                // 将消息标记为持久的
                properties.Persistent = true;

                /*
                 * 关于消息持久性的说明
                 * 将消息标记为Persistent并不能完全保证消息不会丢失。
                 * 尽管它告诉RabbitMQ将消息保存到磁盘，但当RabbitMQ接收到消息并且尚未保存消息时仍有一段时间间隔。
                 * 此外，RabbitMQ不会为每条消息执行fsync(2) - 它可能只是保存到缓存中，并没有真正写入磁盘。
                 * 消息的持久化保证并不健壮，但对于简单的任务队列来说已经足够了。如果您需要一个更加健壮的保证，可以使用 发布者确认。
                 */

                channel.BasicPublish("", "task_queue", properties, body);
                Console.WriteLine($"[{message}]  --> Send");
                Thread.Sleep(2000);
            }
            
            Console.WriteLine("Press anyKey to exit");
            Console.ReadKey();
        }

    }
}
