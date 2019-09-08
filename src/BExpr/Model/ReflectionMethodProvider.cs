using System;
using System.Collections.Generic;
using System.Linq;

namespace BExpr.Model
{
    internal class ReflectionMethodProvider<T> : IMethodProvider<T>
    {
        public Func<IReadOnlyList<object>, ExpressionResult> GetMethod(T target, string name)
        {
            var method = target.GetType().GetMethod(name);
            var parameters = method.GetParameters();

            return args => parameters.Length != args.Count
                ? ExpressionResult.Error(
                    "ArgumentLengthMismatch", 
                    $"Expected {parameters.Length} arguments to {name} method got {args.Count}")
                : Wrap(name, () => method.Invoke(target, args.ToArray()));
        }

        private ExpressionResult Wrap(string name, Func<object> invoke)
        {
            try
            {
                var result = invoke();
                if (result is ExpressionResult er)
                    return er;

                return new ExpressionResult(result);
            }
            catch(Exception ex)
            {
                return ExpressionResult.Error("MethodEvalError", $"Exception evaluating method '{name}', '{ex.Message}'");
            }

        }

        public bool HasMethod(T target, string name)
        {
            return target.GetType().GetMethod(name) != null;
        }
    }
}