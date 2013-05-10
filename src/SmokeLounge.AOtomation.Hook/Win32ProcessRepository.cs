// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Win32ProcessRepository.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the Win32ProcessRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;

    [Export(typeof(IWin32ProcessRepository))]
    public class Win32ProcessRepository : IWin32ProcessRepository
    {
        #region Fields

        private readonly IWin32ProcessFactory win32ProcessFactory;

        #endregion

        #region Constructors and Destructors

        [ImportingConstructor]
        public Win32ProcessRepository(IWin32ProcessFactory win32ProcessFactory)
        {
            Contract.Requires<ArgumentNullException>(win32ProcessFactory != null);
            this.win32ProcessFactory = win32ProcessFactory;
        }

        #endregion

        #region Public Methods and Operators

        public IWin32Process GetProcessById(int id)
        {
            try
            {
                var process = Process.GetProcessById(id);
                Contract.Assume(process != null);
                return this.win32ProcessFactory.Create(process);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        public IReadOnlyCollection<IWin32Process> GetProcessesByName(string processName)
        {
            return Process.GetProcessesByName(processName).Select(this.win32ProcessFactory.Create).ToArray();
        }

        #endregion

        #region Methods

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.win32ProcessFactory != null);
        }

        #endregion
    }
}