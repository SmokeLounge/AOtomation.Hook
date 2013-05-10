// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIpcServerChannelFactory.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the IIpcServerChannelFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook.Communication.NamedPipe
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(IIpcServerChannelFactoryContract))]
    public interface IIpcServerChannelFactory
    {
        #region Public Methods and Operators

        IIpcServerChannel Create(string channelName);

        #endregion
    }

    [ContractClassFor(typeof(IIpcServerChannelFactory))]
    internal abstract class IIpcServerChannelFactoryContract : IIpcServerChannelFactory
    {
        #region Public Methods and Operators

        public IIpcServerChannel Create(string channelName)
        {
            Contract.Requires<ArgumentNullException>(channelName != null);
            Contract.Requires<ArgumentException>(channelName != string.Empty);
            Contract.Ensures(Contract.Result<IIpcServerChannel>() != null);

            throw new NotImplementedException();
        }

        #endregion
    }
}