using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zencodeguy.ExcelImporter.Parsers
{
    public static class HeaderRowParser
    {
        public static void Parse(string Line, ImportDefinition ID)
        {
            Helpers.ParametersValid(Line, ID, "HEADERROW");

            var tokens = Line.Split(' ');
            if(tokens.Count() != 1)
            {
                throw new ArgumentException("Header Row definition has too many tokens.");
            }

            ID.HeaderRow = true;
        }
    }
}
