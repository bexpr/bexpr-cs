using System;
using System.Linq;

namespace BExpr.Model
{
    public class TypeError : ErrorInfo
    {
        public TypeError(string source, params Type[] types) 
            : base(
                  "TypeError",
                  $"The type(s) {string.Join(", ", types.Select(t => t.FullName))} " +
                  $"are not valid in this context [{source}]")
        {
        }
    }
}