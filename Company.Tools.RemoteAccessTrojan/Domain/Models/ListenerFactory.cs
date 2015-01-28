using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Company.Tools.RemoteAccessTrojan.Domain.Models
{
    public class ListenerFactory
    {
        public Listener Create()
        {
            var listener = new Listener()
            {
            };
            return listener;
        }
    }
}