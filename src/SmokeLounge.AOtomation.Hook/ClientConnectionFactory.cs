// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientConnectionFactory.cs" company="SmokeLounge">
//   Copyright © 2013 SmokeLounge.
//   This program is free software. It comes without any warranty, to
//   the extent permitted by applicable law. You can redistribute it
//   and/or modify it under the terms of the Do What The Fuck You Want
//   To Public License, Version 2, as published by Sam Hocevar. See
//   http://www.wtfpl.net/ for more details.
// </copyright>
// <summary>
//   Defines the ClientConnectionFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SmokeLounge.AOtomation.Hook
{
    using System;
    using System.ComponentModel.Composition;
    using System.Diagnostics.Contracts;

    using SmokeLounge.AOtomation.Hook.Communication.NamedPipe;

    [Export(typeof(IClientConnectionFactory))]
    public class ClientConnectionFactory : IClientConnectionFactory
    {
        #region Fields

        private readonly IInjectLibrary injectLibrary;

        private readonly IIpcServerChannelFactory ipcServerChannelFactory;

        private readonly IReadProcessMemory readProcessMemory;

        private readonly IWin32ProcessRepository win32ProcessRepository;

        #endregion

        #region Constructors and Destructors

        [ImportingConstructor]
        public ClientConnectionFactory(
            IInjectLibrary injectLibrary, 
            IWin32ProcessRepository win32ProcessRepository, 
            IIpcServerChannelFactory ipcServerChannelFactory, 
            IReadProcessMemory readProcessMemory)
        {
            Contract.Requires<ArgumentNullException>(injectLibrary != null);
            Contract.Requires<ArgumentNullException>(win32ProcessRepository != null);
            Contract.Requires<ArgumentNullException>(ipcServerChannelFactory != null);
            Contract.Requires<ArgumentNullException>(readProcessMemory != null);

            this.injectLibrary = injectLibrary;
            this.win32ProcessRepository = win32ProcessRepository;
            this.ipcServerChannelFactory = ipcServerChannelFactory;
            this.readProcessMemory = readProcessMemory;
        }

        #endregion

        #region Public Methods and Operators

        public IClientConnection Create(int remoteProcessId)
        {
            var win32Process = this.win32ProcessRepository.GetProcessById(remoteProcessId);
            this.injectLibrary.InjectToProcess(win32Process.Handle);

            var sendHookCallbackChannelName = "AnarchyHook" + remoteProcessId + "cs";
            Contract.Assume(string.IsNullOrWhiteSpace(sendHookCallbackChannelName) == false);
            var sendHookCallbackChannel = this.ipcServerChannelFactory.Create(sendHookCallbackChannelName);

            var receiveHookCallbackChannelName = "AnarchyHook" + remoteProcessId + "dm";
            Contract.Assume(string.IsNullOrWhiteSpace(receiveHookCallbackChannelName) == false);
            var receiveHookCallbackChannel = this.ipcServerChannelFactory.Create(receiveHookCallbackChannelName);

            var hookServerChannelName = "AnarchyHook" + remoteProcessId;
            Contract.Assume(string.IsNullOrWhiteSpace(hookServerChannelName) == false);

            return new ClientConnection(
                win32Process.Handle, 
                hookServerChannelName, 
                sendHookCallbackChannel, 
                receiveHookCallbackChannel, 
                this.readProcessMemory);
        }

        #endregion

        #region Methods

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.injectLibrary != null);
            Contract.Invariant(this.win32ProcessRepository != null);
            Contract.Invariant(this.ipcServerChannelFactory != null);
            Contract.Invariant(this.readProcessMemory != null);
        }

        #endregion
    }
}