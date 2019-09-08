namespace BExpr.Model
{
    public class Not<T> : UnaryExpression<T>
    {
        public Not(IExpression<T> expression)
            : base(expression, "!") { }

        protected override ExpressionResult Evaluate(object value)
        {
            if (value is bool b)
            {
                return new ExpressionResult(!b);
            }
            return new ExpressionResult(new ErrorInfo("TypeError", $"Unable to apply '!' operator to {value?.GetType()}"));
        }
    }
}