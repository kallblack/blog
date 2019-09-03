## docker容器安装rabbitmq集群

### 1. 安装前准备

拉取rabbitmq镜像：


```bash
docker pull rabbitmq:3.7.17-management
```

### 2. 启动容器，建立集群

这里我们在同一台宿主机上进行安装，创建3个rabbitmq节点：

```bash
docker run -d --hostname rabbitmq01 --name rabbitmqCluster01 -v /root/rabbitmqcluster/rabbitmq01:/var/lib/rabbitmq -p 15672:15672 -p 5672:5672  -p 15674:15674 -e RABBITMQ_ERLANG_COOKIE='rabbitmqCookie' rabbitmq:3.7.17-management

docker run -d --hostname rabbitmq02 --name rabbitmqCluster02 -v /root/rabbitmqcluster/rabbitmq02:/var/lib/rabbitmq -p 15682:15672 -p 5682:5672 -p 15684:15674 -e RABBITMQ_ERLANG_COOKIE='rabbitmqCookie'  --link rabbitmqCluster01:rabbitmq01 rabbitmq:3.7.17-management

docker run -d --hostname rabbitmq03 --name rabbitmqCluster03 -v /root/rabbitmqcluster/rabbitmq03:/var/lib/rabbitmq -p 15692:15672 -p 5692:5672 -p 15694:15674 -e RABBITMQ_ERLANG_COOKIE='rabbitmqCookie'  --link rabbitmqCluster01:rabbitmq01 --link rabbitmqCluster02:rabbitmq02  rabbitmq:3.7.17-management
```

注意点：

+ Erlang Cookie值必须相同，也就是RABBITMQ_ERLANG_COOKIE参数的值必须相同
+ 常用默认端口：
    - 4369: epmd, a peer discovery service used by RabbitMQ nodes and CLI tools
    - 5672, 5671: used by AMQP 0-9-1 and 1.0 clients without and with TLS    
    - 15672: HTTP API clients, management UI and rabbitmqadmin (only if the management plugin is enabled)
    - 15674: STOMP-over-WebSockets clients (only if the Web STOMP plugin is enabled)
    - 15675: MQTT-over-WebSockets clients (only if the Web MQTT plugin is enabled)

### 3. 分别设置三个节点

设置节点1：

```bash
docker exec -it rabbitmqCluster01 bash
rabbitmqctl stop_app
rabbitmqctl reset
rabbitmqctl start_app
rabbitmq-plugins enable rabbitmq_tracing rabbitmq_stomp rabbitmq_web_stomp rabbitmq_web_mqtt #启动相关插件
rabbitmqctl set_policy ha-all "^" '{"ha-mode":"all"}' #设置为镜像模式
exit
```

设置节点2：

```bash
docker exec -it rabbitmqCluster02 bash
rabbitmqctl stop_app
rabbitmqctl reset
rabbitmqctl join_cluster --ram rabbit@rabbitmq01
rabbitmqctl start_app
rabbitmq-plugins enable rabbitmq_tracing rabbitmq_stomp rabbitmq_web_stomp rabbitmq_web_mqtt #启动相关插件
exit
```

设置节点3：

```bash
docker exec -it rabbitmqCluster03 bash
rabbitmqctl stop_app
rabbitmqctl reset
rabbitmqctl join_cluster --ram rabbit@rabbitmq01
rabbitmqctl start_app
rabbitmq-plugins enable rabbitmq_tracing rabbitmq_stomp rabbitmq_web_stomp rabbitmq_web_mqtt #启动相关插件
exit
```

### 4. Web UI界面
设置好之后，使用http://宿主机ip:15672 进行访问了，默认账号密码是guest/guest

![UI界面](https://github.com/kallblack/blog/blob/master/images/queue/rabbitmq%E7%9A%84UI%E7%95%8C%E9%9D%A2.png)


### 5. 前端页面js通过websocket连rabbitmq

[前端页面持续展示rabbitmq所推送的消息示例](https://github.com/kallblack/blog/tree/master/queue/rabbitmq/websocket-rabbitmq-example)