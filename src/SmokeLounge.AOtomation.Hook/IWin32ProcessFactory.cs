// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWin32ProcessFactory.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the IWin32ProcessFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(IWin32ProcessFactoryContract))]
    public interface IWin32ProcessFactory
    {
        #region Public Methods and Operators

        IWin32Process Create(Process process);

        #endregion
    }

    [ContractClassFor(typeof(IWin32ProcessFactory))]
    internal abstract class IWin32ProcessFactoryContract : IWin32ProcessFactory
    {
        #region Public Methods and Operators

        public IWin32Process Create(Process process)
        {
            Contract.Requires<ArgumentNullException>(process != null);
            Contract.Ensures(Contract.Result<IWin32Process>() != null);

            throw new NotImplementedException();
        }

        #endregion
    }
}