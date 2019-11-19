# <center>ubuntu系统18.04 LTS</center>

## 1. 广东地区速度较快的源

网易（广东广州电信/联通千兆双线接入）
```bash
http://mirrors.163.com/ubuntu/
```

## 2. 重置root密码

```bash
sudo passwd root
```

## 3. 允许root用户登录

进入配置文件中修改PermitRootLogin后的默认值为yes
```bash
sudo vim /etc/ssh/sshd_config
sudo service ssh restart
```