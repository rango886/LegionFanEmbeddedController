# LegionFanEmbeddedController
[中文介绍](README_ZH.md)
# Introduction
<div>
    <img src="assets\example.gif">
</div>

This project controls legion laptop fans by directly modifying the EC memory. https://github.com/GermanAizek/WinRing0/

The project has been tested on Legion 7 16ACHg6 2021/r9000k.

Theoretically supports the 2020/2021 models of the Legion series.

The fan curve will be invalidated after switching performance mode (FN+Q).

This project still has some unresolved issues. (For example, although a lower fan speed is set, the fans lock at 2300 RPM under high load).
To address this, a BIOS downgrade to GKCN24WW is required. However, this may lead to other issues, such as:
1. Nvidia driver installation failure: Error code 31 may occur. This can be resolved by forcefully installing the driver using DISM++.
2. Fan noise: When fan speeds for fan1 and fan2 are set to a lower RPM (e.g., 1000 RPM), a buzzing noise may be audible during periods of increased GPU load.

I have not found a solution for the higher version of the BIOS at the moment.

# How to Use
Need to run as administrator by right-clicking

It is recommended to create shortcuts for frequently used commands.

The fan curve configuration is located in the config directory. Refer to the commands below to set the fan curve:
```
# Read fan status
fan_ctrl.exe


########################### Read Mode ###########################
AMD Ryzen 9 5900HX with Radeon Graphics                        67
NVIDIA GeForce RTX 3080 Laptop GPU                             58
GLOWAY YCT2TNVMe-M.2/80                                        44
SAMSUNG MZVLB1T0HBLR-000L2                                     52

FAN1 speed      1568                         FAN2 speed      1579

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

EC Firmware Ver  24                      EC Chip model    8227 v2



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

https://github.com/GermanAizek/WinRing0/

# Donation
<div>
    <img src="assets\a.png">
</div>