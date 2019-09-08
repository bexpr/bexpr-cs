using System;
using System.Collections.Generic;

namespace BExpr.Model
{
    public class Function<T> : FunctionBase<T>
    {
        public Function(string name, IExpression<T> args) : base(name, args) { }

        protected override Func<T, IReadOnlyList<object>, EvaluationContext, ExpressionResult> EvalFunc
            => (_, args, context)
                => context.HasFunction(Name)
                    ? context.GetFunction(Name)(args)
                    : ExpressionResult.Error("MissingFunction", $"Function '{Name}' was not found in the evaluation context");

        public override string ToString()
        {
            return "#" + base.ToString();
        }
    }
}