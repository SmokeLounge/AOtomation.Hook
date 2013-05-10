// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWin32ProcessRepository.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the IWin32ProcessRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(IWin32ProcessRepositoryContract))]
    public interface IWin32ProcessRepository
    {
        #region Public Methods and Operators

        IWin32Process GetProcessById(int id);

        IReadOnlyCollection<IWin32Process> GetProcessesByName(string processName);

        #endregion
    }

    [ContractClassFor(typeof(IWin32ProcessRepository))]
    internal abstract class IWin32ProcessRepositoryContract : IWin32ProcessRepository
    {
        #region Public Methods and Operators

        public IWin32Process GetProcessById(int id)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<IWin32Process> GetProcessesByName(string processName)
        {
            Contract.Ensures(Contract.Result<IReadOnlyCollection<IWin32Process>>() != null);

            throw new NotImplementedException();
        }

        #endregion
    }
}