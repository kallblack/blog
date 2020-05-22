# <center>docker容器安装ELK(不使用filebeat)</center>


## 1. 拉取所有需要的镜像

所有镜像统一版本号，这里我用的是7.7.0版本

```bash
#拉取ElasticSearch镜像
docker pull elasticsearch:7.7.0

#拉取Kibana镜像
docker pull kibana:7.7.0

#拉取Logstash镜像
docker pull logstash:7.7.0
```

## 2.启动ElasticSearch

启动两个端口，9200为数据查询和写入端口，也即是业务端口，9300为集群端口，同步集群数据，此处我单节点部署

指定日志输出格式为json格式，默认情况下也为json格式，默认输出路径 /var/lib/docker/containers/*/*.log

日志文件最多保留三个，每个最多10M

容器开机自启

传递参数为单节点部署

数据存储映射至宿主机

需要给 /home/yun/ELK/elasticsearch/data 赋予权限，否则报权限不足的错误

```bash
#赋予权限
chmod 755 /home/yun/ELK/elasticsearch/data

#启动ElasticSearch
docker run -d \
    -p 9200:9200 \
    -p 9300:9300 \
    --name elasticsearch \
    --restart=always \
    -e "discovery.type=single-node" \
    -v /home/yun/ELK/elasticsearch/data:/usr/share/elasticsearch/data \
    elasticsearch:7.7.0
```

## 3.启动Logstash 

先给出Logstash的配置文件

Logstash配置文件为：

```bash
input {
  tcp {
    host => "0.0.0.0"
    port => "5043"
  }
}
filter {
    grok {
            match => { "message" => "%{TIMESTAMP_ISO8601:time}\|%{WORD:type}\|%{LOGLEVEL:level}\|%{JAVACLASS:class}\|%{GREEDYDATA:msg}" }
        }
}

output {
  stdout { codec => rubydebug }
  elasticsearch {
        hosts => [ "elasticsearch:9200" ]
        index => "%{[type]}-%{+YYYY.MM.dd}"
    }
}
```

启动Logstash

```bash
docker run -p 5043:5043 -d \
    --name logstash \
    --link elasticsearch \
    --restart=always \
    -v /home/yun/ELK/logstash/conf/logstash.conf:/usr/share/logstash/pipeline/logstash.conf \
    logstash:7.7.0
```

## 4.启动Kibana

先创建一个kibana的配置文件

```bash
vi kibana.yml

#然后输入如下内容：
server.name: kibana
server.host: "0"
elasticsearch.hosts: [ "http://elasticsearch:9200" ]
monitoring.ui.container.elasticsearch.enabled: true
#配置语言为中文
i18n.locale: "zh-CN"
```

启动kibana

```bash
docker run -p 5601:5601 -d \
    --name kibana \
    --link elasticsearch \
    --restart=always \
    -e ELASTICSEARCH_URL=http://elasticsearch:9200 \
    -v /home/yun/ELK/kibana/conf/kibana.yml:/usr/share/kibana/config/kibana.yml \
    kibana:7.7.0
```

## 5.配置Nlog

```xml
<targets>

 <target xsi:type="Network" keepConnection="false" name="ownFile-web" address="tcp://192.168.65.227:5043" layout="${longdate}|station|${uppercase:${level}}|${logger}|${message} ${exception:format=message}" />

</targets>
```
