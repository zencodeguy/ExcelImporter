using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zencodeguy.ExcelImporter.Parsers;

namespace zencodeguy.ExcelImporter.Tests.Parsers
{
    [TestClass]
    public class FileTypeParserTests
    {
        private void ParseThrowsExceptionWhenLineIsNullEquivalent(string Line)
        {
            // Arrange
            var id = new ImportDefinition();

            try
            {
                FileTypeParser.Parse(Line, id);
                Assert.Fail("ArgumentNullException expected, not thrown.");
            }
            catch(ArgumentNullException ex)
            {
                Assert.AreEqual("Line", ex.ParamName);
            }
            catch(Exception ex)
            {
                Assert.Fail("ArgumentNullException expected, " +
                    ex.GetType().Name + " thrown instead.");
            }
        }

        [TestMethod]
        public void ParseThrowsExceptionWhenLineIsNull()
        {
            ParseThrowsExceptionWhenLineIsNullEquivalent(null);
        }

        [TestMethod]
        public void ParseThrowsExceptionWhenLineIsEmptyString()
        {
            ParseThrowsExceptionWhenLineIsNullEquivalent(string.Empty);
        }

        [TestMethod]
        public void ParseThrowsExceptionWhenLineIsWhiteSpace()
        {
            ParseThrowsExceptionWhenLineIsNullEquivalent("   ");
        }

        [TestMethod]
        public void ParseThrowsExceptionWhenImportDefinitionIsNull()
        {
            // Act
            try
            {
                FileTypeParser.Parse("FILETYPE EXCEL", null);
                Assert.Fail("ArgumentNullException expected, not thrown.");
            }
            catch(ArgumentNullException ex)
            {
                Assert.AreEqual("ID", ex.ParamName);
            }
            catch(Exception ex)
            {
                Assert.Fail("ArgumentNullException expected, " +
                    ex.GetType().Name + " thrown instead.");
            }
        }

        [TestMethod]
        public void ParseThrowsExceptionWhenLineDoesNotStartWithFILETYPE()
        {
            // Arrange
            var id = new ImportDefinition();
            var line = "NOT FILETYPE";

            try
            {
                FileTypeParser.Parse(line, id);
                Assert.Fail("ArgumentException expected, not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Line is not a FILETYPE declaration.", 
                    ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("ArgumentException expected, " +
                    ex.GetType().Name + " thrown instead.");
            }
        }

        [TestMethod]
        public void ParseThrowsExceptionWhenFILETYPEDeclarationIsNotComplete()
        {
            // Arrange
            var id = new ImportDefinition();
            var line = "FILETYPE";

            try
            {
                FileTypeParser.Parse(line, id);
                Assert.Fail("ArgumentException expected, not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("FILETYPE row does not specify a file type value.",
                    ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("ArgumentException expected, " +
                    ex.GetType().Name + " thrown instead.");
            }
        }

        [TestMethod]
        public void ParseThrowsExceptionWhenFileTypeIsInvalidValue()
        {
            // Arrange
            var id = new ImportDefinition();
            var line = "FILETYPE INVALIDVALUE";

            try
            {
                FileTypeParser.Parse(line, id);
                Assert.Fail("ArgumentException expected, not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("FILETYPE declaration value of INVALIDVALUE is not a valid file type.",
                    ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("ArgumentException expected, " +
                    ex.GetType().Name + " thrown instead.");
            }
        }

        [TestMethod]
        public void ParseReturnsExcelFileType()
        {
            // Arrange
            var line = "FILETYPE EXCEL";
            var id = new ImportDefinition();

            // Act
            FileTypeParser.Parse(line, id);

            // Assert
            Assert.AreEqual(Enums.FileTypes.EXCEL, id.FileType);
        }

        [TestMethod]
        public void ParseReturnsCSVFileType()
        {
            // Arrange
            var line = "FILETYPE CSV";
            var id = new ImportDefinition();

            // Act
            FileTypeParser.Parse(line, id);

            // Assert
            Assert.AreEqual(Enums.FileTypes.CSV, id.FileType);
        }

    }
}
