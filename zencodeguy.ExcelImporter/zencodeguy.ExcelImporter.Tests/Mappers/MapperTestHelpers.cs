using System.Collections.Generic;
using System.Text;

namespace zencodeguy.ExcelImporter.Tests.Mappers
{
    public static class MapperTestHelpers
    {
        public static ImportDefinition CreateImportDefinition()
        {
            var sb = new StringBuilder();
            sb.AppendLine("TABLENAME TestTable;")
                .AppendLine("COLUMN 0 ABoolean BOOLEAN;")
                .AppendLine("COLUMN 1 AByte BYTE;")
                .AppendLine("COLUMN 2 ACharacter CHAR;")
                .AppendLine("COLUMN 3 ADateTime DATETIME;")
                .AppendLine("COLUMN 4 ADecimal DECIMAL;")
                .AppendLine("COLUMN 5 ADouble DOUBLE;")
                .AppendLine("COLUMN 6 AGuid GUID;")
                .AppendLine("COLUMN 7 AnInt16 INT16;")
                .AppendLine("COLUMN 8 AnInt32 INT32;")
                .AppendLine("COLUMN 9 AnInt64 INT64;")
                .AppendLine("COLUMN 10 AString STRING;")
                .AppendLine("DESTINATION DestinationBoolean BOOLEAN SET {true};")
                .AppendLine("DESTINATION DestinationByte BYTE SET {254};")
                .AppendLine("DESTINATION DestinationChar CHAR SET {a};")
                .AppendLine("DESTINATION DestinationDateTime DATETIME SET {2018-01-01T05:00:00Z};")
                .AppendLine("DESTINATION DestinationDecimal DECIMAL SET {123.456};")
                .AppendLine("DESTINATION DestinationDouble DOUBLE SET {1.23};")
                .AppendLine("DESTINATION DestinationGuid GUID SET {4732B277-11B3-440D-9327-1E2E6613746C};")
                .AppendLine("DESTINATION DestinationINT16 INT16 SET {32767};")
                .AppendLine("DESTINATION DestinationINT32 INT32 SET {2147483647};")
                .AppendLine("DESTINATION DestinationINT64 INT64 SET {9223372036854775807};")
                .AppendLine("DESTINATION DestinationString STRING SET {This is a string.};")
                .AppendLine("DESTINATION DestinationSubstituteBoolean BOOLEAN SUBSTITUTE {boolean};")
                .AppendLine("DESTINATION DestinationSubstituteByte BYTE SUBSTITUTE {byte};")
                .AppendLine("DESTINATION DestinationSubstituteChar CHAR SUBSTITUTE {char};")
                .AppendLine("DESTINATION DestinationSubstituteDateTime DATETIME SUBSTITUTE {datetime};")
                .AppendLine("DESTINATION DestinationSubstituteDecimal DECIMAL SUBSTITUTE {decimal};")
                .AppendLine("DESTINATION DestinationSubstituteDouble DOUBLE SUBSTITUTE {double};")
                .AppendLine("DESTINATION DestinationSubstituteGuid GUID SUBSTITUTE {guid};")
                .AppendLine("DESTINATION DestinationSubstituteInt16 INT16 SUBSTITUTE {int16};")
                .AppendLine("DESTINATION DestinationSubstituteInt32 INT32 SUBSTITUTE {int32};")
                .AppendLine("DESTINATION DestinationSubstituteInt64 INT64 SUBSTITUTE {int64};")
                .AppendLine("DESTINATION DestinationSubstituteString STRING SUBSTITUTE {string};")
                .AppendLine("DESTINATION DestinationGenerateGuid GUID GENERATE;")
                .AppendLine("DESTINATION DestinationGenerateDateOnly DATETIME GENERATE DATEONLY;")
                .AppendLine("DESTINATION DestinationGenerateDateTime DATETIME GENERATE;");
            var importDefinitionString = sb.ToString();
            var id = ImportDefinitionFactory.Create(importDefinitionString);
            return id.Definition;
        }

        public static ImportResult CreateImportResult()
        {
            var ir = new ImportResult();
            var importedRow = new ImportedRow();
            importedRow.Columns.Add("ABoolean", "true");
            importedRow.Columns.Add("AByte", "1");
            importedRow.Columns.Add("ACharacter", "z");
            importedRow.Columns.Add("ADateTime", "1929-06-12T11:30:15Z");
            importedRow.Columns.Add("ADecimal", "654.321");
            importedRow.Columns.Add("ADouble", "-123.456");
            importedRow.Columns.Add("AGuid", "C506A057-9EAD-422D-BADE-C5E3E1A97F62");
            importedRow.Columns.Add("AnInt16", "-32768");
            importedRow.Columns.Add("AnInt32", "-2147483648");
            importedRow.Columns.Add("AnInt64", "-9223372036854775808");
            importedRow.Columns.Add("AString", "This is an imported string.");
            ir.Rows.Add(importedRow);
            return ir;
        }

        public static Dictionary<string, string> CreateSubstitutions()
        {
            var substitutions = new Dictionary<string, string>()
            {
                { "boolean", "true" },
                {"byte", "100"},
                {"char", "G"},
                {"datetime", "1776-07-04T12:00:00Z"},
                {"decimal", "987.123"},
                {"double", "852.147"},
                {"guid", "C02070CE-698B-4DC3-8D67-C182A772AE7D"},
                {"int16", "32000"},
                {"int32", "2147483000"},
                {"int64", "9223372036000000000"},
                {"string", "This is a substituted string."}
            };

            return substitutions;
        }
    }
}
