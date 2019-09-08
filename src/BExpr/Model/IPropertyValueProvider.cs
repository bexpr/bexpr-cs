namespace BExpr.Model
{

    public interface IPropertyValueProvider<T>
    {
        bool HasValue(T target, string name);
        object GetValue(T target, string name);
    }
}