// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIpcServerChannel.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the IIpcServerChannel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook.Communication.NamedPipe
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(IIpcServerChannelContract))]
    public interface IIpcServerChannel : IDisposable
    {
        #region Public Properties

        string Name { get; }

        Action<byte[], int, Action> PacketReceivedCallback { get; set; }

        #endregion

        #region Public Methods and Operators

        void Start();

        void Stop();

        #endregion
    }

    [ContractClassFor(typeof(IIpcServerChannel))]
    internal abstract class IIpcServerChannelContract : IIpcServerChannel
    {
        #region Public Properties

        public string Name
        {
            get
            {
                Contract.Ensures(string.IsNullOrWhiteSpace(Contract.Result<string>()) == false);

                throw new NotImplementedException();
            }
        }

        public Action<byte[], int, Action> PacketReceivedCallback
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}