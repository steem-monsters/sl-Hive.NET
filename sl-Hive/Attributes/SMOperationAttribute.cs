namespace sl_Hive.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SMOperationAttribute : Attribute
    {
        public string Id { get; }

        public SMOperationAttribute(string id)
        {
            Id = id;
        }
    }
}
