using System;
using System.Collections.Generic;

namespace BExpr.Model
{
    public class Method<T> : FunctionBase<T>
    {
        private readonly IMethodProvider<T> methodProvider;

        public Method(string name, IExpression<T> args, IMethodProvider<T> methodProvider) : base(name, args) {
            this.methodProvider = methodProvider;
        }

        protected override Func<T, IReadOnlyList<object>, EvaluationContext, ExpressionResult> EvalFunc
            => (target, args, context)
                => methodProvider.HasMethod(target, Name)
                    ? methodProvider.GetMethod(target, Name)(args)
                    : ExpressionResult.Error("MissingFunction", $"Function '{Name}' was not found in the evaluation context");

    }
}