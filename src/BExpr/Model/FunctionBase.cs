using System;
using System.Collections.Generic;

namespace BExpr.Model
{
    public abstract class FunctionBase<T> : IExpression<T>
    {
        protected FunctionBase(string name, IExpression<T> args)
        {
            Name = name;
            Args = args;
        }

        public string Name { get; }
        public IExpression<T> Args { get; }

        public ExpressionResult Evaluate(T target, EvaluationContext context)
        {
            var args = Args.Evaluate(target, context);
            
            if(args.IsError)
            {
                return ExpressionResult.Error(args);
            }

            if (args.Value is IReadOnlyList<object> funcArgs)
            {
                return EvalFunc(target, funcArgs, context);
            }

            return ExpressionResult.TypeError("FunctionArgs", args.Value?.GetType());
        }

        protected abstract Func<T, IReadOnlyList<object>, EvaluationContext, ExpressionResult> EvalFunc { get; }

        public override string ToString()
        {
            return $"{Name}({Args})";
        }
    }
}