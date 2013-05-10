// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWin32Process.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the IWin32Process type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(IWin32ProcessContract))]
    public interface IWin32Process
    {
        #region Public Properties

        IntPtr Handle { get; }

        bool HasExited { get; }

        int Id { get; }

        IReadOnlyCollection<IWin32Module> Modules { get; }

        #endregion
    }

    [ContractClassFor(typeof(IWin32Process))]
    internal abstract class IWin32ProcessContract : IWin32Process
    {
        #region Public Properties

        public IntPtr Handle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool HasExited
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Id
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IReadOnlyCollection<IWin32Module> Modules
        {
            get
            {
                Contract.Ensures(Contract.Result<IReadOnlyCollection<IWin32Module>>() != null);

                throw new NotImplementedException();
            }
        }

        #endregion
    }
}