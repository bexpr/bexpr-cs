namespace BExpr.Model
{
    public class NotEquals<T> : RelationalOperator<T>
    {
        public NotEquals(IExpression<T> left, IExpression<T> right)
            : base(left, right, "!=") { }

        protected override ExpressionResult Evaluate(object left, object right)
        {
            return Value(!Equals(left, right));
        }
    }
}
