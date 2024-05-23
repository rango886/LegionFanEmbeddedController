using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.IO;
using System.Text.Json;
using LibreHardwareMonitor.Hardware;

namespace fan_ctrl
{
    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }
    public class FanConfig
    {
        public int[] FAN1_curve { get; set; }
        public int[] FAN2_curve { get; set; }
        public int[] ACC_time { get; set; }
        public int[] DEC_time { get; set; }
        public int[] CPU_lower_temp { get; set; }
        public int[] CPU_upper_temp { get; set; }
        public int[] GPU_lower_temp { get; set; }
        public int[] GPU_upper_temp { get; set; }
        public int[] IC_lower_temp { get; set; }
        public int[] IC_upper_temp { get; set; }


    }
    internal class Program
    {
        static String ReadHW()
        {
            String hwStatus = "";
            Computer computer = new Computer
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true,
                IsMotherboardEnabled = true,
                IsControllerEnabled = true,
                IsNetworkEnabled = true,
                IsStorageEnabled = true
            };

            computer.Open();
            computer.Accept(new UpdateVisitor());

            foreach (IHardware hardware in computer.Hardware)
            {
                //Console.WriteLine("Hardware: {0}", hardware.Name);

                //foreach (IHardware subhardware in hardware.SubHardware)
                //{
                //    Console.WriteLine("\tSubhardware: {0}", subhardware.Name);

                //    foreach (ISensor sensor in subhardware.Sensors)
                //    {
                //        Console.WriteLine("\t\tSensor: {0}, value: {1}", sensor.Name, sensor.Value);
                //    }
                //}

                foreach (ISensor sensor in hardware.Sensors)
                {
                    if (sensor.Name == "Core (Tctl/Tdie)" || sensor.Name == "GPU Hot Spot" || sensor.Name == "Temperature")
                    {
                        hwStatus = hwStatus + string.Format("{0,-63}", hardware.Name) + string.Format("{0:0}\n", sensor.Value);
                    }
                    
                }
            }

            computer.Close();
            return hwStatus;
        }
        static byte[] ConvertIntArrayToByteArray(int[] intArray)
        {
            byte[] byteArray = new byte[intArray.Length];
            for (int i = 0; i < intArray.Length; i++)
            {
                byteArray[i] = (byte)intArray[i];
            }
            return byteArray;
        }
        static Boolean WriteConfig(string configPath)
        {
            Console.WriteLine("########################### Write Mode ###########################");
            //string Chip_ID = EC.DirectECRead((ushort)EC.ITE_REGISTER_MAP.ECHIPID1).ToString("X2") + EC.DirectECRead((ushort)EC.ITE_REGISTER_MAP.ECHIPID2).ToString("X2") + " v" + EC.DirectECRead((ushort)EC.ITE_REGISTER_MAP.ECHIPVER).ToString("X1");
            //if (Chip_ID != "8227 v2")
            //{
            //    Console.WriteLine("not support platform");
            //}
            //string fileName = "fanConfig.json";
            string jsonString = File.ReadAllText(configPath);
            FanConfig fanConfig = JsonSerializer.Deserialize<FanConfig>(jsonString);
            //Console.WriteLine(ConvertIntArrayToByteArray(fanConfig.FAN1_curve));
            //Console.WriteLine(string.Join(", ", fanConfig.FAN1_curve));

            //Console.WriteLine(string.Join(", ", fanConfig.FAN2_curve));
            //Console.WriteLine(string.Join(", ", fanConfig.ACC_time.Select(b => b.ToString().PadLeft(3))));
            //Console.WriteLine(string.Join(", ", fanConfig.DEC_time.Select(b => b.ToString().PadLeft(3))));
            //Console.WriteLine(string.Join(", ", fanConfig.CPU_lower_temp.Select(b => b.ToString().PadLeft(3))));
            //Console.WriteLine(string.Join(", ", fanConfig.CPU_upper_temp.Select(b => b.ToString().PadLeft(3))));
            //Console.WriteLine(string.Join(", ", fanConfig.GPU_lower_temp.Select(b => b.ToString().PadLeft(3))));
            //Console.WriteLine(string.Join(", ", fanConfig.GPU_upper_temp.Select(b => b.ToString().PadLeft(3))));
            //Console.WriteLine(string.Join(", ", fanConfig.IC_lower_temp.Select(b => b.ToString().PadLeft(3))));
            //Console.WriteLine(string.Join(", ", fanConfig.IC_upper_temp.Select(b => b.ToString().PadLeft(3))));

            //FAN1_curve
            EC.DirectECWriteArray((ushort)EC.ITE_REGISTER_MAP.FAN1_BASE, ConvertIntArrayToByteArray(fanConfig.FAN1_curve));
            byte FAN1_curve_Target = EC.DirectECRead(0xC5FC);
            EC.DirectECWrite(0xC5FC - 0x18, FAN1_curve_Target);
            EC.DirectECWrite((ushort)EC.ITE_REGISTER_MAP.DCR5, (byte)(FAN1_curve_Target * 255 / 45));

            //FAN2_curve
            EC.DirectECWriteArray((ushort)EC.ITE_REGISTER_MAP.FAN2_BASE, ConvertIntArrayToByteArray(fanConfig.FAN2_curve));
            byte FAN2_curve_Target = EC.DirectECRead(0xC5FD);
            EC.DirectECWrite(0xC5FD - 0x18, FAN2_curve_Target);
            EC.DirectECWrite((ushort)EC.ITE_REGISTER_MAP.DCR4, (byte)(FAN2_curve_Target * 255 / 45));

            
            EC.DirectECWriteArray((ushort)EC.ITE_REGISTER_MAP.CPU_TEMP, ConvertIntArrayToByteArray(fanConfig.CPU_upper_temp));
            EC.DirectECWriteArray((ushort)EC.ITE_REGISTER_MAP.GPU_TEMP, ConvertIntArrayToByteArray(fanConfig.GPU_upper_temp));
            EC.DirectECWriteArray((ushort)EC.ITE_REGISTER_MAP.VRM_TEMP, ConvertIntArrayToByteArray(fanConfig.IC_upper_temp));
            EC.DirectECWriteArray((ushort)EC.ITE_REGISTER_MAP.CPU_TEMP_HYST, ConvertIntArrayToByteArray(fanConfig.CPU_lower_temp));
            EC.DirectECWriteArray((ushort)EC.ITE_REGISTER_MAP.GPU_TEMP_HYST, ConvertIntArrayToByteArray(fanConfig.GPU_lower_temp));
            EC.DirectECWriteArray((ushort)EC.ITE_REGISTER_MAP.VRM_TEMP_HYST, ConvertIntArrayToByteArray(fanConfig.IC_lower_temp));

            // ACC_time
            byte ACC_time_Target = EC.DirectECRead((ushort)EC.ITE_REGISTER_MAP.FAN_CUR_POINT);
            EC.DirectECWrite((ushort)EC.ITE_REGISTER_MAP.FAN1_CUR_ACC, ConvertIntArrayToByteArray(fanConfig.ACC_time)[ACC_time_Target]);
            EC.DirectECWrite((ushort)EC.ITE_REGISTER_MAP.FAN2_CUR_ACC, ConvertIntArrayToByteArray(fanConfig.ACC_time)[ACC_time_Target]);
            EC.DirectECWriteArray((ushort)EC.ITE_REGISTER_MAP.FAN_ACC_BASE, ConvertIntArrayToByteArray(fanConfig.ACC_time));

            // DEC_time
            byte DEC_time_Target = EC.DirectECRead((ushort)EC.ITE_REGISTER_MAP.FAN_CUR_POINT);
            EC.DirectECWrite((ushort)EC.ITE_REGISTER_MAP.FAN1_CUR_DEC, ConvertIntArrayToByteArray(fanConfig.DEC_time)[DEC_time_Target]);
            EC.DirectECWrite((ushort)EC.ITE_REGISTER_MAP.FAN2_CUR_DEC, ConvertIntArrayToByteArray(fanConfig.DEC_time)[DEC_time_Target]);
            EC.DirectECWriteArray((ushort)EC.ITE_REGISTER_MAP.FAN_DEC_BASE, ConvertIntArrayToByteArray(fanConfig.DEC_time));


            //Thread.Sleep(500);
            
            return true;
        }
        static void DoReadStatus()
        {
            while (true)
            {
                String hwStatus = ReadHW();
                
                Console.Clear();
                Console.WriteLine("########################### Read Mode ###########################");
                Console.WriteLine(hwStatus);
                ReadStatus();
           
                Thread.Sleep(2000);
            }
        }
        static void ReadStatus()
        {
            
            if (WinRing.WinRingInitOk)
            {
                byte Low = EC.DirectECRead((ushort)EC.ITE_REGISTER_MAP.FAN1_RPM_LSB);
                byte High = EC.DirectECRead((ushort)EC.ITE_REGISTER_MAP.FAN1_RPM_MSB);
                int lFanSpeed = (ushort)(High << 8 | Low);

                Low = EC.DirectECRead((ushort)EC.ITE_REGISTER_MAP.FAN2_RPM_LSB);
                High = EC.DirectECRead((ushort)EC.ITE_REGISTER_MAP.FAN2_RPM_MSB);
                int rFanSpeed = (ushort)(High << 8 | Low);

                byte[] FAN1_list = EC.DirectECReadArray((ushort)EC.ITE_REGISTER_MAP.FAN1_BASE, 10);

                string FAN1_str = string.Join(", ", EC.DirectECReadArray((ushort)EC.ITE_REGISTER_MAP.FAN1_BASE, 10).Select(b => b.ToString().PadLeft(3)));

                string FAN2_str = string.Join(", ", EC.DirectECReadArray((ushort)EC.ITE_REGISTER_MAP.FAN2_BASE, 10).Select(b => b.ToString().PadLeft(3)));
                string FAN_ACC_str = string.Join(", ", EC.DirectECReadArray((ushort)EC.ITE_REGISTER_MAP.FAN_ACC_BASE, 10).Select(b => b.ToString().PadLeft(3)));
                string FAN_DEC_str = string.Join(", ", EC.DirectECReadArray((ushort)EC.ITE_REGISTER_MAP.FAN_DEC_BASE, 10).Select(b => b.ToString().PadLeft(3)));
                string CPU_TEMP_str = string.Join(", ", EC.DirectECReadArray((ushort)EC.ITE_REGISTER_MAP.CPU_TEMP, 10).Select(b => b.ToString().PadLeft(3)));
                string GPU_TEMP_str = string.Join(", ", EC.DirectECReadArray((ushort)EC.ITE_REGISTER_MAP.GPU_TEMP, 10).Select(b => b.ToString().PadLeft(3)));
                string VRM_TEMP_str = string.Join(", ", EC.DirectECReadArray((ushort)EC.ITE_REGISTER_MAP.VRM_TEMP, 10).Select(b => b.ToString().PadLeft(3)));
                string CPU_TEMP_HYST_str = string.Join(", ", EC.DirectECReadArray((ushort)EC.ITE_REGISTER_MAP.CPU_TEMP_HYST, 10).Select(b => b.ToString().PadLeft(3)));
                string GPU_TEMP_HYST_str = string.Join(", ", EC.DirectECReadArray((ushort)EC.ITE_REGISTER_MAP.GPU_TEMP_HYST, 10).Select(b => b.ToString().PadLeft(3)));
                string VRM_TEMP_HYST_str = string.Join(", ", EC.DirectECReadArray((ushort)EC.ITE_REGISTER_MAP.VRM_TEMP_HYST, 10).Select(b => b.ToString().PadLeft(3)));

                string Chip_ID = EC.DirectECRead((ushort)EC.ITE_REGISTER_MAP.ECHIPID1).ToString("X2") + EC.DirectECRead((ushort)EC.ITE_REGISTER_MAP.ECHIPID2).ToString("X2") + " v" + EC.DirectECRead((ushort)EC.ITE_REGISTER_MAP.ECHIPVER).ToString("X1");
                string EC_FW_Ver = "" + EC.DirectECRead((ushort)EC.ITE_REGISTER_MAP.FW_VER);

                
                Console.Write("FAN1 speed      " + lFanSpeed);
                Console.Write("                         ");
                Console.WriteLine("FAN2 speed      " + rFanSpeed);
                Console.WriteLine("");
                Console.WriteLine("FAN1 curve       " + FAN1_str);
                Console.WriteLine("FAN2 curve       " + FAN2_str);
                Console.WriteLine("ACC  time        " + FAN_ACC_str);
                Console.WriteLine("DEC  time        " + FAN_DEC_str);
                Console.WriteLine("CPU  lower temp  " + CPU_TEMP_HYST_str);
                Console.WriteLine("CPU  upper temp  " + CPU_TEMP_str);
                Console.WriteLine("GPU  lower temp  " + GPU_TEMP_HYST_str);
                Console.WriteLine("GPU  upper temp  " + GPU_TEMP_str);
                Console.WriteLine("IC   lower temp  " + VRM_TEMP_HYST_str);
                Console.WriteLine("IC   upper temp  " + VRM_TEMP_str);

                Console.WriteLine("");
                Console.Write("EC Firmware Ver  " + EC_FW_Ver);
                Console.Write("                      ");
                Console.WriteLine("EC Chip model    " + Chip_ID);
                Console.Write("                      ");
                

            }
        }
        static void Main(string[] args)
        {
            //Initialize WinRing
            var success = WinRing.InitializeOls();

            if (!success)
            {
                WinRing.WinRingInitOk = false;
            }
            else
                WinRing.WinRingInitOk = true;

            if (WinRing.WinRingInitOk)
            {
                if (EC.DirectECRead((ushort)EC.ITE_REGISTER_MAP.ECHIPID1) != 0x82 || EC.DirectECRead((ushort)EC.ITE_REGISTER_MAP.ECHIPID2) != 0x27)

                {
                    WinRing.WinRingInitOk = false;
                }

            }
            if (args.Length == 0)
            {
                DoReadStatus();
            }
            else
            {
                if (args[0] == "write")
                {
                    Console.Write("Current config file "); 
                    Console.WriteLine(args[1]);
                    Boolean res = WriteConfig(args[1]);
                    //Boolean res = true;
                    if (res)
                    {
                        ReadStatus();
                        Console.WriteLine(" ");
                        
                        Console.WriteLine("write configuration succeeded"); 
                    }
                    else 
                    {
                        Console.WriteLine("write configuration failed");
                    }
                    if (args[2] == "1")
                    {
                        Console.ReadKey();
                    }
                    
                }
                else
                {
                    DoReadStatus();
                }

            }


        }
    }
}
