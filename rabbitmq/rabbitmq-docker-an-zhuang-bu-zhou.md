# RabbitMQ Docker安装步骤

## 拉取镜像

```bash
docker pull rabbitmq:management
```

## 运行镜像

```bash
docker run -d --hostname my-rabbit -p 5671:5671 -p 5672:5672 -p 4369:4369 -p 25672:25672 -p 15671:15671 -p 15672:15672  --name okong-rabbit rabbitmq:management
```

## 进RabbitMQ管理页面

```text
http://localhost:15672
```

默认账户密码：guest/guest

