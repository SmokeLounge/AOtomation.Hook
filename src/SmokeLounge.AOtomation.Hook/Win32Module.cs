// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Win32Module.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the Win32Module type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System;

    public class Win32Module : IWin32Module
    {
        #region Fields

        private readonly IntPtr baseAddress;

        private readonly string name;

        private readonly IntPtr win32ProcessHandle;

        #endregion

        #region Constructors and Destructors

        public Win32Module(IntPtr win32ProcessHandle, string name, IntPtr baseAddress)
        {
            this.win32ProcessHandle = win32ProcessHandle;
            this.name = name;
            this.baseAddress = baseAddress;
        }

        #endregion

        #region Public Properties

        public IntPtr BaseAddress
        {
            get
            {
                return this.baseAddress;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public IntPtr Win32ProcessHandle
        {
            get
            {
                return this.win32ProcessHandle;
            }
        }

        #endregion
    }
}