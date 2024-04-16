# 问题描述
当nvidia gpu的负载和温度增加的时候，风扇转速不符合预期

# 环境介绍

Distribution: ubuntu 23.10

Model name: Legion 7 16ACHg6 2021 / R9000k

CPU model: AMD Ryzen 9 5900HX

GPU model: NVIDIA RTX 3080

CPU压力测试 sudo apt install sysbench

GPU压力测试 https://www.geeks3d.com/gputest/

# 问题复现

风扇配置如下

![image](assets\1.png)

根据psensor显示的情况，在当前温度下，fan1和fan2曲线正常


## cpu 压力测试
```
sudo apt install sysbench
sysbench --threads=16 cpu run
```
![image](assets\2.png)

当cpu的温度增加时，fan1和fan2曲线正常，fan1 1200rpm,fan2 1100 rpm

## amd 内置显卡压力测试
```
从这里下载 https://www.geeks3d.com/gputest/
./start_furmark_windowed_1024x640.sh
```

![image](assets\3.png)

当amd gpu的温度增加时，fan1和fan2曲线正常，fan1 1200rpm,fan2 1100 rpm

## nvidia 显卡压力测试
```
# 切换nvidia显卡，并重启
sudo prime-select nvidia
./start_furmark_windowed_1024x640.sh
```

![image](assets\4.png)

当nvidia gpu的温度增加时，fan1和fan2曲线都不正常，fan1 2300rpm,fan2 2300 rpm


# 推测
我试过
1. 开启和关闭关闭迷你曲线
2. 切换不同的BIOS版本(GKCN64WW,GKCN60WW,GKCN59WW,GKCN58WW,GKCN57WW)
3. 独显直连和混合模式
都没法解决这个问题。
我推测，也许在EC里面有三条曲线，对应fan1和fan2，而不是两条风扇曲线。三条风扇曲线分别对应CPU,amd GPU,nvidia GPU。

假设有这么一个nvidia gpu fan曲线，如果只设置了fan1曲线(CPU),fan2曲线(amd GPU),而没有设置fan3曲线(nvidia GPU),
当nvidia gpu的负载和温度增加的时候就会导致风扇转速不符合预期

不知道怎样才能验证是否有nvidia gpu的风扇曲线