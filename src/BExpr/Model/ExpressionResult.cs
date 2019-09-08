using System;
using System.Collections.Generic;
using System.Linq;

namespace BExpr.Model
{
    public class ExpressionResult
    {
        public ExpressionResult(object value)
        {
            this.Value = value;
            this.IsError = false;
        }

        internal ExpressionResult(ErrorInfo error)
        {
            this.Errors = new[] { error };
            this.IsError = true;
        }

        internal ExpressionResult(IReadOnlyList<ErrorInfo> errors)
        {
            this.Errors = errors;
            this.IsError = true;
        }

        public bool IsError { get; }

        public object Value { get; }

        public IReadOnlyList<ErrorInfo> Errors { get; }

        public static ExpressionResult Error(string title, string message)
        {
            return new ExpressionResult(new ErrorInfo(title, message));
        }

        public static ExpressionResult Error(params ErrorInfo[] errors)
        {
            return new ExpressionResult(errors);
        }

        public static ExpressionResult Error(params ExpressionResult[] results)
        {
            return new ExpressionResult(results.SelectMany(r => r.Errors).ToArray());
        }

        public static ExpressionResult TypeError(string op, params Type[] types)
        {
            return Error(new TypeError(op, types));
        }
    }
}