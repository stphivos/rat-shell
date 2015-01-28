using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Company.Tools.RemoteAccessTrojan.Infrastructure.CrossCutting;

namespace Company.Tools.RemoteAccessTrojan.Application
{
    public class ApplicationServiceFactory
    {
        public IApplicationService Create()
        {
            var service = (ApplicationService)Config.Current.ApplicationServiceTypeName.GetLocalInstance();
            return service;
        }
    }
}