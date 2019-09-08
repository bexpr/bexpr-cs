namespace BExpr.Model
{
    public class DoubleLiteral<T> : Literal<T, double>
    {
        public DoubleLiteral(double value) : base(value) { }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}