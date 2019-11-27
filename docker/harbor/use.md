# <center>Harbor使用</center>

## 1. 在docker配置中增加Harbor地址信任
```bash
vim /lib/systemd/system/docker.service 
#在ExecStart后面增加--insecure-registry 192.168.65.227:8010
ExecStart=/usr/bin/dockerd -H fd:// --containerd=/run/containerd/containerd.sock  --registry-mirror=http://hub-mirror.c.163.com
 --insecure-registry 192.168.65.227:8010

#重启docker
systemctl  daemon-reload
systemctl restart docker
```

## 2. 制作镜像
将 mongo 制作成一个私有镜像， mongo 为我之前从 docker hub 上拉取的镜像
```bash
docker tag mongo 192.168.65.227:8010/joyoj/mongo:0.0.1
```

## 3. 上传

先登录私有库
```bash
docker login 192.168.65.227:8010
```

再推送镜像
```bash
docker push 192.168.65.227:8010/joyoj/mongo:0.0.1
```

在Harbor的UI页面上就可以看到上传的镜像了