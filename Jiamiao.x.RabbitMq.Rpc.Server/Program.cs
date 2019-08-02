using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Jiamiao.x.RabbitMq.Rpc.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== RPC.Server ==========");

            var factory = new ConnectionFactory() {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("rpc_queue", false, false, false, null);
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume("rpc_queue", false, consumer);
            consumer.Received += (model, ea) =>
            {
                string response = null;

                var body = ea.Body;
                var props = ea.BasicProperties;
                var replyProps = channel.CreateBasicProperties();
                replyProps.CorrelationId = props.CorrelationId;

                try
                {
                    var message = Encoding.UTF8.GetString(body);
                    var n = int.Parse(message);
                    Console.WriteLine($" [.] fib({message})");
                    response = Fib(n).ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine($" [.] {e.Message}");
                    response = "";
                }
                finally
                {
                    var responseByte = Encoding.UTF8.GetBytes(response);
                    channel.BasicPublish("",props.ReplyTo,replyProps,responseByte);
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };

            Console.WriteLine(" Press anyKey to exit.");
            Console.ReadKey();
        }

        private static int Fib(int n)
        {
            if (n == 0 || n == 1)
                return n;
            return Fib(n - 1) + Fib(n - 2);
        }
    }
}
