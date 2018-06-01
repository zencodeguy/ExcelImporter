using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zencodeguy.ExcelImporter.Parsers;

namespace zencodeguy.ExcelImporter.Tests.Parsers
{
    [TestClass]
    public class HeaderRowParserTests
    {
        [TestMethod]
        public void HeaderRowParserSetsImportDefinitionHeaderRowToTrue()
        {
            // Arrange
            var id = new ImportDefinition();

            // Act
            HeaderRowParser.Parse("HEADERROW", id);

            // Assert
            Assert.IsTrue(id.HeaderRow);
        }

        private void HeaderRowParserThrowsExceptionWhenLineIsNullEquivalent(string Line)
        {
            // Arrange
            var id = new ImportDefinition();

            // Act
            try
            {
                HeaderRowParser.Parse(Line, id);
                Assert.Fail("ArgumentNullException expected, not thrown.");
            }
            catch(ArgumentNullException ex)
            {
                Assert.AreEqual("Line", ex.ParamName);
            }
            catch(Exception ex)
            {
                Assert.Fail("ArgumentNullException expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void HeaderRowParserThrowsExceptionWhenLineIsNull()
        {
            HeaderRowParserThrowsExceptionWhenLineIsNullEquivalent(null);
        }

        [TestMethod]
        public void HeaderRowParserThrowsExceptionWhenLineIsEmptyString()
        {
            HeaderRowParserThrowsExceptionWhenLineIsNullEquivalent(string.Empty);
        }

        [TestMethod]
        public void HeaderRowParserThrowsExceptionWhenLineIsWhiteSpace()
        {
            HeaderRowParserThrowsExceptionWhenLineIsNullEquivalent("   ");
        }

        [TestMethod]
        public void HeaderRowParserThrowsExceptionWhenImportDefinitionIsNull()
        {
            // Act
            try
            {
                HeaderRowParser.Parse("HEADERROW", null);
                Assert.Fail("ArgumentNullException expected, not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("ID", ex.ParamName);
            }
            catch (Exception ex)
            {
                Assert.Fail("ArgumentNullException expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void HeaderRowParserThrowsExceptionWhenLineHasTooManyTokens()
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "HEADERROW Has too many tokens";

            // Act
            try
            {
                HeaderRowParser.Parse(s, id);
                Assert.Fail("ArgumentException expected, not thrown.");
            }
            catch(ArgumentException ex)
            {
                Assert.AreEqual("Header Row definition has too many tokens.",
                    ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("ArgumentException expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        public void HeaderRowParserThrowsExceptionWhenLineisNotHeaderRowDeclaration()
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "NOTAHEADERROW";

            // Act
            try
            {
                HeaderRowParser.Parse(s, id);
                Assert.Fail("ArgumentException expected, not thrown.");
            }
            catch(ArgumentException ex)
            {
                Assert.AreEqual("Passed in string is not a HEADERROW declaration.",
                    ex.Message);
            }
            catch(Exception ex)
            {
                Assert.Fail("ArgumentException expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }
    }
}
