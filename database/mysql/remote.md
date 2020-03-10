# <center>配置mysql允许远程连接</center>

## 1. 修改mysql配置文件
```bash
vim /etc/mysql/mysql.conf.d/mysqld.cnf #注意不同的mysql版本，配置文件位置不同
```
找到bind-address = 127.0.0.1这一行

改为bind-address = 0.0.0.0即可

## 2. 为需要远程登录的用户赋予权限

```bash
mysql -uroot -p #连接数据库
use mysql; #切换到mysql数据库
SELECT `Host`,`User` FROM user; #查看用户表
UPDATE user SET `Host` = '%' WHERE `User` = 'root' LIMIT 1; #更新root用户权限
flush privileges; #强制刷新权限
exit #退出mysql
```

## 3. 关闭防火墙
```bash
sudo ufw disable #Ubuntu系统关闭防火墙
sudo ufw status #查看防火墙状态
```

如果上述步骤不行，重启电脑后再试