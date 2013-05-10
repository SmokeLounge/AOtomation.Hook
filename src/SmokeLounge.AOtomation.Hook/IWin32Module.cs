// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWin32Module.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the IWin32Module type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(IWin32ModuleContract))]
    public interface IWin32Module
    {
        #region Public Properties

        IntPtr BaseAddress { get; }

        string Name { get; }

        IntPtr Win32ProcessHandle { get; }

        #endregion
    }

    [ContractClassFor(typeof(IWin32Module))]
    internal abstract class IWin32ModuleContract : IWin32Module
    {
        #region Public Properties

        public IntPtr BaseAddress
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IntPtr Win32ProcessHandle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}