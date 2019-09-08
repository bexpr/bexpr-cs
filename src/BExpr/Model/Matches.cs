using System.Text.RegularExpressions;

namespace BExpr.Model
{
    public class Matches<T> : RelationalOperator<T>
    {
        public Matches(IExpression<T> left, IExpression<T> right)
            : base(left, right, "matches") { }

        protected override ExpressionResult Evaluate(object left, object right)
        {
            if(left is string ls && right is string rs)
            {
                var regex = new Regex(rs);
                return Value(regex.IsMatch(ls));
            }
            return ExpressionResult.TypeError(Op, left?.GetType(), right?.GetType());
        }
    }
}
