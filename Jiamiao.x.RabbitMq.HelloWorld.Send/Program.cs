using System;
using System.Text;
using RabbitMQ.Client;

namespace Jiamiao.x.RabbitMq.HelloWorld.Send
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== SEND ==========");

            var factory = new ConnectionFactory() {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("hello", false, false, false, null);

            var message = "Hello world";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);

            Console.WriteLine($"---------- Send -> hello -> {message} ----------");

            Console.WriteLine("========== FINISH ==========");
            Console.ReadKey();
        }
    }
}
