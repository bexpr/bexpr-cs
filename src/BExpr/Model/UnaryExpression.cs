namespace BExpr.Model
{
    public abstract class UnaryExpression<T> : IExpression<T>
    {
        public UnaryExpression(IExpression<T> expression, string op)
        {
            Expression = expression;
            Op = op;
        }

        public IExpression<T> Expression { get; }
        public string Op { get; }

        public ExpressionResult Evaluate(T target, EvaluationContext context)
        {
            var result = Expression.Evaluate(target, context);
            return Evaluate(result.Value);
        }

        protected abstract ExpressionResult Evaluate(object value);

        public override string ToString()
        {
            return $"{Op}{Expression}";
        }
    }
}