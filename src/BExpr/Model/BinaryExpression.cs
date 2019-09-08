using System;
using System.Linq;

namespace BExpr.Model
{
    public abstract class BinaryExpression<T> : BinaryExpressionBase<T>
    {
        public BinaryExpression(IExpression<T> left, IExpression<T> right, string op)
            : base(left, right, op) { }

        public override ExpressionResult Evaluate(T target, EvaluationContext context)
        {
            var leftRes = Left.Evaluate(target, context);
            var rightRes = Right.Evaluate(target, context);
            if (leftRes.IsError || rightRes.IsError)
            {
                return ExpressionResult.Error(
                    leftRes.Errors.Concat(rightRes.Errors).ToArray());
            }
            var left = leftRes.Value;
            var right = rightRes.Value;

            if (left is string || right is string)
            {
                return EvalResult(left, right, left.ToString(), right.ToString(), Evaluate);
            }

            if (left is bool ln || right is bool)
            {
                return EvalValueResult(left, right, left as bool?, right as bool?, Evaluate);
            }

            if(left is decimal || right is decimal)
            {
                var l = left.UpCastDecimal();
                var r = right.UpCastDecimal();
                return EvalValueResult(left, right, l, r, Evaluate);
            }

            if (left is double
                || right is double
                || left is float
                || right is float)
            {
                var l = left.UpCastDouble();
                var r = right.UpCastDouble();
                return EvalValueResult(left, right, l, r, Evaluate);
            }

            if (left is long || right is long)
            {
                var l = left.UpCastLong();
                var r = right.UpCastLong();
                return EvalValueResult(left, right, l, r, Evaluate);
            }

            if (left is int || right is int)
            {
                var l = left.UpCastInt();
                var r = right.UpCastInt();
                return EvalValueResult(left, right, l, r, Evaluate);
            }

            return ExpressionResult.TypeError(Op, left?.GetType(), right?.GetType());
        }

        protected virtual ExpressionResult Evaluate(decimal left, decimal right)
        {
            return ExpressionResult.TypeError(Op, typeof(decimal));
        }

        protected virtual ExpressionResult Evaluate(double left, double right)
        {
            return ExpressionResult.TypeError(Op, typeof(double));
        }

        protected virtual ExpressionResult Evaluate(long left, long right)
        {
            return ExpressionResult.TypeError(Op, typeof(int), typeof(long));
        }

        protected virtual ExpressionResult Evaluate(int left, int right)
        {
            return Evaluate((long)left, right);
        }

        protected virtual ExpressionResult Evaluate(string left, string right)
        {
            return ExpressionResult.TypeError(Op, typeof(string));
        }

        protected virtual ExpressionResult Evaluate(bool left, bool right)
        {
            return ExpressionResult.TypeError(Op, typeof(bool));
        }

        protected ExpressionResult EvalValueResult<TEval>(
            object sourceLeft,
            object sourceRight,
            TEval? left,
            TEval? right,
            Func<TEval, TEval, ExpressionResult> f) where TEval : struct
        {
            if (left == null)
            {
                return TypeError(sourceLeft.GetType());
            }
            if (right == null)
            {
                return TypeError(sourceRight.GetType());
            }

            return f(left.Value, right.Value);
        }
        protected ExpressionResult EvalResult<TEval>(
            object sourceLeft,
            object sourceRight,
            TEval left,
            TEval right,
            Func<TEval, TEval, ExpressionResult> f) where TEval : class
        {
            if (left == null)
            {
                return TypeError(sourceLeft.GetType());
            }
            if (right == null)
            {
                return TypeError(sourceRight.GetType());
            }

            return f(left, right);
        }

        private ExpressionResult TypeError(Type type)
        {
            return new ExpressionResult(new TypeError(Op, type));
        }
    }
}