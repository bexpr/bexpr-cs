using System;
using System.Collections.Generic;

namespace BExpr.Model
{
    public interface IMethodProvider<T>
    {
        bool HasMethod(T target, string name);
        Func<IReadOnlyList<object>, ExpressionResult> GetMethod(T target, string name);
    }
}