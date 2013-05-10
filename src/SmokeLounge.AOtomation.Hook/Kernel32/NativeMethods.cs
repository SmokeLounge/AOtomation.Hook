// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the NativeMethods type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook.Kernel32
{
    using System;
    using System.Runtime.InteropServices;

    internal static class NativeMethods
    {
        [DllImport("kernel32.dll", EntryPoint = "CloseHandle")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle([In] IntPtr hObject);

        [DllImport("kernel32.dll", EntryPoint = "CreateRemoteThread")]
        public static extern IntPtr CreateRemoteThread(
            [In] IntPtr hProcess, 
            [In] IntPtr lpThreadAttributes, 
            uint dwStackSize, 
            IntPtr lpStartAddress, 
            [In] IntPtr lpParameter, 
            uint dwCreationFlags, 
            [Out] out uint lpThreadId);

        [DllImport("kernel32.dll", EntryPoint = "GetExitCodeThread")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetExitCodeThread([In] IntPtr hThread, [Out] out uint lpExitCode);

        [DllImport("kernel32.dll", EntryPoint = "GetModuleHandle")]
        public static extern IntPtr GetModuleHandle([In] [MarshalAs(UnmanagedType.LPStr)] string lpModuleName);

        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress")]
        public static extern IntPtr GetProcAddress(
            [In] IntPtr hModule, [In] [MarshalAs(UnmanagedType.LPStr)] string lpProcName);

        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ReadProcessMemory(
            [In] IntPtr hProcess, 
            [In] IntPtr lpBaseAddress, 
            IntPtr lpBuffer, 
            uint nSize, 
            [Out] out uint lpNumberOfBytesRead);

        [DllImport("kernel32.dll", EntryPoint = "WaitForSingleObject")]
        public static extern uint WaitForSingleObject([In] IntPtr hHandle, uint dwMilliseconds);

        [DllImport("kernel32.dll", EntryPoint = "WriteProcessMemory")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WriteProcessMemory(
            [In] IntPtr hProcess, 
            [In] IntPtr lpBaseAddress, 
            [In] IntPtr lpBuffer, 
            uint nSize, 
            [Out] out uint lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", EntryPoint = "VirtualAllocEx")]
        public static extern IntPtr VirtualAllocEx(
            [In] IntPtr hProcess, [In] IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll", EntryPoint = "VirtualFreeEx")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool VirtualFreeEx(
            [In] IntPtr hProcess, [In] IntPtr lpAddress, uint dwSize, uint dwFreeType);
    }
}