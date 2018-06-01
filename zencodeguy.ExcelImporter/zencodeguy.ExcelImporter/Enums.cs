namespace zencodeguy.ExcelImporter
{
    public class Enums
    {
        public enum ComparisonOperators
        {
            EQ,
            NOTEQ,
            IN,
            NOTIN
        }
        public enum DataTypes
        {
            BOOLEAN,
            BYTE,
            CHAR,
            DATETIME,
            DECIMAL,
            DOUBLE,
            GUID,
            INT16,
            INT32,
            INT64,
            STRING
        }

        public enum FileTypes
        {
            CSV,
            EXCEL
        }
    }
}
