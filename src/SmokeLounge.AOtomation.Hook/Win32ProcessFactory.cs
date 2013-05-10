// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Win32ProcessFactory.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the Win32ProcessFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System.ComponentModel.Composition;
    using System.Diagnostics;

    [Export(typeof(IWin32ProcessFactory))]
    public class Win32ProcessFactory : IWin32ProcessFactory
    {
        #region Public Methods and Operators

        public IWin32Process Create(Process process)
        {
            return new Win32Process(process);
        }

        #endregion
    }
}