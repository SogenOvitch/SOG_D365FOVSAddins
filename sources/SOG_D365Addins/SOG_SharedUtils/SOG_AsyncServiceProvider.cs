using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell.Interop.COMAsyncServiceProvider;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using System.Threading.Tasks;

namespace SOG_SharedUtils
{
    public class SOG_AsyncServiceProvider : ServiceContainer, IServiceProvider, Microsoft.VisualStudio.Shell.IAsyncServiceProvider
    {
        public Task<object> GetServiceAsync(Type serviceType)
        {
            // Just wrap the synchronous lookup
            var service = GetService(serviceType);
            return Task.FromResult(service);
        }

        public Task<object> GetServiceAsync(Type serviceType, bool swallowErrors)
        {
            try
            {
                return GetServiceAsync(serviceType);
            }
            catch when (swallowErrors)
            {
                return Task.FromResult<object>(null);
            }
        }

        public IVsTask QueryServiceAsync(ref Guid guidService)
        {
            throw new NotImplementedException();
        }
    }
}
