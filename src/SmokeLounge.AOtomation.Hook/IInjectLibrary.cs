// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInjectLibrary.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the IInjectLibrary type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System;

    public interface IInjectLibrary
    {
        #region Public Methods and Operators

        IntPtr InjectToProcess(IntPtr processHandle);

        #endregion
    }
}