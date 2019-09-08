using System.Collections.Generic;
using System.Linq;

namespace BExpr.Model
{
    public class MethodArgs<T> : IExpression<T>
    {
        public MethodArgs(IReadOnlyList<IExpression<T>> argList)
        {
            ArgList = argList;
        }

        public IReadOnlyList<IExpression<T>> ArgList { get; }

        public ExpressionResult Evaluate(T target, EvaluationContext context)
        {
            var results = ArgList.Select(arg => arg.Evaluate(target, context)).ToArray();
            if(results.Any(r => r.IsError))
            {
                return ExpressionResult.Error(results);
            }
            var args = results.Select(arg => arg.Value).ToList();
            return new ExpressionResult(args);
        }

        public override string ToString()
        {
            return string.Join(", ", ArgList);
        }
    }
}