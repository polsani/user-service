namespace UserService.Data.Annotations
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class BulkInsertColumnName : System.Attribute
    {
        public string ColumnName { get; }

        public BulkInsertColumnName(string columnName)
        {
            ColumnName = columnName;
        }
    }
}