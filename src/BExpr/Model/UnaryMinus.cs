namespace BExpr.Model
{
    public class UnaryMinus<T> : UnaryExpression<T>
    {
        public UnaryMinus(IExpression<T> expression)
            : base(expression, "-") { }

        protected override ExpressionResult Evaluate(object value)
        {
            switch(value)
            {
                case double d:
                    return new ExpressionResult(-d);
                case float f:
                    return new ExpressionResult(-f);
                case long l:
                    return new ExpressionResult(-l);
                case int i:
                    return new ExpressionResult(-i);
                case short s:
                    return new ExpressionResult(-s);
                case byte b:
                    return new ExpressionResult(-b);
            }
            return ExpressionResult.TypeError("-", value?.GetType());
        }
    }
}