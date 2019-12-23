# <center>Ubuntu19.04制作本地源</center>

因工作需要，要在不能上网的Ubuntu 19.04机器上安装Nginx，而如果以源码的方式离线安装Nginx，又需要先离线安装gcc，离线安装gcc的依赖包太多，离线安装起来很麻烦

所以这里采用本地源的方式在线安装Nginx

首先，要准备一台能上网的Ubuntu机器，系统版本和不能上网的Ubuntu一样，都是19.04版本

## 1. 在能上网的Ubuntu上制作本地源

先在线下载Nginx安装包及依赖包
```bash
sudo rm -fr /var/cache/apt/archives/* #先请空旧的安装包
sudo apt-get install nginx
```
执行完命令后，可以看到/var/cache/apt/archives文件夹里下载了Nginx相关的包

将下载的包复制到/var/debs目录下
```bash
sudo mkdir /var/debs
sudo cp -r /var/cache/apt/archives/*.deb /var/debs/
```

进入/var/debs目录生成索引及Release文件
```bash
cd /var/debs
sudo apt-ftparchive packages . > Packages
sudo apt-ftparchive release . > Release
```

制作签名
```bash
cd /var/debs
su root #这里只能用root用户，sudo也会报没有权限
gpg --gen-key #执行gpg会进入一些对话，其中要新建一个用户名username和相应的密码
# 结束之后，输入命令，可以查看key
gpg --list-key
# 导出公钥，pub为gpg --list-key命令显示的pub值，是一串编码，例如：
# gpg -a --export 3965436CE14C8209EA2ACA6B1E4EC1F9B0DCF03A > nginx.pub
gpg -a --export <pub> > nginx.pub
#创建InRelease和Release.gpg文件
gpg --clearsign -o InRelease Release
gpg -abs -o Release.gpg Release
```
以上步骤执行完后，/var/debs文件夹里内容如下图所示：
![image][nginx-Base64]


## 2.在不能上网的Ubuntu上通过本地源安装软件
将第1步制作的文件夹里的所有文件复制到/var/debs里，然后修改源地址
```bash
#由于该Ubuntu不能上网，原来的源没没有什么用处，直接懒得备份，直接改掉
echo "deb file:///var/debs/ /"| sudo tee /etc/apt/sources.list 
```
接下来导入公钥，并安装Nginx
```bash
cd /var/debs
sudo apt-key add nginx.pub
sudo apt-get update
sudo apt-get install nginx
```






