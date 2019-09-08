using System;
using System.Collections.Generic;

namespace BExpr.Model
{
    public class EvaluationContext
    {
        private readonly IDictionary<string, Func<IReadOnlyList<object>, ExpressionResult>> functions 
            = new Dictionary<string, Func<IReadOnlyList<object>, ExpressionResult>>();

        public EvaluationContext()
        {

        }

        public bool HasFunction(string name)
        {
            return functions.ContainsKey(name);
        }

        public Func<IReadOnlyList<object>, ExpressionResult> GetFunction(string name)
        {
            return functions.ContainsKey(name) ? functions[name] : null;
        }
    }
}
