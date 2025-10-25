namespace __SolutionName__.Api.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ValidationModelAttribute : Attribute
    {
        public Type ModelType { get; }

        public ValidationModelAttribute(Type modelType)
        {
            ModelType = modelType;
        }
    }
}
