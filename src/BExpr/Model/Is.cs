using System;

namespace BExpr.Model
{
    public class Is<T> : RelationalOperator<T>
    {
        public Is(IExpression<T> left, IExpression<T> right)
            : base(left, right, "is") { }

        protected override ExpressionResult Evaluate(object left, object right)
        {
            if(right is Type type)
            {
                return Value(right != null && type.IsAssignableFrom(right.GetType()));
            }

            return ExpressionResult.TypeError(Op, left?.GetType(), right?.GetType());
        }
    }
}
