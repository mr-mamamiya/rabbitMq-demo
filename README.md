- [前置条件](#%e5%89%8d%e7%bd%ae%e6%9d%a1%e4%bb%b6)
- [1.HelloWorld](#1helloworld)
  - [参考文档](#%e5%8f%82%e8%80%83%e6%96%87%e6%a1%a3)
  - [流程图](#%e6%b5%81%e7%a8%8b%e5%9b%be)
  - [解析](#%e8%a7%a3%e6%9e%90)
    - [业务解析](#%e4%b8%9a%e5%8a%a1%e8%a7%a3%e6%9e%90)
    - [生产者](#%e7%94%9f%e4%ba%a7%e8%80%85)
    - [消费者](#%e6%b6%88%e8%b4%b9%e8%80%85)
  - [运行](#%e8%bf%90%e8%a1%8c)
- [2.WorkQueues](#2workqueues)
  - [参考文档](#%e5%8f%82%e8%80%83%e6%96%87%e6%a1%a3-1)
- [3.Publish/Subscribe](#3publishsubscribe)
  - [参考文档](#%e5%8f%82%e8%80%83%e6%96%87%e6%a1%a3-2)
- [4.Routing](#4routing)
  - [参考文档](#%e5%8f%82%e8%80%83%e6%96%87%e6%a1%a3-3)
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
### 运行


## 2.WorkQueues
### 参考文档
https://yq.aliyun.com/articles/642456


## 3.Publish/Subscribe
### 参考文档
https://yq.aliyun.com/articles/642455

## 4.Routing
### 参考文档
https://yq.aliyun.com/articles/642453

## 5.Topic
### 参考文档
https://yq.aliyun.com/articles/642452

## 6.RPC
### 参考文档
https://yq.aliyun.com/articles/672082