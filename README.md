- [前置条件](#%e5%89%8d%e7%bd%ae%e6%9d%a1%e4%bb%b6)
- [1.HelloWorld](#1helloworld)
  - [参考文档](#%e5%8f%82%e8%80%83%e6%96%87%e6%a1%a3)
  - [流程图](#%e6%b5%81%e7%a8%8b%e5%9b%be)
  - [解析](#%e8%a7%a3%e6%9e%90)
    - [业务解析](#%e4%b8%9a%e5%8a%a1%e8%a7%a3%e6%9e%90)
    - [生产者](#%e7%94%9f%e4%ba%a7%e8%80%85)
    - [消费者](#%e6%b6%88%e8%b4%b9%e8%80%85)
  - [运行](#%e8%bf%90%e8%a1%8c)
  - [全部代码](#%e5%85%a8%e9%83%a8%e4%bb%a3%e7%a0%81)
    - [生产者](#%e7%94%9f%e4%ba%a7%e8%80%85-1)
    - [消费者](#%e6%b6%88%e8%b4%b9%e8%80%85-1)
- [2.WorkQueues](#2workqueues)
  - [参考文档](#%e5%8f%82%e8%80%83%e6%96%87%e6%a1%a3-1)
  - [流程图](#%e6%b5%81%e7%a8%8b%e5%9b%be-1)
  - [解析](#%e8%a7%a3%e6%9e%90-1)
    - [业务流程](#%e4%b8%9a%e5%8a%a1%e6%b5%81%e7%a8%8b)
    - [生产者](#%e7%94%9f%e4%ba%a7%e8%80%85-2)
    - [消费者](#%e6%b6%88%e8%b4%b9%e8%80%85-2)
  - [运行](#%e8%bf%90%e8%a1%8c-1)
  - [全部代码](#%e5%85%a8%e9%83%a8%e4%bb%a3%e7%a0%81-1)
    - [生产者](#%e7%94%9f%e4%ba%a7%e8%80%85-3)
    - [消费者](#%e6%b6%88%e8%b4%b9%e8%80%85-3)
- [3.Publish/Subscribe](#3publishsubscribe)
  - [参考文档](#%e5%8f%82%e8%80%83%e6%96%87%e6%a1%a3-2)
  - [流程图](#%e6%b5%81%e7%a8%8b%e5%9b%be-2)
  - [解析](#%e8%a7%a3%e6%9e%90-2)
    - [交换器](#%e4%ba%a4%e6%8d%a2%e5%99%a8)
    - [业务解析](#%e4%b8%9a%e5%8a%a1%e8%a7%a3%e6%9e%90-1)
    - [生产者](#%e7%94%9f%e4%ba%a7%e8%80%85-4)
    - [消费者](#%e6%b6%88%e8%b4%b9%e8%80%85-4)
  - [运行](#%e8%bf%90%e8%a1%8c-2)
  - [全部代码](#%e5%85%a8%e9%83%a8%e4%bb%a3%e7%a0%81-2)
    - [生产者](#%e7%94%9f%e4%ba%a7%e8%80%85-5)
    - [消费者](#%e6%b6%88%e8%b4%b9%e8%80%85-5)
- [4.Routing](#4routing)
  - [参考文档](#%e5%8f%82%e8%80%83%e6%96%87%e6%a1%a3-3)
  - [流程图](#%e6%b5%81%e7%a8%8b%e5%9b%be-3)
  - [解析](#%e8%a7%a3%e6%9e%90-3)
    - [业务解析](#%e4%b8%9a%e5%8a%a1%e8%a7%a3%e6%9e%90-2)
    - [生产者](#%e7%94%9f%e4%ba%a7%e8%80%85-6)
    - [消费者](#%e6%b6%88%e8%b4%b9%e8%80%85-6)
  - [运行](#%e8%bf%90%e8%a1%8c-3)
  - [全部代码](#%e5%85%a8%e9%83%a8%e4%bb%a3%e7%a0%81-3)
- [5.Topic](#5topic)
  - [参考文档](#%e5%8f%82%e8%80%83%e6%96%87%e6%a1%a3-4)
- [6.RPC](#6rpc)
  - [参考文档](#%e5%8f%82%e8%80%83%e6%96%87%e6%a1%a3-5)

## 前置条件
1. 已安装RabbitMQ，并且运行在`localhost`标准端口`5672`上
2. 项目均为`.net core 2.2`控制台项目
3. 项目均需安装`RabbitMQ.Client`，目前版本为`5.1.0`

## 1.HelloWorld
### 参考文档
https://yq.aliyun.com/articles/642459
### 流程图
![HelloWorld.png](https://i.loli.net/2019/08/02/5d440e169ca5562981.png)
### 解析
#### 业务解析
生产者将`Hello world`字符串写入名字为`hello`的队列中，消息队列将此字符串推送至消费者，由消费者打印到控制台
#### 生产者
- 项目名称：Jiamiao.x.RabbitMq.HelloWorld.Send
- 实例化*ConnectionFactory*，创建*Connection*，创建*Channel*
  ```C#
  var factory = new ConnectionFactory() {HostName = "localhost"};
  using var connection = factory.CreateConnection();
  using var channel = connection.CreateModel();
  ```
- 定义名字为*hello*的消息队列
  ```C#
  channel.QueueDeclare("hello", false, false, false, null);
  ```
- 发送*Hello world*字符串到队列中
  ```C#
  var message = "Hello world";
  var body = Encoding.UTF8.GetBytes(message);
  channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: null, body: body);
  ```
#### 消费者
- 项目名称：Jiamiao.x.RabbitMq.HelloWorld.Receive
- 实例化*ConnectionFactory*，创建*Connection*，创建*Channel*，定义*hello*消息队列与生产者代码一致
- 创建消费者*consumer*，并指定消费者接受到消息的事件
  ```C#
  var consumer = new EventingBasicConsumer(channel);
  consumer.Received += (model, ea) =>
  {
      var body = ea.Body;
      var message = Encoding.UTF8.GetString(body);
      Console.WriteLine($"message content = {message}");
  };
  ```
- 为*channel*指定消费者
  ```C#
  channel.BasicConsume("hello", true, consumer);
  ```
### 运行
1. 运行消费者
   ```bash
   cd Jiamiao.x.RabbitMq.HelloWorld.Receive
   dotnet run
   ```
2. 运行生产者
   ```bash
   cd Jiamiao.x.RabbitMq.HelloWorld.Send
   dotnet run
   ```
### 全部代码
#### 生产者
```C#
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
```
#### 消费者
```C#
using System;
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
                Console.WriteLine($"message content = {message}");
            };

            channel.BasicConsume("hello", true, consumer);

            Console.WriteLine("========== FINISH ==========");
            Console.ReadKey();
        }
    }
}

```

## 2.WorkQueues
### 参考文档
https://yq.aliyun.com/articles/642456
### 流程图
![WorkQueue.png](https://i.loli.net/2019/08/03/NkjqrLyFbAK8Jxu.png)
### 解析
#### 业务流程
生产者将当前时间写入`task_queue`消息队列中，标记消息持久化，同时启用多个消费者对消息进行消费，消费者处理完消息之后手动确认消息已处理完
#### 生产者
- 项目名称：Jiamiao.x.RabbitMq.WorkQueues.Send
- 实例化*ConnectionFactory*，创建*Connection*，创建*Channel*
  ```C#
  var factory = new ConnectionFactory() {HostName = "localhost"};
  using var connection = factory.CreateConnection();
  using var channel = connection.CreateModel();
  ```
- 定义*task_queue*消息队列，其中指定*durable*参数为*true*才能对消息进行持久化
  ```C#
  channel.QueueDeclare("task_queue",  durable:true, exclusive:false, autoDelete:false, arguments: null);
  ```
- *properties.Persistent=true* 将消息标记为持久的，并发送消息
  ```C#
  var message = $"Send message -> {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
  var body = Encoding.UTF8.GetBytes(message);
  var properties = channel.CreateBasicProperties();
  properties.Persistent = true;
  channel.BasicPublish(exchange: "", routingKey: "task_queue", basicProperties: properties, body: body); 
  ```
- 备注：即使将消息标记为持久化，但*RabbitMQ*接收到消息后并不会马上进行持久化，而是写入缓存，一段时间后才进行持久化操作。消息的持久化保证并不健壮，对于简单的消息队列来说足够，如果需要足够健壮的保证，可以使用 [发布者确认](https://www.rabbitmq.com/confirms.html)
#### 消费者
- 项目名称：Jiamiao.x.RabbitMq.WorkQueues.Receive
- 实例化*ConnectionFactory*，创建*Connection*，创建*Channel*，定义*task_queue*消息队列与生产者代码一致
- 设置发送消息策略：不要向一个消费者发送新消息，知道他处理并确认了上一个消息
  ```C#
  channel.BasicQos(0, 1, false);
  ```
- 创建消费者*consumer*，指定消费者接受到消息的处理事件，并手动回复消息确认信号
  ```C#
  var consumer = new EventingBasicConsumer(channel);
  consumer.Received += (model, ea) =>
  {
      var body = ea.Body;
      var message = Encoding.UTF8.GetString(body);
      Console.WriteLine($"{message}");
      // 为保证看出效果，假设处理任务需要5秒钟
      Thread.Sleep(5000);
      // 手动发送消息确认信号
      channel.BasicAck(ea.DeliveryTag, false);
  };
  ```
- 绑定*Channel*消费者，并指定不自动回复消息确认信号
  ```C#
  channel.BasicConsume("task_queue", false, consumer); 
  ```

### 运行
1. 运行消费者A
   ```bash
   cd Jiamiao.x.RabbitMq.WorkQueues.Receive
   dotnet run receiverA
   ```
2. 运行消费者B
   ```bash
   cd Jiamiao.x.RabbitMq.WorkQueues.Receive
   dotnet run receiverB
   ```
3. 运行生产者
   ```bash
   cd Jiamiao.x.RabbitMq.WorkQueues.Send
   dotnet run
   ```
### 全部代码
#### 生产者
```C#
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
            channel.QueueDeclare("task_queue",  durable:true, exclusive:false, autoDelete:false, arguments: null);
            // 为了模拟效果，这里发出50条消息，每条消息间隔2秒钟
            for (int i = 0; i < 50; i++)
            {
                var message = $"Send message -> {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
                var body = Encoding.UTF8.GetBytes(message);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                channel.BasicPublish(exchange: "", routingKey: "task_queue", basicProperties: properties, body: body); 
                Console.WriteLine($"[{message}]  --> Send");
                Thread.Sleep(2000);
            }
            Console.WriteLine("Press anyKey to exit");
            Console.ReadKey();
        }

    }
}
```
#### 消费者
```C#
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
                Console.WriteLine($"{message}");
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
```

## 3.Publish/Subscribe
### 参考文档
https://yq.aliyun.com/articles/642455
### 流程图
![PublishSubscribe.png](https://i.loli.net/2019/08/06/l6x2eH4VkufaYAy.png)
### 解析
#### 交换器
交换器一方面接受来自消息生产者的消息，另一方面按照一定的逻辑规则将消息推送到消息队列中，

*在RabbitMQ中，消息传递模型的核心理念是生产者从来不会把任何消息直接发送到队列，而是发送到交换机，由交换机自行决定分发到指定(一个或多个)队列中，消息生产者甚至不知道消息是否会被分到任何队列中*

#### 业务解析
消息生产者将消息发送到名字为*logs*的*fanout*类型的交换机(*fanout*为广播交换机)，消费者A和消费者B订阅*logs*交换机，接受生产者发送的消息，生产者A将消息打印到控制台，生产者B将消息保存到本地文件
#### 生产者
- 项目名称：Jiamiao.x.RabbitMq.Publish_Subscribe.Publish
- 实例化*ConnectionFactory*，创建*Connection*，创建*Channel*
  ```C#
  var factory = new ConnectionFactory() {HostName = "localhost"};
  using var connection = factory.CreateConnection();
  using var channel = connection.CreateModel();
  ```
- 定义名字为*logs*的*fanout*交换机
  ```C#
  channel.ExchangeDeclare("logs",ExchangeType.Fanout);
  ```
- 发送当前事件到交换机
  ```C#
  var message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
  var body = Encoding.UTF8.GetBytes(message);
  channel.BasicPublish(exchange:"logs", routingKey:"", basicProperties:null, body:body);
  ```

#### 消费者
- 项目名称：Jiamiao.x.RabbitMq.Publish_Subscribe.Subscribe
- 实例化*ConnectionFactory*，创建*Connection*，创建*Channel*，定义名字为*logs*的*fanout*交换机与生产者代码一致
- 创建一个非持久化、独占、自动删除的随机命名的消息队列，并绑定到*logs*交换机上
  ```C#
  var queueName = channel.QueueDeclare().QueueName;
  channel.QueueBind(queueName, "logs", "");
  ```
- 创建消费者*consumer*，并指定接收到消息的处理业务逻辑
  ```C#
  var consumer = new EventingBasicConsumer(channel);
  consumer.Received += (model, ea) =>
  {
      var body = ea.Body;
      var message = Encoding.UTF8.GetString(body);
      Console.WriteLine($"{message}   -->  queueName={queueName}");
  };
  ```
- 绑定消费者到消息队列上
  ```C#
  channel.BasicConsume(queue:queueName, autoAck:true, consumer:consumer);
  ```

### 运行
1. 运行消费者A
   ```bash
   cd Jiamiao.x.RabbitMq.Publish_Subscribe.Subscribe
   dotnet run
   ```
2. 运行消费者B(将控制台内容输出到*logs_from_rabbit.log*本地文件中)
   ```bash
   cd Jiamiao.x.RabbitMq.Publish_Subscribe.Subscribe
   dotnet run > logs_from_rabbit.log
   ````
3. 运行生产者
   ```bash
   cd Jiamiao.x.RabbitMq.Publish_Subscribe.Publish
   dotnet run
   ```
### 全部代码
#### 生产者
```C#
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
            channel.ExchangeDeclare("logs",ExchangeType.Fanout);
            // 为模拟效果，发送当前消息50次，消息间隔1秒钟
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
```
#### 消费者
```C#
using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Jiamiao.x.RabbitMq.Publish_Subscribe.Subscribe
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========== Publish/Subscribe.Subscribe ==========");
            var factory = new ConnectionFactory() {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare("logs",ExchangeType.Fanout);
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, "logs", "");
            Console.WriteLine("Waiting for logs...");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"{message}   -->  queueName={queueName}");
            };
            channel.BasicConsume(queue:queueName, autoAck:true, consumer:consumer);
            Console.WriteLine("========== Press anyKey to exit =========="); Console.WriteLine("");
            Console.ReadKey();
        }
    }
}
```

## 4.Routing
### 参考文档
https://yq.aliyun.com/articles/642453
### 流程图
![Routing.png](https://i.loli.net/2019/08/07/9YGtIXNMdJSWgB2.png)
### 解析
#### 业务解析
消息生产者产生4中类型的消息，分别是`error` `info` `monitor` `warning`，将消息发送到名字为*direct_logs*类型为*direct*的交换机，并按照消息类型指定消息的*routingKey*，消费者从启动命令参数获取需要订阅的*routingKey*，并将接收到的消息打印到控制台
#### 生产者
- 项目名称：Jiamiao.x.RabbitMq.Routing.Send
- 实例化*ConnectionFactory*，创建*Connection*，创建*Channel*
  ```C#
  var factory = new ConnectionFactory() { HostName = "localhost" };
  using var connection = factory.CreateConnection();
  using var channel = connection.CreateModel();
  ```
- 定义名字为*direct_logs*的*direct*交换机
  ```C#
  channel.ExchangeDeclare("direct_logs", ExchangeType.Direct);
  ```
- 发送指定内容到交换机，其中指定*routingKey*
  ```C#
  var routingKey = "routingKey";
  var message = $"{routingKey} : {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
  var body = Encoding.UTF8.GetBytes(message);
  channel.BasicPublish("direct_logs", logType, null, body);
  ```
#### 消费者
- 项目名称：Jiamiao.x.RabbitMq.Routing.Receive
- 实例化*ConnectionFactory*，创建*Connection*，创建*Channel*，定义名字为*direct_logs*的*direct*交换机与生产者代码一致
- 创建一个非持久化、独占、自动删除的随机命名的消息队列，绑定到*direct*交换机上，并指定一个*routingKey*
  ```C#
  var queueName = channel.QueueDeclare().QueueName;
  channel.QueueBind(queue:queueName, exchange:"direct_logs", routingKey:"routingKey");
  ```
- 创建消费者*consumer*，并指定接受到消息的处理业务逻辑
  ```C#
  var consumer = new EventingBasicConsumer(channel);
  consumer.Received += (model, ea) =>
  {
      var body = ea.Body;
      var message = Encoding.UTF8.GetString(body);
      var routingKey = ea.RoutingKey;
      Console.WriteLine($"[{workedId}]  Receive Content:[{message}]  RoutingKey:[{routingKey}]");
  };
  ```
- 绑定消费者到消息队列上
  ```C#
  channel.BasicConsume(queueName, true, consumer);
  ```

### 运行
1. 运行消费者A，接收*error*类型的*routingKey*
   ```bash
   cd Jiamiao.x.RabbitMq.Routing.Receive
   dotnet run error
   ```
2. 运行消费者B，接受*info* *monitor* *warning*三种类型的*routingKey*
   ```bash
   cd Jiamiao.x.RabbitMq.Routing.Receive
   dotnet run info monitor warning
   ```
3. 运行生产者
   ```bash
   cd Jiamiao.x.RabbitMq.Routing.Send
   dotnet run
   ```

### 全部代码

## 5.Topic
### 参考文档
https://yq.aliyun.com/articles/642452

## 6.RPC
### 参考文档
https://yq.aliyun.com/articles/672082
