using System;
using System.Collections.Generic;

namespace BExpr.Model
{
    public class Between<T> : RelationalOperator<T>
    {
        public Between(IExpression<T> left, IExpression<T> right)
            : base(left, right, "between") { }

        protected override ExpressionResult Evaluate(object left, object right)
        {
            if(right is IList<object> rl && left is IComparable lc)
            {
                if(rl.Count != 2)
                {
                    return ExpressionResult.Error(
                        "ParameterMismatch", 
                        "Between operator expects an array of exactly two elements on the right side");
                }

                var first = rl[0];
                var second = rl[1];
                if(first is IComparable fc && second is IComparable sc)
                {
                    return Value(lc.CompareTo(fc) >= 0 && lc.CompareTo(sc) <= 0);
                }
                return ExpressionResult.TypeError(Op, first?.GetType(), second?.GetType());
            }

            return ExpressionResult.TypeError(Op, left?.GetType(), right?.GetType());
        }
    }
}
