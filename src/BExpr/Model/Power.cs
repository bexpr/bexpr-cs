using System;

namespace BExpr.Model
{
    public class Power<T> : IExpression<T>
    {
        public Power(IExpression<T> @base, IExpression<T> exponent)
        {
            this.Base = @base;
            this.Exponent = exponent;
        }

        public IExpression<T> Base { get; }
        public IExpression<T> Exponent { get; }

        public ExpressionResult Evaluate(T target, EvaluationContext context)
        {
            var baseRes = Base.Evaluate(target, context);
            var exponentRes = Exponent.Evaluate(target, context);

            if (baseRes.IsError || exponentRes.IsError)
            {
                return ExpressionResult.Error(baseRes, exponentRes);
            }

            var baseValue = baseRes.Value.CastDouble();
            var exponentValue = exponentRes.Value.CastDouble();
            
            if(baseValue == null || exponentValue == null)
            {
                return ExpressionResult.TypeError("^", baseRes.Value?.GetType(), exponentRes.Value?.GetType());
            }

            var result = Math.Pow(baseValue.Value, exponentValue.Value);
            return new ExpressionResult(result);
        }

        public override string ToString()
        {
            return $"{Base} ^ {Exponent}";
        }
    }
}