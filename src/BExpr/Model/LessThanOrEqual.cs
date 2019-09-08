using System;

namespace BExpr.Model
{
    public class LessThanOrEqual<T> : RelationalOperator<T>
    {
        public LessThanOrEqual(IExpression<T> left, IExpression<T> right)
            : base(left, right, "<=") { }

        protected override ExpressionResult Evaluate(object left, object right)
        {
            if (left is IComparable lc && right is IComparable rc)
            {
                return Value(lc.CompareTo(rc) <= 0);
            }
            return ExpressionResult.TypeError(Op, left?.GetType(), right?.GetType());
        }
    }
}
