using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace fan_ctrl
{
    internal static class WinRing
    {
        //DLL
        public static bool WinRingInitOk = false;
        [DllImport("WinRing0x64.dll")]
        public static extern DLL_Error_Code GetDllStatus();
        [DllImport("WinRing0x64.dll")]
        public static extern UInt64 GetDllVersion(ref byte major, ref byte minor, ref byte revision, ref byte release);
        [DllImport("WinRing0x64.dll")]
        public static extern UInt64 GetDriverVersion(ref byte major, ref byte minor, ref byte revision, ref byte release);
        [DllImport("WinRing0x64.dll")]
        public static extern DriverType GetDriverType();
        [DllImport("WinRing0x64.dll")]
        public static extern bool InitializeOls();
        [DllImport("WinRing0x64.dll")]
        public static extern void DeinitializeOls();

        [DllImport("WinRing0x64.dll")]
        public static extern byte ReadIoPortByte(UInt32 address);

        [DllImport("WinRing0x64.dll")]
        public static extern void WriteIoPortByte(UInt32 port, byte value);

    }


    internal static class EC
    {
        //=======================================EC Direct Access interface=================================
        //Port Config:
        //  BADRSEL(0x200A) bit1-0  Addr    Data
        //                  00      2Eh     2Fh
        //                  01      4Eh     4Fh
        //
        //              01      4Eh     4Fh
        //  ITE-EC Ram Read/Write Algorithm:
        //  Addr    w   0x2E
        //  Data    w   0x11
        //  Addr    w   0x2F
        //  Data    w   high byte
        //  Addr    w   0x2E
        //  Data    w   0x10
        //  Addr    w   0x2F
        //  Data    w   low byte
        //  Addr    w   0x2E
        //  Data    w   0x12
        //  Addr    w   0x2F
        //  Data    rw  value

        public static void DirectECWrite(UInt16 Addr, byte data)
        {
            byte EC_ADDR_PORT = (byte)EC.ITE_PORT.EC_ADDR_PORT;
            byte EC_DATA_PORT = (byte)EC.ITE_PORT.EC_DATA_PORT;
            WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2E);
            WinRing.WriteIoPortByte(EC_DATA_PORT, 0x11);
            WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2F);
            WinRing.WriteIoPortByte(EC_DATA_PORT, (byte)((Addr >> 8) & 0xFF));

            WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2E);
            WinRing.WriteIoPortByte(EC_DATA_PORT, 0x10);
            WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2F);
            WinRing.WriteIoPortByte(EC_DATA_PORT, (byte)(Addr & 0xFF));

            WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2E);
            WinRing.WriteIoPortByte(EC_DATA_PORT, 0x12);
            WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2F);
            WinRing.WriteIoPortByte(EC_DATA_PORT, data);
        }

        public static void DirectECWriteArray(UInt16 Addr_base, byte[] data)
        {
            byte EC_ADDR_PORT = (byte)EC.ITE_PORT.EC_ADDR_PORT;
            byte EC_DATA_PORT = (byte)EC.ITE_PORT.EC_DATA_PORT;
            for (var i = 0; i < data.Count(); i++)
            {
                var Addr = (ushort)(Addr_base + i);
                WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2E);
                WinRing.WriteIoPortByte(EC_DATA_PORT, 0x11);
                WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2F);
                WinRing.WriteIoPortByte(EC_DATA_PORT, (byte)((Addr >> 8) & 0xFF));

                WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2E);
                WinRing.WriteIoPortByte(EC_DATA_PORT, 0x10);
                WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2F);
                WinRing.WriteIoPortByte(EC_DATA_PORT, (byte)(Addr & 0xFF));

                WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2E);
                WinRing.WriteIoPortByte(EC_DATA_PORT, 0x12);
                WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2F);
                WinRing.WriteIoPortByte(EC_DATA_PORT, data[i]);
            }
        }

        public static byte DirectECRead(UInt16 Addr)
        {
            byte EC_ADDR_PORT = (byte)EC.ITE_PORT.EC_ADDR_PORT;
            byte EC_DATA_PORT = (byte)EC.ITE_PORT.EC_DATA_PORT;
            WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2E);
            WinRing.WriteIoPortByte(EC_DATA_PORT, 0x11);
            WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2F);
            WinRing.WriteIoPortByte(EC_DATA_PORT, (byte)((Addr >> 8) & 0xFF));

            WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2E);
            WinRing.WriteIoPortByte(EC_DATA_PORT, 0x10);
            WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2F);
            WinRing.WriteIoPortByte(EC_DATA_PORT, (byte)(Addr & 0xFF));

            WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2E);
            WinRing.WriteIoPortByte(EC_DATA_PORT, 0x12);
            WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2F);
            return WinRing.ReadIoPortByte(EC_DATA_PORT);
        }

        public static byte[] DirectECReadArray(UInt16 Addr_base, int size)
        {
            byte EC_ADDR_PORT = (byte)EC.ITE_PORT.EC_ADDR_PORT;
            byte EC_DATA_PORT = (byte)EC.ITE_PORT.EC_DATA_PORT;
            var buffer = new byte[size];
            for (var i = 0; i < size; i++)
            {
                var Addr = (ushort)(Addr_base + i);
                WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2E);
                WinRing.WriteIoPortByte(EC_DATA_PORT, 0x11);
                WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2F);
                WinRing.WriteIoPortByte(EC_DATA_PORT, (byte)((Addr >> 8) & 0xFF));

                WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2E);
                WinRing.WriteIoPortByte(EC_DATA_PORT, 0x10);
                WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2F);
                WinRing.WriteIoPortByte(EC_DATA_PORT, (byte)(Addr & 0xFF));

                WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2E);
                WinRing.WriteIoPortByte(EC_DATA_PORT, 0x12);
                WinRing.WriteIoPortByte(EC_ADDR_PORT, 0x2F);
                buffer[i] = WinRing.ReadIoPortByte(EC_DATA_PORT);
            }
            return buffer;
        }

        public enum ITE_PORT : byte
        {
            EC_ADDR_PORT = 0x4E,
            EC_DATA_PORT = 0x4F,
        }
        public enum ITE_REGISTER_MAP : UInt16
        {
            ECINDAR0 = 0x103B,
            ECINDAR1 = 0x103C,
            ECINDAR2 = 0x103D,
            ECINDAR3 = 0x103E,
            ECINDDR = 0x103F,
            GPDRA = 0x1601,
            GPCRA0 = 0x1610,
            GPCRA1 = 0x1611,
            GPCRA2 = 0x1612,
            GPCRA3 = 0x1613,
            GPCRA4 = 0x1614,
            GPCRA5 = 0x1615,
            GPCRA6 = 0x1616,
            GPCRA7 = 0x1617,
            GPOTA = 0x1671,
            GPDMRA = 0x1661,
            DCR0 = 0x1802,
            DCR1 = 0x1803,
            DCR2 = 0x1804,
            DCR3 = 0x1805,
            DCR4 = 0x1806,
            DCR5 = 0x1807,
            DCR6 = 0x1808,
            DCR7 = 0x1809,
            CTR2 = 0x1842,
            ECHIPID1 = 0x2000,
            ECHIPID2 = 0x2001,
            ECHIPVER = 0x2002,
            ECDEBUG = 0x2003,
            EADDR = 0x2100,
            EDAT = 0x2101,
            ECNT = 0x2102,
            ESTS = 0x2103,
            FW_VER = 0xC2C7,
            FAN_CUR_POINT = 0xC534,
            FAN_POINT = 0xC535,
            FAN1_BASE = 0xC540,
            FAN2_BASE = 0xC550,
            FAN_ACC_BASE = 0xC560,
            FAN_DEC_BASE = 0xC570,
            CPU_TEMP = 0xC580,
            CPU_TEMP_HYST = 0xC590,
            GPU_TEMP = 0xC5A0,
            GPU_TEMP_HYST = 0xC5B0,
            VRM_TEMP = 0xC5C0,
            VRM_TEMP_HYST = 0xC5D0,
            CPU_TEMP_EN = 0xC631,
            GPU_TEMP_EN = 0xC632,
            VRM_TEMP_EN = 0xC633,
            FAN1_ACC_TIMER = 0xC3DA,
            FAN2_ACC_TIMER = 0xC3DB,
            FAN1_CUR_ACC = 0xC3DC,
            FAN1_CUR_DEC = 0xC3DD,
            FAN2_CUR_ACC = 0xC3DE,
            FAN2_CUR_DEC = 0xC3DF,
            FAN1_RPM_LSB = 0xC5E0,
            FAN1_RPM_MSB = 0xC5E1,
            FAN2_RPM_LSB = 0xC5E2,
            FAN2_RPM_MSB = 0xC5E3,
        }
    }

    enum DLL_Error_Code
    {
        OLS_DLL_NO_ERROR = 0,
        OLS_DLL_UNSUPPORTED_PLATFORM = 1,
        OLS_DLL_DRIVER_NOT_LOADED = 2,
        OLS_DLL_DRIVER_NOT_FOUND = 3,
        OLS_DLL_DRIVER_UNLOADED = 4,
        OLS_DLL_DRIVER_NOT_LOADED_ON_NETWORK = 5,
        OLS_DLL_UNKNOWN_ERROR = 9
    }

    enum DriverType
    {
        OLS_DRIVER_TYPE_UNKNOWN = 0,
        OLS_DRIVER_TYPE_WIN_9X = 1,
        OLS_DRIVER_TYPE_WIN_NT = 2,
        OLS_DRIVER_TYPE_WIN_NT4 = 3,
        OLS_DRIVER_TYPE_WIN_NT_X64 = 4,
        OLS_DRIVER_TYPE_WIN_NT_IA64 = 5
    }
}