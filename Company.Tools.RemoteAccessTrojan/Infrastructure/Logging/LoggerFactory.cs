using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Company.Tools.RemoteAccessTrojan.Infrastructure.CrossCutting;

namespace Company.Tools.RemoteAccessTrojan.Infrastructure.Logging
{
    public class LoggerFactory
    {
        public Logger Create()
        {
            var logger = (Logger)Config.Current.LoggerTypeName.GetLocalInstance();
            return logger;
        }
    }
}