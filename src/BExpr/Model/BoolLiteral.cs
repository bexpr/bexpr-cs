namespace BExpr.Model
{
    public class BoolLiteral<T> : Literal<T, bool>
    {
        public BoolLiteral(bool value) : base(value) { }
        
        public override string ToString()
        {
            return Value ? "true" : "false";
        }
    }
}