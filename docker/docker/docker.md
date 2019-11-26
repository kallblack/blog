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

## 3. 安装docker-compose
 
进入https://github.com/docker/compose/releases 查看最新版本，当前版本为1.25.0

```bash
#1.安装
curl -L https://github.com/docker/compose/releases/download/1.25.0/docker-compose-`uname -s`-`uname -m` -o /usr/local/bin/docker-compose
#2.增加执行权限
chmod +x /usr/local/bin/docker-compose
#4.查看是否安装成功
docker-compose --version
```

由于github下载速度很慢，我尝试了多次都没有下载成功，所以我使用单独的下载工具将docker-compose-Linux-x86_64下载下来
```bash
#1.将docker-compose-Linux-x86_64拷贝到/usr/local/bin/目录下，并更名为docker-compose
cd /usr/local/bin/ && mv docker-compose-Linux-x86_64 docker-compose
#2.增加执行权限
chmod +x /usr/local/bin/docker-compose
#4.查看是否安装成功
docker-compose --version
```

