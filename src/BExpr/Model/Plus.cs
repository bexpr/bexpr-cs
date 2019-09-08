﻿namespace BExpr.Model
{
    public class Plus<T> : BinaryExpression<T>
    {
        public Plus(IExpression<T> left, IExpression<T> right)
            : base(left, right, "+") { }

        protected override ExpressionResult Evaluate(decimal left, decimal right) => new ExpressionResult(left + right);
        protected override ExpressionResult Evaluate(double left, double right) => new ExpressionResult(left + right);
        protected override ExpressionResult Evaluate(long left, long right) => new ExpressionResult(left + right);
        protected override ExpressionResult Evaluate(int left, int right) => new ExpressionResult(left + right);
        protected override ExpressionResult Evaluate(string left, string right) => new ExpressionResult(left + right);
    }
}