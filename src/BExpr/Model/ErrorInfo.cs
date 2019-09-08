namespace BExpr.Model
{
    public class ErrorInfo
    {
        public ErrorInfo(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        public string Name { get;  }
        public string Description { get; }
    }
}