# LegionFanEmbeddedController

# 项目简介
<div>
    <img src="assets\example.gif">
</div>
本项目通过直接修改EC内存进行笔记本风扇控制 https://github.com/GermanAizek/WinRing0/

本项目在Legion 7 16ACHg6 2021/r9000k 上测试过

理论上支持 2020/2021年的拯救者机型

其他机型在尝试前，请有恢复笔记本BIOS的能力再做尝试

切换性能模式(FN+Q)后风扇曲线会失效

# 使用方式
需要右键管理员权限运行

推荐为常用命令创建快捷方式使用

风扇曲线配置在config目录下，设置风扇曲线参考下方的命令
```
# 读取风扇状态
fan_ctrl.exe
########################### Read Mode ###########################
FAN1 speed      1551                         FAN2 speed      1560

FAN1 curve         0,  15,  15,  15,  20,  20,  20,  20,  32,  32
FAN2 curve         0,  14,  15,  15,  20,  20,  20,  20,  32,  32
ACC  time          2,   2,   2,   2,   2,   2,   2,   2,   2,   2
DEC  time          2,   2,   2,   2,   2,   2,   2,   2,   2,   2
CPU  lower temp    0,  45,  51,  55,  59,  68,  72,  78,  87,   0
CPU  upper temp   48,  54,  58,  62,  71,  76,  81,  90, 127,   0
GPU  lower temp    0,  56,  56,  56,  56,  56,  61,  66,  72,   0
GPU  upper temp   60,  60,  60,  60,  60,  65,  69,  77, 127,   0
IC   lower temp    0,  39,  42,  46, 127, 127, 127, 127, 127,   0
IC   upper temp   41,  44,  50, 127, 127, 127, 127, 127, 127,   0

EC Firmware Ver  64                      EC Chip model    8227 v2

# 写入配置不关闭命令行窗口
fan_ctrl.exe write .\config\silent1.json 1

# 写入配置后关闭命令行窗口
fan_ctrl.exe write .\config\silent1.json 0
```

风扇曲线配置解释
```
{
	"FAN1_curve":     [   0,  15,  15,  15,  15,  15,  15,  15,  32,  32],
	"FAN2_curve":     [   0,  14,  15,  15,  15,  15,  15,  15,  32,  32],
	"ACC_time":       [   2,   2,   2,   2,   2,   2,   2,   2,   2,   2],
	"DEC_time":       [   2,   2,   2,   2,   2,   2,   2,   2,   2,   2],
	"CPU_lower_temp": [   0,  45,  51,  55,  59,  68,  72,  78,  87,   0],
	"CPU_upper_temp": [  48,  54,  58,  62,  71,  76,  81,  90, 127,   0],
	"GPU_lower_temp": [   0,  56,  56,  56,  56,  56,  61,  66,  72,   0],
	"GPU_upper_temp": [  60,  60,  60,  60,  60,  65,  69,  77, 127,   0],
	"IC_lower_temp":  [   0,  39,  42,  46, 127, 127, 127, 127, 127,   0],
	"IC_upper_temp":  [  41,  44,  50, 127, 127, 127, 127, 127, 127,   0]
}
```
- FAN1_curve 和 FAN2_curve 代表两个风扇的曲线，风扇曲线有十个挡位
- 设置的值代表乘以100后的值，上面配置中FAN1_curve第二档为15，风扇转速则为1500转
- 每个风扇挡位所在的列有对应的温度

# 感谢项目
https://github.com/johnfanv2/LenovoLegionLinux

https://github.com/SmokelessCPUv2/Lagon-Fan-EC-Control

https://github.com/GermanAizek/WinRing0/

# 捐赠

<div>
    <img src="assets\b.png">
</div>