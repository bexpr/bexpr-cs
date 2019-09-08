using System.Collections.Generic;

namespace BExpr.Model
{
    public class In<T> : RelationalOperator<T>
    {
        public In(IExpression<T> left, IExpression<T> right)
            : base(left, right, "in") { }

        protected override ExpressionResult Evaluate(object left, object right)
        {
            if(right is IList<object> rl)
            {
                return Value(rl.Contains(left));
            }
            return ExpressionResult.TypeError(Op, left?.GetType(), right?.GetType());
        }
    }
}
