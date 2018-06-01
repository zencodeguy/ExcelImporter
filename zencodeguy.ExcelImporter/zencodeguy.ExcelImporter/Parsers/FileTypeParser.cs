using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zencodeguy.ExcelImporter.Parsers
{
    public static class FileTypeParser
    {
        public static void Parse(string Line, ImportDefinition ID)
        {
            Helpers.ParametersValid(Line, ID, "FILETYPE");

            var tokens = Line.Split(' ');
            if (tokens.Count() == 1)
            {
                throw new ArgumentException("FILETYPE row does not specify a file type value.");
            }


            switch(tokens[1].Trim().ToUpper())
            {
                case "EXCEL":
                    ID.FileType = Enums.FileTypes.EXCEL;
                    break;
                case "CSV":
                    ID.FileType = Enums.FileTypes.CSV;
                    break;
                default:
                    throw new ArgumentException("FILETYPE declaration value of " +
                        tokens[1] + " is not a valid file type.");
            }
        }
    }
}
