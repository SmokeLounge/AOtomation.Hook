// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InjectLibrary.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the InjectLibrary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Text;

    using SmokeLounge.AOtomation.Hook.Kernel32;

    [Export(typeof(IInjectLibrary))]
    public class InjectLibrary : IInjectLibrary
    {
        #region Public Methods and Operators

        public IntPtr InjectToProcess(IntPtr processHandle)
        {
            try
            {
                var dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AnarchyHook.dll");
                var buffer = Encoding.ASCII.GetBytes(dllPath);

                using (var remoteMemory = new RemoteMemory(processHandle, buffer.Length))
                {
                    using (var pinnedBuffer = new PinnedObject(buffer))
                    {
                        uint bytesWritten;
                        NativeMethods.WriteProcessMemory(
                            processHandle, 
                            remoteMemory.Address, 
                            pinnedBuffer.AddrOfPinnedObject, 
                            (uint)buffer.Length, 
                            out bytesWritten);
                    }

                    var kernel32Handle = NativeMethods.GetModuleHandle("kernel32.dll");
                    var loadLibraryAddress = NativeMethods.GetProcAddress(kernel32Handle, "LoadLibraryA");
                    using (var remoteThread = new RemoteThread(processHandle, loadLibraryAddress))
                    {
                        remoteThread.Start(remoteMemory.Address);
                        remoteThread.Join(TimeSpan.FromMilliseconds(5000));
                        var exitCode = remoteThread.ExitCode();
                        return new IntPtr(exitCode);
                    }
                }
            }
            catch (Exception)
            {
                return IntPtr.Zero;
            }
        }

        public bool UninjectFromProcess(IntPtr processHandle, IntPtr libraryHandle)
        {
            try
            {
                var buffer = BitConverter.GetBytes(libraryHandle.ToInt32());

                using (var remoteMemory = new RemoteMemory(processHandle, buffer.Length))
                {
                    using (var pinnedBuffer = new PinnedObject(buffer))
                    {
                        uint bytesWritten;
                        NativeMethods.WriteProcessMemory(
                            processHandle, 
                            remoteMemory.Address, 
                            pinnedBuffer.AddrOfPinnedObject, 
                            (uint)buffer.Length, 
                            out bytesWritten);
                    }

                    var kernel32Handle = NativeMethods.GetModuleHandle("kernel32.dll");
                    var freeLibraryAddress = NativeMethods.GetProcAddress(kernel32Handle, "FreeLibrary");
                    using (var remoteThread = new RemoteThread(processHandle, freeLibraryAddress))
                    {
                        remoteThread.Start(remoteMemory.Address);
                        remoteThread.Join(TimeSpan.FromMilliseconds(5000));
                        var exitCode = remoteThread.ExitCode();
                        return exitCode != 0;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}