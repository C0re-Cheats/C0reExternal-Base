﻿using System.Diagnostics;
using System.Runtime.InteropServices;
using System;
using System.Text;
using System.ComponentModel;
using ExternalBase.C0re.Math;
using System.Threading;

namespace MemoryManager
{
    public class NativeWIN32
    {
        Vectoring Vector3 = new Vectoring();
        public const ushort KEYEVENTF_KEYUP = 0x0002;
        public enum VK : ushort
        {
            F5 = 0x74,
        }

        public struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public long time;
            public uint dwExtraInfo;
        };
        [StructLayout(LayoutKind.Explicit, Size = 28)]
        public struct INPUT
        {
            [FieldOffset(0)]
            public uint type;
#if x86 
    //32bit 
    [FieldOffset(4)] 
#else
            //64bit 
            [FieldOffset(8)]
#endif
            public KEYBDINPUT ki;
        };
    };
    internal class ProcM
    {
        // Fields
        protected int BaseAddress;
        protected Process[] MyProcess;
        protected ProcessModule myProcessModule;
        private const uint PAGE_EXECUTE = 16;
        private const uint PAGE_EXECUTE_READ = 32;
        private const uint PAGE_EXECUTE_READWrt = 64;
        private const uint PAGE_EXECUTE_WrtCOPY = 128;
        private const uint PAGE_GUARD = 256;
        private const uint PAGE_NOACCESS = 1;
        private const uint PAGE_NOCACHE = 512;
        private const uint PAGE_READONLY = 2;
        private const uint PAGE_READWrt = 4;
        private const uint PAGE_WrtCOPY = 8;
        private const uint PROCESS_ALL_ACCESS = 2035711;
        protected int processHandle;
        protected string ProcessName;

        // Methods
        public ProcM(string pProcessName)
        {
            this.ProcessName = pProcessName;
        }

        public bool CheckProcess()
        {
            return (Process.GetProcessesByName(this.ProcessName).Length > 0);
        }

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(int hObject);
        public string CutString(string mystring)
        {
            char[] chArray = mystring.ToCharArray();
            string str = "";
            for (int i = 0; i < mystring.Length; i++)
            {
                try
                {
                    if ((chArray[i] == ' ') && (chArray[i + 1] == ' '))
                    {
                        return str;
                    }
                    if (chArray[i] == '\0')
                    {
                        return str;
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    chArray[i] = ' ';
                }
                str = str + chArray[i].ToString();
            }
            return mystring.TrimEnd(new char[] { '0' });
        }

        public int DllImageAddress(string dllname)
        {
            try
            {
                ProcessModuleCollection modules = MyProcess[0].Modules;

                foreach (ProcessModule procmodule in modules)
                {
                    if (dllname == procmodule.ModuleName)
                    {
                        return (int)procmodule.BaseAddress;
                    }
                }
                return -1;
            }
            catch (IndexOutOfRangeException)
            {
                return -1;
            }
            catch (Win32Exception)
            {
                return -1;
            }
        }
        public int DllImageSize(string dllname)
        {
            try
            {
                ProcessModuleCollection modules = MyProcess[0].Modules;

                foreach (ProcessModule procmodule in modules)
                {
                    if (dllname == procmodule.ModuleName)
                    {
                        return procmodule.ModuleMemorySize;
                    }
                }
                return -1;
            }
            catch (IndexOutOfRangeException ex)
            {
                return -1;
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                return -1;
            }
        }
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern int FindWindowByCaption(int ZeroOnly, string lpWindowName);
        public int ImageAddress()
        {
            this.BaseAddress = 0;
            this.myProcessModule = this.MyProcess[0].MainModule;
            this.BaseAddress = (int)this.myProcessModule.BaseAddress;
            return this.BaseAddress;


        }

        public int ImageAddress(int pOffset)
        {
            this.BaseAddress = 0;
            this.myProcessModule = this.MyProcess[0].MainModule;
            this.BaseAddress = (int)this.myProcessModule.BaseAddress;
            return (pOffset + this.BaseAddress);
        }
        public string MyProcessName()
        {
            return this.ProcessName;
        }

        [DllImport("kernel32.dll")]
        public static extern int OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        public int Pointer(bool AddToImageAddress, int pOffset)
        {
            return this.rdInt(this.ImageAddress(pOffset));
        }

        public int Pointer(string Module, int pOffset)
        {
            return this.rdInt(this.DllImageAddress(Module) + pOffset);
        }

        public int Pointer(bool AddToImageAddress, int pOffset, int pOffset2)
        {
            if (AddToImageAddress)
                return (this.rdInt(this.ImageAddress() + pOffset) + pOffset2);
            else
                return (this.rdInt(pOffset) + pOffset2);
        }

        public int Pointer(string Module, int pOffset, int pOffset2)
        {
            return (this.rdInt(this.DllImageAddress(Module) + pOffset) + pOffset2);
        }

        public int Pointer(bool AddToImageAddress, int pOffset, int pOffset2, int pOffset3)
        {
            return (this.rdInt(this.rdInt(this.ImageAddress(pOffset)) + pOffset2) + pOffset3);
        }

        public int Pointer(string Module, int pOffset, int pOffset2, int pOffset3)
        {
            return (this.rdInt(this.rdInt(this.DllImageAddress(Module) + pOffset) + pOffset2) + pOffset3);
        }

        public int Pointer(bool AddToImageAddress, int pOffset, int pOffset2, int pOffset3, int pOffset4)
        {
            return (this.rdInt(this.rdInt(this.rdInt(this.ImageAddress(pOffset)) + pOffset2) + pOffset3) + pOffset4);
        }

        public int Pointer(string Module, int pOffset, int pOffset2, int pOffset3, int pOffset4)
        {
            return (this.rdInt(this.rdInt(this.rdInt(this.DllImageAddress(Module) + pOffset) + pOffset2) + pOffset3) + pOffset4);
        }

        public int Pointer(bool AddToImageAddress, int pOffset, int pOffset2, int pOffset3, int pOffset4, int pOffset5)
        {
            return (this.rdInt(this.rdInt(this.rdInt(this.rdInt(this.ImageAddress(pOffset)) + pOffset2) + pOffset3) + pOffset4) + pOffset5);
        }

        public int Pointer(string Module, int pOffset, int pOffset2, int pOffset3, int pOffset4, int pOffset5)
        {
            return (this.rdInt(this.rdInt(this.rdInt(this.rdInt(this.DllImageAddress(Module) + pOffset) + pOffset2) + pOffset3) + pOffset4) + pOffset5);
        }

        public int Pointer(bool AddToImageAddress, int pOffset, int pOffset2, int pOffset3, int pOffset4, int pOffset5, int pOffset6)
        {
            return (this.rdInt(this.rdInt(this.rdInt(this.rdInt(this.rdInt(this.ImageAddress(pOffset)) + pOffset2) + pOffset3) + pOffset4) + pOffset5) + pOffset6);
        }

        public int Pointer(string Module, int pOffset, int pOffset2, int pOffset3, int pOffset4, int pOffset5, int pOffset6)
        {
            return (this.rdInt(this.rdInt(this.rdInt(this.rdInt(this.rdInt(this.DllImageAddress(Module) + pOffset) + pOffset2) + pOffset3) + pOffset4) + pOffset5) + pOffset6);
        }

        public byte rdByte(int pOffset)
        {
            byte[] buffer = new byte[1];
            ReadProcessMemory(this.processHandle, pOffset, buffer, 1, 0);
            return buffer[0];
        }

        public byte rdByte(bool AddToImageAddress, int pOffset)
        {
            byte[] buffer = new byte[1];
            int lpBaseAddress = AddToImageAddress ? this.ImageAddress(pOffset) : pOffset;
            ReadProcessMemory(this.processHandle, lpBaseAddress, buffer, 1, 0);
            return buffer[0];
        }

        public byte rdByte(string Module, int pOffset)
        {
            byte[] buffer = new byte[1];
            ReadProcessMemory(this.processHandle, this.DllImageAddress(Module) + pOffset, buffer, 1, 0);
            return buffer[0];
        }

        public float rdFloat(int pOffset)
        {
            return BitConverter.ToSingle(this.rdMem(pOffset, 4), 0);
        }

        public float rdFloat(bool AddToImageAddress, int pOffset)
        {
            return BitConverter.ToSingle(this.rdMem(pOffset, 4, AddToImageAddress), 0);
        }

        public float rdFloat(string Module, int pOffset)
        {
            return BitConverter.ToSingle(this.rdMem(this.DllImageAddress(Module) + pOffset, 4), 0);
        }

        public int rdInt(int pOffset)
        {
            return BitConverter.ToInt32(this.rdMem(pOffset, 4), 0);
        }

        public int rdInt(bool AddToImageAddress, int pOffset)
        {
            return BitConverter.ToInt32(this.rdMem(pOffset, 4, AddToImageAddress), 0);
        }

        public int rdInt(string Module, int pOffset)
        {
            return BitConverter.ToInt32(this.rdMem(this.DllImageAddress(Module) + pOffset, 4), 0);
        }

        public byte[] rdMem(int pOffset, int pSize)
        {
            byte[] buffer = new byte[pSize];
            ReadProcessMemory(this.processHandle, pOffset, buffer, pSize, 0);
            return buffer;
        }

        public byte[] rdMem(int pOffset, int pSize, bool AddToImageAddress)
        {
            byte[] buffer = new byte[pSize];
            int lpBaseAddress = AddToImageAddress ? this.ImageAddress(pOffset) : pOffset;
            ReadProcessMemory(this.processHandle, lpBaseAddress, buffer, pSize, 0);
            return buffer;
        }
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesrd);
        public short rdShort(int pOffset)
        {
            return BitConverter.ToInt16(this.rdMem(pOffset, 2), 0);
        }

        public short rdShort(bool AddToImageAddress, int pOffset)
        {
            return BitConverter.ToInt16(this.rdMem(pOffset, 2, AddToImageAddress), 0);
        }

        public short rdShort(string Module, int pOffset)
        {
            return BitConverter.ToInt16(this.rdMem(this.DllImageAddress(Module) + pOffset, 2), 0);
        }

        public string rdStringAscii(int pOffset, int pSize)
        {
            return this.CutString(Encoding.ASCII.GetString(this.rdMem(pOffset, pSize)));
        }

        public string rdStringAscii(bool AddToImageAddress, int pOffset, int pSize)
        {
            return this.CutString(Encoding.ASCII.GetString(this.rdMem(pOffset, pSize, AddToImageAddress)));
        }

        public string rdStringAscii(string Module, int pOffset, int pSize)
        {
            return this.CutString(Encoding.ASCII.GetString(this.rdMem(this.DllImageAddress(Module) + pOffset, pSize)));
        }

        public string rdStringUnicode(int pOffset, int pSize)
        {
            return this.CutString(Encoding.Unicode.GetString(this.rdMem(pOffset, pSize)));
        }

        public string rdStringUnicode(bool AddToImageAddress, int pOffset, int pSize)
        {
            return this.CutString(Encoding.Unicode.GetString(this.rdMem(pOffset, pSize, AddToImageAddress)));
        }

        public string rdStringUnicode(string Module, int pOffset, int pSize)
        {
            return this.CutString(Encoding.Unicode.GetString(this.rdMem(this.DllImageAddress(Module) + pOffset, pSize)));
        }

        public uint rdUInt(int pOffset)
        {
            return BitConverter.ToUInt32(this.rdMem(pOffset, 4), 0);
        }

        public uint rdUInt(bool AddToImageAddress, int pOffset)
        {
            return BitConverter.ToUInt32(this.rdMem(pOffset, 4, AddToImageAddress), 0);
        }

        public uint rdUInt(string Module, int pOffset)
        {
            return BitConverter.ToUInt32(this.rdMem(this.DllImageAddress(Module) + pOffset, 4), 0);
        }

        public double rdDouble(int pOffset)
        {
            return BitConverter.ToDouble(this.rdMem(pOffset, 8), 0);
        }

        public Vectoring.Vector3 rdVector(int pOffset)
        {
            Vectoring.Vector3 vec = new Vectoring.Vector3();
            vec.x = rdFloat(pOffset);
            vec.y = rdFloat(pOffset + 0x4);
            vec.z = rdFloat(pOffset + 0x8);
            return vec;
        }

        public bool StartProcess()
        {
            if (this.ProcessName != "")
            {
                this.MyProcess = Process.GetProcessesByName(this.ProcessName);
                if (this.MyProcess.Length == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("'" + this.ProcessName + ".exe' is not running. Please start it first!");
                    Thread.Sleep(3000);
                    return false;
                }
                this.processHandle = OpenProcess(2035711, false, this.MyProcess[0].Id);
                if (this.processHandle == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("'" + this.ProcessName + ".exe' is not running. Please start it first!");
                    Thread.Sleep(3000);
                    return false;
                }
                return true;
            }
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("Define process name first!");
            return false;
        }

        [DllImport("kernel32.dll")]
        public static extern bool VirtualProtectEx(int hProcess, int lpAddress, int dwSize, uint flNewProtect, out uint lpflOldProtect);
        public void WrtByte(int pOffset, byte pBytes)
        {
            this.WrtMem(pOffset, BitConverter.GetBytes((short)pBytes));
        }

        public void WrtByte(bool AddToImageAddress, int pOffset, byte pBytes)
        {
            this.WrtMem(pOffset, BitConverter.GetBytes((short)pBytes), AddToImageAddress);
        }

        public void WrtByte(string Module, int pOffset, byte pBytes)
        {
            this.WrtMem(this.DllImageAddress(Module) + pOffset, BitConverter.GetBytes((short)pBytes));
        }

        public void WrtDouble(int pOffset, double pBytes)
        {
            this.WrtMem(pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WrtDouble(bool AddToImageAddress, int pOffset, double pBytes)
        {
            this.WrtMem(pOffset, BitConverter.GetBytes(pBytes), AddToImageAddress);
        }

        public void WrtDouble(string Module, int pOffset, double pBytes)
        {
            this.WrtMem(this.DllImageAddress(Module) + pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WrtFloat(int pOffset, float pBytes)
        {
            this.WrtMem(pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WrtFloat(bool AddToImageAddress, int pOffset, float pBytes)
        {
            this.WrtMem(pOffset, BitConverter.GetBytes(pBytes), AddToImageAddress);
        }

        public void WrtFloat(string Module, int pOffset, float pBytes)
        {
            this.WrtMem(this.DllImageAddress(Module) + pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WrtInt(int pOffset, int pBytes)
        {
            this.WrtMem(pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WrtInt(bool AddToImageAddress, int pOffset, int pBytes)
        {
            this.WrtMem(pOffset, BitConverter.GetBytes(pBytes), AddToImageAddress);
        }

        public void WrtInt(string Module, int pOffset, int pBytes)
        {
            this.WrtMem(this.DllImageAddress(Module) + pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WrtMem(int pOffset, byte[] pBytes)
        {
            WriteProcessMemory(this.processHandle, pOffset, pBytes, pBytes.Length, 0);
        }

        public void WrtMem(int pOffset, byte[] pBytes, bool AddToImageAddress)
        {
            int lpBaseAddress = AddToImageAddress ? this.ImageAddress(pOffset) : pOffset;
            WriteProcessMemory(this.processHandle, lpBaseAddress, pBytes, pBytes.Length, 0);
        }

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesWritten);
        public void WrtShort(int pOffset, short pBytes)
        {
            this.WrtMem(pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WrtShort(bool AddToImageAddress, int pOffset, short pBytes)
        {
            this.WrtMem(pOffset, BitConverter.GetBytes(pBytes), AddToImageAddress);
        }

        public void WrtShort(string Module, int pOffset, short pBytes)
        {
            this.WrtMem(this.DllImageAddress(Module) + pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WrtStringAscii(int pOffset, string pBytes)
        {
            this.WrtMem(pOffset, Encoding.ASCII.GetBytes(pBytes + "\0"));
        }

        public void WrtStringAscii(bool AddToImageAddress, int pOffset, string pBytes)
        {
            this.WrtMem(pOffset, Encoding.ASCII.GetBytes(pBytes + "\0"), AddToImageAddress);
        }

        public void WrtStringAscii(string Module, int pOffset, string pBytes)
        {
            this.WrtMem(this.DllImageAddress(Module) + pOffset, Encoding.ASCII.GetBytes(pBytes + "\0"));
        }

        public void WrtStringUnicode(int pOffset, string pBytes)
        {
            this.WrtMem(pOffset, Encoding.Unicode.GetBytes(pBytes + "\0"));
        }

        public void WrtStringUnicode(bool AddToImageAddress, int pOffset, string pBytes)
        {
            this.WrtMem(pOffset, Encoding.Unicode.GetBytes(pBytes + "\0"), AddToImageAddress);
        }

        public void WrtStringUnicode(string Module, int pOffset, string pBytes)
        {
            this.WrtMem(this.DllImageAddress(Module) + pOffset, Encoding.Unicode.GetBytes(pBytes + "\0"));
        }

        public void WrtUInt(int pOffset, uint pBytes)
        {
            this.WrtMem(pOffset, BitConverter.GetBytes(pBytes));
        }

        public void WrtUInt(bool AddToImageAddress, int pOffset, uint pBytes)
        {
            this.WrtMem(pOffset, BitConverter.GetBytes(pBytes), AddToImageAddress);
        }

        public void WrtUInt(string Module, int pOffset, uint pBytes)
        {
            this.WrtMem(this.DllImageAddress(Module) + pOffset, BitConverter.GetBytes(pBytes));
        }
        public Process getProc()
        {
            return MyProcess[0];
        }

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 2035711,
            CreateThread = 2,
            DupHandle = 64,
            QueryInformation = 1024,
            SetInformation = 512,
            Synchronize = 1048576,
            Terminate = 1,
            VMOperation = 8,
            VMRead = 16,
            VMWrt = 32
        }

        const UInt32 WM_KEYDOWN = 0x0100;
        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


        [DllImport("User32.dll")]
        static extern void SendInput(uint nInputs, NativeWIN32.INPUT pInputs, int cbSize);
        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

    }
}