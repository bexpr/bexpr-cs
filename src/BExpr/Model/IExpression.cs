namespace BExpr.Model
{
    public interface IExpression<T>
    {
        ExpressionResult Evaluate(T target, EvaluationContext context);
    }
}