namespace BExpr.Model
{
    public class Modulus<T> : BinaryExpression<T>
    {
        public Modulus(IExpression<T> left, IExpression<T> right)
            : base(left, right, "%") { }

        protected override ExpressionResult Evaluate(decimal left, decimal right) => new ExpressionResult(left % right);
        protected override ExpressionResult Evaluate(double left, double right) => new ExpressionResult(left % right);
        protected override ExpressionResult Evaluate(long left, long right) => new ExpressionResult(left % right);
        protected override ExpressionResult Evaluate(int left, int right) => new ExpressionResult(left % right);
    }
}