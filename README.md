# LegionFanEmbeddedController
[中文介绍](README_ZH.md)
# Introduction
This project controls legion laptop fans by directly modifying the EC memory.

The project has been tested on Legion 7 16ACHg6 2021/r9000k.

Theoretically supports the 2020/2021 models of the Legion series.

Before attempting on other models, make sure you have the ability to restore the laptop BIOS.

The fan curve will be invalidated after switching performance mode (FN+Q).

# How to Use
Need to run as administrator by right-clicking

It is recommended to create shortcuts for frequently used commands.

The fan curve configuration is located in the config directory. Refer to the commands below to set the fan curve:
```
# Read fan status
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

# Write configuration without closing the command prompt window
fan_ctrl.exe write .\config\silent1.json 1

# Write configuration and close the command prompt window afterwards
fan_ctrl.exe write .\config\silent1.json 0
```

Explanation of the fan curve configuration:
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

- FAN1_curve and FAN2_curve represent the curves of two fans, with ten speed levels.
- The values set represent the speed multiplied by 100. For example, in the configuration provided, a value of 15 in the second level of FAN1_curve corresponds to a fan speed of 1500 RPM.
- Each column in the fan curve corresponds to a specific temperature range.

# Special thanks
https://github.com/johnfanv2/LenovoLegionLinux

https://github.com/SmokelessCPUv2/Lagon-Fan-EC-Control

# Donation
![](assets\donation.png)