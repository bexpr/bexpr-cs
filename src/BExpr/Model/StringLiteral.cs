namespace BExpr.Model
{
    public class StringLiteral<T> : Literal<T, string>
    {
        public StringLiteral(string value) : base(value) { }

        public override string ToString()
        {
            return $"'{Value}'";
        }
    }
}