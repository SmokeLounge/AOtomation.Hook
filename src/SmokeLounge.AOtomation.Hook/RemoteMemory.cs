// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoteMemory.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the RemoteMemory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System;

    using Microsoft.Win32.SafeHandles;

    using SmokeLounge.AOtomation.Hook.Kernel32;

    internal class RemoteMemory : CriticalHandleZeroOrMinusOneIsInvalid
    {
        #region Fields

        private readonly IntPtr processHandle;

        private readonly int size;

        #endregion

        #region Constructors and Destructors

        public RemoteMemory(IntPtr processHandle, int size)
        {
            this.processHandle = processHandle;
            this.size = size;
            this.handle = NativeMethods.VirtualAllocEx(
                processHandle, 
                IntPtr.Zero, 
                (uint)size, 
                (uint)(MemoryAllocationTypes.Reserve | MemoryAllocationTypes.Commit), 
                (uint)MemoryProtectionConstant.ReadWrite);
        }

        #endregion

        #region Public Properties

        public IntPtr Address
        {
            get
            {
                return this.handle;
            }
        }

        public IntPtr ProcessHandle
        {
            get
            {
                return this.processHandle;
            }
        }

        public int Size
        {
            get
            {
                return this.size;
            }
        }

        #endregion

        #region Methods

        protected override bool ReleaseHandle()
        {
            return NativeMethods.VirtualFreeEx(
                this.processHandle, this.handle, (uint)this.size, (uint)MemoryFreeType.Release);
        }

        #endregion
    }
}