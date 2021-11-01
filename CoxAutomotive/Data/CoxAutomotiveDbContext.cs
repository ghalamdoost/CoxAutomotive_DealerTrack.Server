using CoxAutomotive.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoxAutomotive.Data
{
    public class CoxAutomotiveDbContext : IdentityDbContext<User>
    {
        public CoxAutomotiveDbContext(DbContextOptions<CoxAutomotiveDbContext> options)
            : base(options)
        {
        }
    }
}
