namespace BExpr.Model
{
    public abstract class Conditional<T> : IExpression<T>
    {
        public Conditional(IExpression<T> left, IExpression<T> right, string op)
        {
            Left = left;
            Right = right;
            Op = op;
        }

        public IExpression<T> Left { get; }
        public IExpression<T> Right { get; }
        public string Op { get; }
        protected abstract (bool ShortCircuit, bool Result) ShortCircuit(bool left);
        protected abstract bool Evaluate(bool left, bool right);
        public ExpressionResult Evaluate(T target, EvaluationContext context)
        {
            var leftRes = Left.Evaluate(target, context);
            if (leftRes.IsError)
            {
                return leftRes;
            }
            if (!(leftRes.Value is bool lb))
            {
                return ExpressionResult.TypeError(Op, leftRes?.GetType());
            }

            var (CanShortCircuit, Result) = ShortCircuit(lb);

            if (CanShortCircuit)
            {
                return new ExpressionResult(Result);
            }

            var rightRes = Right.Evaluate(target, context);
            if (rightRes.IsError)
            {
                return rightRes;
            }
            if(!(rightRes.Value is bool rb))
            {
                return new ExpressionResult(new TypeError(Op, rightRes?.GetType()));
            }

            return new ExpressionResult(Evaluate(lb, rb));
        }

        public override string ToString() => $"{Left} {Op} {Right}";
    }
}