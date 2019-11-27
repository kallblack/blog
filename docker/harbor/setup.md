# <center>Harbor离线安装</center>

## 1. 准备工作
先要安装docker-compose，安装方法请查看https://kallblack.github.io/blog/docker/docker/docker 的第3小节

然后下载Harbor离线安装包，查看https://github.com/goharbor/harbor/releases，找到最新稳定版本，我这里是1.9.3版本

下载1.9.3版本的offline安装包harbor-offline-installer-v1.9.3.tgz，由于从github上下载比较慢，我这里使用独立的下载工具下载完后拷贝到服务器上

```bash
#解压安装包
tar xvf harbor-offline-installer-v1.9.3.tgz
```

## 2. 修改配置文件
```bash
vim harbor.yml
#我主要修改了如下参数：
===================================================================

# The initial password of Harbor admin
# It only works in first time to install harbor
# Remember Change the admin password from UI after launching Harbor.
harbor_admin_password: yun

# Harbor DB configuration
database:
  # The password for the root user of Harbor DB. Change this before any production use.
  password: yun

# The default data volume
data_volume: /data/harbor

# The initial password of Harbor admin
# It only works in first time to install harbor
# Remember Change the admin password from UI after launching Harbor.
harbor_admin_password: yun

# Harbor DB configuration
database:
  # The password for the root user of Harbor DB. Change this before any production use.
  password: yun

# The default data volume
data_volume: /data/harbor

# Log configurations
log:
  # options are debug, info, warning, error, fatal
  level: info
  # configs for logs in local storage
  local:
    # Log files are rotated log_rotate_count times before being removed. If count is 0, old versions are removed rather than rotated.
    rotate_count: 50
    # Log files are rotated only if they grow bigger than log_rotate_size bytes. If size is followed by k, the size is assumed to be in kilobytes.
    # If the M is used, the size is in megabytes, and if G is used, the size is in gigabytes. So size 100, size 100k, size 100M and size 100G
    # are all valid.
    rotate_size: 200M
    # The directory on your host that store log
    location: /var/log/harbor

# Global proxy
# Config http proxy for components, e.g. http://my.proxy.com:3128
# Components doesn't need to connect to each others via http proxy.
# Remove component from `components` array if want disable proxy
# for it. If you want use proxy for replication, MUST enable proxy
# for core and jobservice, and set `http_proxy` and `https_proxy`.
# Add domain to the `no_proxy` field, when you want disable proxy
# for some special registry.
proxy:
  http_proxy: http://192.168.65.199:808
  https_proxy: http://192.168.65.199:808
  no_proxy: 127.0.0.1,localhost,.local,.internal,log,db,redis,nginx,core,portal,postgresql,jobservice,registry,registryctl,clair,192.168.0.1/16
  components:
    - core
    - jobservice
    - clair
```

## 3. 安装harbor
```bash
#执行安装脚本
./install.sh

#出现如下所示表示安装成功
----Harbor has been installed and started successfully.----
```

