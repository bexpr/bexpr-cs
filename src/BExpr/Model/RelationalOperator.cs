namespace BExpr.Model
{
    public abstract class RelationalOperator<T> : BinaryExpressionBase<T>
    {
        public RelationalOperator(IExpression<T> left, IExpression<T> right, string op)
            : base(left, right, op) {  }

        public override ExpressionResult Evaluate(T target, EvaluationContext context)
        {
            var lr = Left.Evaluate(target, context);
            var rr = Right.Evaluate(target, context);

            if(lr.IsError || rr.IsError)
            {
                return ExpressionResult.Error(lr, rr);
            }

            var result = Evaluate(lr.Value, rr.Value);
            return result;
        }

        protected abstract ExpressionResult Evaluate(object left, object right);
    }
}