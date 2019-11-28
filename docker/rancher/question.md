# <center>Rancher使用过程中遇到的问题</center>

## 1. 在建立集群的时候，calico服务报如下错误：
```bash
2019-11-27 02:20:04.711 [FATAL][2951] int_dataplane.go 980: Kernel's RPF check is set to 'loose'. This would allow endpoints to spoof their IP address. Calico requires net.ipv4.conf.all.rp_filter to be set to 0 or 1. If you require loose RPF and you are not concerned about spoofing, this check can be disabled by setting the IgnoreLooseRPF configuration parameter to 'true'.
2019/11/27 上午10:20:05 2019-11-27 02:20:05.850 [WARNING][2970] int_dataplane.go 354: Failed to query VXLAN device error=Link not found
2019/11/27 上午10:20:05 2019-11-27 02:20:05.913 [WARNING][2970] int_dataplane.go 384: Failed to cleanup preexisting XDP state error=failed to load XDP program (/tmp/felix-xdp-599806692): stat /sys/fs/bpf/calico/xdp/prefilter_v1_calico_tmp_A: no such file or directory
2019/11/27 上午10:20:05 libbpf: failed to get EHDR from /tmp/felix-xdp-599806692
2019/11/27 上午10:20:05 Error: failed to open object file
```

这时我们需要对net.ipv4.conf.default.rp_filter设置值
```bash
vim /etc/sysctl.conf
#将这行代码的注释去掉
#net.ipv4.conf.default.rp_filter=1
#改为
net.ipv4.conf.default.rp_filter=1

#然后使修改生效
sysctl -p
```


## 2. 在建立集群的时候，coredns报如下错误：
```bash
plugin/loop: Loop (127.0.0.1:55953 -> :53) detected for zone ".", see https://coredns.io/plugins/loop#troubleshooting. Query: "HINFO 4547991504243258144.3688648895315093531."
```

Ubuntu 18.04 中默认开启了 systemd-resolved，它会在系统的 /etc/resolv.conf 中写入 nameserver 127.0.0.53。由于这是一个本地地址，从而会导致 CoreDNS 或者 kube-dns 无法解析外网地址。

解决方法是替换掉 systemd-resolved 生成的 resolv.conf 文件：
```bash
#修改DNS
vim /run/systemd/resolve/resolv.conf
#将DNS改为非127.0.0.53
nameserver 192.168.100.2

#建立软链接
sudo rm /etc/resolv.conf
sudo ln -s /run/systemd/resolve/resolv.conf /etc/resolv.conf
sudo systemctl enable systemd-resolved
sudo reboot #重启电脑
```