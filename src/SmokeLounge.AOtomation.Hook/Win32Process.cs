// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Win32Process.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the Win32Process type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.Linq;

    public class Win32Process : IWin32Process
    {
        #region Fields

        private readonly Process process;

        #endregion

        #region Constructors and Destructors

        public Win32Process(Process process)
        {
            Contract.Requires<ArgumentNullException>(process != null);
            this.process = process;
        }

        #endregion

        #region Public Properties

        public IntPtr Handle
        {
            get
            {
                return this.process.Handle;
            }
        }

        public bool HasExited
        {
            get
            {
                return this.process.HasExited;
            }
        }

        public int Id
        {
            get
            {
                return this.process.Id;
            }
        }

        public IReadOnlyCollection<IWin32Module> Modules
        {
            get
            {
                Contract.Assume(this.process.Modules != null);
                return
                    this.process.Modules.OfType<ProcessModule>()
                        .Select(m => new Win32Module(this.Handle, m.ModuleName, m.BaseAddress))
                        .ToArray();
            }
        }

        #endregion

        #region Methods

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.process != null);
        }

        #endregion
    }
}