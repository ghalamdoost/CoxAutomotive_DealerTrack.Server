using CoxAutomotive.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoxAutomotive.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var services = app.ApplicationServices.CreateScope();
            var dbContenxt = services.ServiceProvider.GetService<CoxAutomotiveDbContext>();

            dbContenxt.Database.Migrate();
        }
    }
}
