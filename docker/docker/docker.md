# <center>docker常用命令</center>

## 1. docker守护进程代理上网设置

```bash
#1.在[Service]中增加一行
vim /lib/systemd/system/docker.service
[Service]
Environment="HTTP_PROXY=http://[proxy-addr]:[proxy-port]/" "HTTPS_PROXY=http://[proxy-addr]:[proxy-port]/" "NO_PROXY=[局域网地址]"
#2.更新配置
systemctl daemon-reload 
#3.重启Docker服务
systemctl restart docker
```

## 2. docker启动带界面的程序

```bash
sudo apt-get install x11-xserver-utils
#注意这行命令只能在本地执行，远程ssh连进来的执行会报错，执行后提示“access control disabled, clients can connect from any host”表示成功
xhost + 
```
在启动docker容器时，添加选项如下：
```bash
 -v /tmp/.X11-unix:/tmp/.X11-unix \   #共享本地unix端口
 -e DISPLAY=unix$DISPLAY \           #修改环境变量DISPLAY
 -e GDK_SCALE \                      #我觉得这两个是与显示效果相关的环境变量，没有细究
 -e GDK_DPI_SCALE \
```