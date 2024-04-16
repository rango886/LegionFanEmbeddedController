# Problem Description
When the load and temperature of the NVIDIA GPU increase, the fan speed does not behave as expected.

# Environment
Distribution: Ubuntu 23.10

Model name: Legion 7 16ACHg6 2021 / R9000k

CPU model: AMD Ryzen 9 5900HX

GPU model: NVIDIA RTX 3080

CPU stress test: sudo apt install sysbench

GPU stress test: [https://www.geeks3d.com/gputest/](https://www.geeks3d.com/gputest/)

# Problem Reproduction

The fan configuration is as follows:

![image](assets\1.png)

According to the readings from Psensor, at the current temperature, the fan1 and fan2 curves are normal.

## CPU Stress Test
```
sudo apt install sysbench
sysbench --threads=16 cpu run
```
![image](assets\2.png)

When the CPU temperature increases, the fan1 and fan2 curves remain normal, with fan1 at 1200 rpm and fan2 at 1100 rpm.

## AMD Integrated GPU Stress Test
```
Download from here: https://www.geeks3d.com/gputest/
./start_furmark_windowed_1024x640.sh
```

![image](assets\3.png)

When the temperature of the AMD GPU increases, the fan1 and fan2 curves remain normal, with fan1 at 1200 rpm and fan2 at 1100 rpm.

## NVIDIA GPU Stress Test
```
# Switch to the NVIDIA GPU and restart
sudo prime-select nvidia
./start_furmark_windowed_1024x640.sh
```

![image](assets\4.png)

When the temperature of the NVIDIA GPU increases, neither the fan1 nor fan2 curves behave normally, with both running at 2300 rpm.

# Speculation
I have tried:
1. Enabling and disabling the mini curve
2. Switching between different BIOS versions (GKCN64WW, GKCN60WW, GKCN59WW, GKCN58WW, GKCN57WW)
3. Dedicated GPU mode and hybrid mode

None of these have resolved the issue. My speculation is that there might be three fan curves in the EC, corresponding to fan1, fan2, and potentially a fan3 for the NVIDIA GPU. If only the fan1 curve (CPU) and fan2 curve (AMD GPU) are set, without a corresponding fan3 curve (NVIDIA GPU), the fan speed may not behave as expected when the load and temperature of the NVIDIA GPU increase.

I am unsure how to verify the existence of an NVIDIA GPU fan curve. 