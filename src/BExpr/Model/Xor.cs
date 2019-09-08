namespace BExpr.Model
{
    public class Xor<T> : BinaryExpression<T>
    {
        public Xor(IExpression<T> left, IExpression<T> right) : base(left, right, "xor") { }

        protected override ExpressionResult Evaluate(bool left, bool right)
            => new ExpressionResult(left ^ right);
    }
}