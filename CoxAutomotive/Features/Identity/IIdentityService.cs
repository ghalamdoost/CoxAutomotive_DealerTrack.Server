using System.Threading.Tasks;

namespace CoxAutomotive.Features.Identity
{
    public interface IIdentityService
    {
        Task<string> GenerateJwtToken(string userId, string userName, string secret);
    }
}
