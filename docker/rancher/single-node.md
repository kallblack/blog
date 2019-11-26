# <center>Rancher单节点安装</center>

## 1. 准备工作

```bash
#切换到root用户
su root
#关闭防火墙
ufw disable
```

## 2. 开始安装

由于我的服务器只能通过代理上网，所以这里加入代理参数
```bash
docker run -d --restart=unless-stopped \
  -p 80:80 -p 443:443 \
  -v /usr/local/software/rancher/:/var/lib/rancher/ \
  -e HTTP_PROXY="http://192.168.65.199:808" \
  -e HTTPS_PROXY="http://192.168.65.199:808" \
  -e NO_PROXY="localhost,127.0.0.1,0.0.0.0,192.168.0.0/16" \
  -e AUDIT_LEVEL=3 \
  rancher/rancher:stable
```
