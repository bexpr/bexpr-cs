namespace BExpr.Model
{
    public class Divide<T> : BinaryExpression<T>
    {
        public Divide(IExpression<T> left, IExpression<T> right)
            : base(left, right, "/") { }

        protected override ExpressionResult Evaluate(decimal left, decimal right)
            => right == 0
                ? DivideByZero()
                : new ExpressionResult(left / right);

        protected override ExpressionResult Evaluate(double left, double right)
            => right == 0 
                ? DivideByZero()
                : new ExpressionResult(left / right);
        protected override ExpressionResult Evaluate(long left, long right)
            => right == 0
                ? DivideByZero()
                : new ExpressionResult(left / right);
        protected override ExpressionResult Evaluate(int left, int right)
            => right == 0
                ? DivideByZero()
                : new ExpressionResult(left / right);

        private ExpressionResult DivideByZero()
        {
            return ExpressionResult.Error("DivideByZero", "Unable to divide by zero");
        }
    }
}