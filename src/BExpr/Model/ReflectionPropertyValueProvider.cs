namespace BExpr.Model
{
    public class ReflectionPropertyValueProvider<T> : IPropertyValueProvider<T>
    {
        public object GetValue(T target, string name)
        {
            return target.GetType().GetProperty(name).GetValue(target);
        }

        public bool HasValue(T target, string name)
        {
            return target.GetType().GetProperty(name) != null;
        }
    }
}