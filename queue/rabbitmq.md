## docker容器安装rabbitmq集群

### 1.安装前准备

拉取rabbitmq镜像：


```bash
docker pull rabbitmq:3.7.17-management
```

### 2.启动容器，建立集群

这里我们在同一台宿主机上进行安装，创建3个rabbitmq节点：

```bash
docker run -d --hostname rabbitmq01 --name rabbitmqCluster01 -v /root/rabbitmqcluster/rabbitmq01:/var/lib/rabbitmq -p 15672:15672 -p 5672:5672  -p 15674:15674 -p 15670:15670 -e RABBITMQ_ERLANG_COOKIE='rabbitmqCookie' rabbitmq:3.7.17-management

docker run -d --hostname rabbitmq02 --name rabbitmqCluster02 -v /root/rabbitmqcluster/rabbitmq02:/var/lib/rabbitmq -p 15682:15672 -p 5682:5672 -p 15684:15674 -p 15680:15670 -e RABBITMQ_ERLANG_COOKIE='rabbitmqCookie'  --link rabbitmqCluster01:rabbitmq01 rabbitmq:3.7.17-management

docker run -d --hostname rabbitmq03 --name rabbitmqCluster03 -v /root/rabbitmqcluster/rabbitmq03:/var/lib/rabbitmq -p 15692:15672 -p 5692:5672 -p 15694:15674 -p 15690:15670 -e RABBITMQ_ERLANG_COOKIE='rabbitmqCookie'  --link rabbitmqCluster01:rabbitmq01 --link rabbitmqCluster02:rabbitmq02  rabbitmq:3.7.17-management
```

注意点：

1. Erlang Cookie值必须相同，也就是RABBITMQ_ERLANG_COOKIE参数的值必须相同
2. 默认端口：
    - 4369: epmd, a peer discovery service used by RabbitMQ nodes and CLI tools * 5672, 
    - 5671: used by AMQP 0-9-1 and 1.0 clients without and with TLS
    - 25672: used for inter-node and CLI tools communication (Erlang distribution server port) and is allocated from a dynamic range (limited to a single port by default, computed as AMQP port + 20000). Unless external connections on these ports are really necessary (e.g. the cluster uses federation or CLI tools are used on machines outside the subnet), these ports should not be publicly exposed. See networking guide for details.
    - 35672-35682: used by CLI tools (Erlang distribution client ports) for communication with nodes and is allocated from a dynamic range (computed as server distribution port + 10000 through server distribution port + 10010). See networking guide for details.
    - 15672: HTTP API clients, management UI and rabbitmqadmin (only if the management plugin is enabled)
    - 61613, 61614: STOMP clients without and with TLS (only if the STOMP plugin is enabled)
    - 1883, 8883: (MQTT clients without and with TLS, if the MQTT plugin is enabled
    - 15674: STOMP-over-WebSockets clients (only if the Web STOMP plugin is enabled)
    - 15675: MQTT-over-WebSockets clients (only if the Web MQTT plugin is enabled)