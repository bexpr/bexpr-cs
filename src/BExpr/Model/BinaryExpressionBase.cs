namespace BExpr.Model
{
    public abstract class BinaryExpressionBase<T> : IExpression<T>
    {
        public BinaryExpressionBase(IExpression<T> left, IExpression<T> right, string op)
        {
            Left = left;
            Right = right;
            Op = op;
        }

        public IExpression<T> Left { get; }
        public IExpression<T> Right { get; }
        public string Op { get; }

        protected ExpressionResult Value(object value)
        {
            return new ExpressionResult(value);
        }
        public abstract ExpressionResult Evaluate(T target, EvaluationContext context);

        public override string ToString()
        {
            return $"{Left} {Op} {Right}";
        }
    }
}