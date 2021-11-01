using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CoxAutomotive.Infrastructure.Utilities
{
    public interface ICsvSerializer<T>
    {
        bool IgnoreEmptyLines { get; set; }
        bool IgnoreReferenceTypesExceptString { get; set; }
        string NewlineReplacement { get; set; }
        string Replacement { get; set; }
        string RowNumberColumnTitle { get; set; }
        char Separator { get; set; }
        string SplitRex { get; set; }
        bool UseEofLiteral { get; set; }
        bool UseLineNumbers { get; set; }
        bool UseTextQualifier { get; set; }

        Task<IList<T>> DeserializeAsync(Stream stream);
    }
}
