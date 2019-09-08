namespace BExpr.Model
{
    public class NullLiteral<T> : IExpression<T>
    {
        public ExpressionResult Evaluate(T target, EvaluationContext context)
        {
            return new ExpressionResult((object)null);
        }

        public override string ToString()
        {
            return "null";
        }
    }
}