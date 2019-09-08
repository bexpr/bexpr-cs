namespace BExpr.Model
{
    public class ParenExpr<T> : IExpression<T>
    {
        public ParenExpr(IExpression<T> expression)
        {
            Expression = expression;
        }

        public IExpression<T> Expression { get; }

        public ExpressionResult Evaluate(T target, EvaluationContext context)
        {
            return Expression.Evaluate(target, context);
        }

        public override string ToString()
        {
            return $"({Expression})";
        }
    }
}