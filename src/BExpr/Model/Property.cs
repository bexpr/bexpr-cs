namespace BExpr.Model
{
    public class Property<T> : IExpression<T>
    {
        private readonly IPropertyValueProvider<T> propertyValueProvider;

        public Property(string name, IPropertyValueProvider<T> propertyValueProvider)
        {
            Name = name;
            this.propertyValueProvider = propertyValueProvider;
        }

        public string Name { get; }

        public ExpressionResult Evaluate(T target, EvaluationContext context)
        {
            if(propertyValueProvider.HasValue(target, this.Name))
            {
                var value = propertyValueProvider.GetValue(target, this.Name);
                return new ExpressionResult(value);
            }
            return ExpressionResult.Error("MissingProperty", $"Property {this.Name} not found");
        }

        public override string ToString()
        {
            return Name;
        }
    }
}