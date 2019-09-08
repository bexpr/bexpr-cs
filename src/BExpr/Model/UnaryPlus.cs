namespace BExpr.Model
{
    public class UnaryPlus<T> : UnaryExpression<T>
    {
        public UnaryPlus(IExpression<T> expression)
            : base(expression, "+") { }

        protected override ExpressionResult Evaluate(object value)
        {
            switch (value)
            {
                case double d:
                    return new ExpressionResult(+d);
                case float f:
                    return new ExpressionResult(+f);
                case long l:
                    return new ExpressionResult(+l);
                case int i:
                    return new ExpressionResult(+i);
                case short s:
                    return new ExpressionResult(+s);
                case byte b:
                    return new ExpressionResult(+b);
            }

            return ExpressionResult.TypeError(Op, value?.GetType());
        }
    }
}