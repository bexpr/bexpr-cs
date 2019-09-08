namespace BExpr.Model
{
    public class IntLiteral<T> : Literal<T, int>
    {
        public IntLiteral(int value) : base(value) { }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}