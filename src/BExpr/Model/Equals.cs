namespace BExpr.Model
{
    public class Equals<T> : RelationalOperator<T>
    {
        public Equals(IExpression<T> left, IExpression<T> right)
            : base(left, right, "==") { }

        protected override ExpressionResult Evaluate(object left, object right)
        {
            return Value(Equals(left, right));
        }
    }
}
