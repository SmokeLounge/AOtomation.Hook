// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IClientConnection.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the IClientConnection type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System;
    using System.Diagnostics.Contracts;

    [ContractClass(typeof(IClientConnectionContract))]
    public interface IClientConnection : IDisposable
    {
        #region Public Properties

        Action<byte[], Action> ReceiveCallback { get; set; }

        Action<byte[], Action> SendCallback { get; set; }

        #endregion

        #region Public Methods and Operators

        void Send(byte[] message);

        void Start();

        #endregion
    }

    [ContractClassFor(typeof(IClientConnection))]
    internal abstract class IClientConnectionContract : IClientConnection
    {
        #region Public Properties

        public Action<byte[], Action> ReceiveCallback
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

        public Action<byte[], Action> SendCallback
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

        public void Send(byte[] message)
        {
            Contract.Requires<ArgumentNullException>(message != null);

            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}