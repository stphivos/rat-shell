using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Company.Tools.RemoteAccessTrojan.Infrastructure.Logging
{
    public abstract class Logger
    {
        public void WriteException(Exception ex)
        {
            this.WriteException(ex, null);
        }

        public void WriteException(Exception ex, string title)
        {
            this.WriteEntry(title, ex.ToString(), EventLogEntryType.Error);
        }

        public abstract void WriteEntry(string title, string message, EventLogEntryType type);
    }
}