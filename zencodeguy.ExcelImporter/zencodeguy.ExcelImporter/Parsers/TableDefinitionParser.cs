using System;

namespace zencodeguy.ExcelImporter.Parsers
{
    public static class TableDefinitionParser
    {
        public static void Parse(string Line, ImportDefinition ID)
        {
            Helpers.ParametersValid(Line, ID, "TABLENAME");

            if(string.IsNullOrWhiteSpace(Line))
            {
                throw new ArgumentNullException("Line");
            }

            if(ID == null)
            {
                throw new ArgumentNullException("ID");
            }

            if(!Line.StartsWith("TABLENAME"))
            {
                throw new ArgumentException("Line is not a TABLENAME declaration.");
            }

            var tokens = Line.Split(' ');
            if (tokens.Length > 2)
            {
                throw new ArgumentException("TABLE definition has too many tokens: " + Line);
            }
            if(tokens.Length < 2)
            {
                throw new ArgumentException("TABLE definition has too few tokens: " + Line);
            }

            ID.TableName = tokens[1].Trim();
        }
    }
}
