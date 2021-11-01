using CoxAutomotive.Features.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CoxAutomotive.Tests.Unit
{
    public class ServiceBuilder
    {
        public ServiceProvider ServiceProvider { get; private set; }
        public ServiceBuilder()
        {
            var service = new ServiceCollection();
            service.AddTransient<IIdentityService, IdentityService>();
            ServiceProvider = service.BuildServiceProvider();
        }
    }
}
