using CoxAutomotive.Features.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CoxAutomotive.Tests.Unit
{
    public class IdentityServiceTest : IClassFixture<ServiceBuilder>
    {
        private static readonly Random random = new Random();
        private readonly IIdentityService IdentityService;
        public IdentityServiceTest(ServiceBuilder builder)
        {
            this.IdentityService = builder.ServiceProvider.GetService<IIdentityService>();
        }

        [Fact]
        public async Task GenerateJwtToken_String()
        {
            var response = await this.IdentityService.GenerateJwtToken("13", "test", "SOME MAGIC UNICORNS GENERATE THIS SECRET");
            Assert.True(response is string);
        }
    }
}
