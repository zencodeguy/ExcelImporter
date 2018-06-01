using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using zencodeguy.ExcelImporter;

namespace zencodeguy.ExcelImporter.Tests
{
    [TestClass]
    public class ImportedRowTests
    {
        private ImportDefinition CreateImportDefinition()
        {
            var sb = new StringBuilder();
            sb.AppendLine("TABLE SomeTableName;");
            sb.AppendLine("COLUMN 0 Boolean BOOLEAN;");
            sb.AppendLine("COLUMN 1 Byte BYTE;");
            sb.AppendLine("COLUMN 2 Char CHAR;");
            sb.AppendLine("COLUMN 3 Date DATETIME;");
            sb.AppendLine("COLUMN 4 Decimal DECIMAL;");
            sb.AppendLine("COLUMN 5 Double DOUBLE;");
            sb.AppendLine("COLUMN 6 UniqueIdentifier GUID;");
            sb.AppendLine("COLUMN 7 16-bit-Integer INT16;");
            sb.AppendLine("COLUMN 8 32-bit-Integer INT32;");
            sb.AppendLine("COLUMN 9 64-bit-Integer INT64;");
            sb.AppendLine("COLUMN 10 String STRING;");

            return ImportDefinitionFactory.Create(sb.ToString()).Definition;
        }
        
        private string[] CreateImportRecordArray()
        {
            string[] values = new string[11];
            values[0] = "true";
            values[1] = "255";
            values[2] = "A";
            values[3] = "01/02/2003";
            values[4] = "123.456";
            values[5] = "12.12345678";
            values[6] = "084EDBC0-081F-4378-B539-7629F8CC26EA";
            values[7] = "32766";
            values[8] = "2147483646";
            values[9] = "9223372036854775806";
            values[10] = "This is a string.";

            return values;
        }

        private string[] CreateInvalidImportRecordArray()
        {
            string[] values = new string[11];
            values[0] = "not a bool";
            values[1] = "not a byte";
            values[2] = "Too Many Characters";
            values[3] = "No date here";
            values[4] = "Not a decimal";
            values[5] = "Not a double";
            values[6] = "Not a guid";
            values[7] = "3.1415968";
            values[8] = "123.123456";
            values[9] = "123456.123456";
            values[10] = "Still a string?";

            return values;

        }

        [TestMethod]
        public void ImportedRowPopulatesCorrectly()
        {
            // Arrange
            var id = CreateImportDefinition();
            var values = CreateImportRecordArray();

            // Act
            var row = new ImportedRow(0, values, id);

            // Assert
            Assert.AreEqual(0, row.RowNumber);
            Assert.AreEqual(0, row.ErrorMessages.Count);
            Assert.AreEqual(11, row.Columns.Count);

            for(var i = 0; i < values.Length; i++)
            {
                Assert.AreEqual(values[i],
                    row.Columns[id.Columns[i].PropertyName]);
            }
        }

        [TestMethod]
        public void ValuesDoNotMatchDataType()
        {
            // Arrange
            var id = CreateImportDefinition();
            var values = CreateInvalidImportRecordArray();

            // Act
            var row = new ImportedRow(0, values, id);

            // Assert
            Assert.AreEqual(10, row.ErrorMessages.Count);
        }
    }
}

