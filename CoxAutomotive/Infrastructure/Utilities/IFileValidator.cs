using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace CoxAutomotive.Infrastructure.Utilities
{
    public interface IFileValidator
    {
        List<string> Validate(IFormFile file);
    }
}
