using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Company.Tools.RemoteAccessTrojan.Server
{
    public class Terminal : IDisposable
    {
        public bool IsStopped { get; set; }

        public void Start(string message)
        {
            this.WriteOutputLine("{0}", message);
            while (!this.IsStopped)
            {
                Thread.Sleep(100);
            }
        }

        public void Stop()
        {
            this.IsStopped = true;
        }

        public string RequestInput(string format, params object[] args)
        {
            this.WriteOutput(format, args);
            var input = Console.ReadLine();
            return input;
        }

        public void WriteOutput(string format, params object[] args)
        {
            Console.Write(string.Format(format, args));
        }

        public void WriteOutputLine(string format, params object[] args)
        {
            this.WriteOutput(format, args);
            Console.WriteLine();
        }

        public void WriteError(Exception ex)
        {
            this.WriteOutputLine("{0}", ex);
        }

        public void Dispose()
        {
            Console.WriteLine("Press any key to exit...");
            Console.Read();
        }
    }
}