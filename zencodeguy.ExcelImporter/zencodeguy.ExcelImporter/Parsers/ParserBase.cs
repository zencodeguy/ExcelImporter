using System;

namespace zencodeguy.ExcelImporter.Parsers
{
    public static class ParserBase
    {
        public static bool ParametersValid(string Line, ImportDefinition ID, string ColumnName)
        {
            if (string.IsNullOrWhiteSpace(Line))
            {
                throw new ArgumentNullException("Line");
            }

            if (ID == null)
            {
                throw new ArgumentNullException("ID");
            }

            if (!Line.StartsWith("COLUMN"))
            {
                throw new ArgumentException("Line is not a Column declaration.");
            }

            return true;
        }
    }
}
