// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RemoteThread.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the RemoteThread type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System;

    using Microsoft.Win32.SafeHandles;

    using SmokeLounge.AOtomation.Hook.Kernel32;

    public class RemoteThread : SafeHandleZeroOrMinusOneIsInvalid
    {
        #region Fields

        private readonly IntPtr processHandle;

        private readonly IntPtr targetAddress;

        #endregion

        #region Constructors and Destructors

        public RemoteThread(IntPtr processHandle, IntPtr targetAddress)
            : base(true)
        {
            this.processHandle = processHandle;
            this.targetAddress = targetAddress;
        }

        #endregion

        #region Public Properties

        public IntPtr ProcessHandle
        {
            get
            {
                return this.processHandle;
            }
        }

        public IntPtr TargetAddress
        {
            get
            {
                return this.targetAddress;
            }
        }

        #endregion

        #region Public Methods and Operators

        public int ExitCode()
        {
            if (this.IsInvalid)
            {
                return 0;
            }

            uint exitCode;
            NativeMethods.GetExitCodeThread(this.handle, out exitCode);
            return Convert.ToInt32(exitCode);
        }

        public void Join(TimeSpan timeout)
        {
            this.JoinInt((uint)timeout.TotalMilliseconds);
        }

        public void Join()
        {
            this.JoinInt(uint.MaxValue);
        }

        public void Join(int millisecondsTimeout)
        {
            this.JoinInt((uint)millisecondsTimeout);
        }

        public void Start()
        {
            this.Start(IntPtr.Zero);
        }

        public void Start(IntPtr parameterAddress)
        {
            uint threadId;
            this.handle = NativeMethods.CreateRemoteThread(
                this.processHandle, IntPtr.Zero, 0, this.targetAddress, parameterAddress, 0, out threadId);
        }

        #endregion

        #region Methods

        protected override bool ReleaseHandle()
        {
            if (this.IsInvalid)
            {
                return false;
            }

            return NativeMethods.CloseHandle(this.handle);
        }

        private void JoinInt(uint millisecondsTimeout)
        {
            if (this.IsClosed)
            {
                return;
            }

            if (this.IsInvalid)
            {
                return;
            }

            NativeMethods.WaitForSingleObject(this.handle, millisecondsTimeout);
        }

        #endregion
    }
}