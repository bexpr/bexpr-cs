using System.Text.RegularExpressions;

namespace BExpr.Model
{
    public class Like<T> : RelationalOperator<T>
    {
        public Like(IExpression<T> left, IExpression<T> right)
            : base(left, right, "like") { }

        protected override ExpressionResult Evaluate(object left, object right)
        {
            if (left is string ls && right is string rs)
            {
                var likeRegex = "^" + Regex.Escape(rs).Replace("%", ".*") + "$";
                var regex = new Regex(likeRegex);
                return Value(regex.IsMatch(ls));
            }
            return ExpressionResult.TypeError(Op, left?.GetType(), right?.GetType());
        }
    }
}
