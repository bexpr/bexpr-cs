using System.Collections.Generic;
using System.Linq;

namespace BExpr.Model
{
    public class List<T> : IExpression<T>
    {
        private readonly IReadOnlyList<IExpression<T>> items;

        public List(IReadOnlyList<IExpression<T>> items)
        {
            this.items = items;
        }

        public ExpressionResult Evaluate(T target, EvaluationContext context)
        {
            var mapped = items
                .Select(item => item.Evaluate(target, context))
                .ToList();

            if(mapped.Any(m => m.IsError))
            {
                return ExpressionResult.Error(mapped.ToArray());
            }

            return new ExpressionResult(
                new System.Collections.Generic.List<object>(mapped.Select(m => m.Value)));
        }

        public override string ToString()
        {
            return items.Count == 0 
                ? "{ }" 
                : "{ " + string.Join(", ", items.Select(i => i.ToString())) + " }";
        }
    }
}
