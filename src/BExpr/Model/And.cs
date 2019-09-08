namespace BExpr.Model
{
    public class And<T> : Conditional<T>
    {
        public And(IExpression<T> left, IExpression<T> right) : base(left, right, "and") {  }
        protected override bool Evaluate(bool left, bool right) => left & right;
        protected override (bool ShortCircuit, bool Result) ShortCircuit(bool left) => (!left, false);
    }
}