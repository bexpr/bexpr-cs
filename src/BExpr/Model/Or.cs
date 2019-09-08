namespace BExpr.Model
{
    public class Or<T> : Conditional<T>
    {
        public Or(IExpression<T> left, IExpression<T> right) : base(left, right, "or") { }
        protected override bool Evaluate(bool left, bool right) => left & right;
        protected override (bool ShortCircuit, bool Result) ShortCircuit(bool left) => (left, true);

    }
}