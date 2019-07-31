﻿using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Jiamiao.x.RabbitMq.HelloWorld.Receive
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== RECEIVE ==========");

            var factory = new ConnectionFactory() {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("hello", false, false, false, null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine("---------- RECEIVE MESSAGE ----------");
                Console.WriteLine($"${message}");
                Console.WriteLine("-------------------------------------");
            };

            channel.BasicConsume("hello", true, consumer);

            Console.WriteLine("========== FINISH ==========");
            Console.ReadKey();
        }
    }
}
