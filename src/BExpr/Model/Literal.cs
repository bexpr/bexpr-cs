namespace BExpr.Model
{
    public abstract class Literal<T, TValue> : IExpression<T>
    {
        protected Literal(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }

        public ExpressionResult Evaluate(T target, EvaluationContext context)
        {
            return new ExpressionResult(Value);
        }
    }
}