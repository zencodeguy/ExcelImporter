
namespace zencodeguy.ExcelImporter
{
    public sealed class ColumnDefinition
    {
        public int ColumnNumber { get; set; }
        public string PropertyName { get; set; }
        public Enums.DataTypes DataType { get; set; }
    }
}
