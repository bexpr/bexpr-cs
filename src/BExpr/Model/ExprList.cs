using System.Collections.Generic;

namespace BExpr.Model
{
    public class ExprList<T> : IExpression<T>
    {
        public ExprList(IReadOnlyList<IExpression<T>> expressions)
        {
            Expressions = expressions;
        }

        public IReadOnlyList<IExpression<T>> Expressions { get; }

        public ExpressionResult Evaluate(T target, EvaluationContext context)
        {
            ExpressionResult result = null;
            foreach(var expr in Expressions)
            {
                result = expr.Evaluate(target, context);
            }
            return result;
        }

        public override string ToString()
        {
            return "(\n" + string.Join(";\n", Expressions) + "\n)";
        }
    }
}