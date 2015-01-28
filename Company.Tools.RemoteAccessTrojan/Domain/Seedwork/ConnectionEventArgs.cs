using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.Tools.RemoteAccessTrojan.Domain.Seedwork
{
    public class ConnectionEventArgs : EventArgs
    {
        public Connection Connection { get; private set; }

        public ConnectionEventArgs(Connection connection)
        {
            this.Connection = connection;
        }
    }
}