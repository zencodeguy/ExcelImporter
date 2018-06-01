using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zencodeguy.ExcelImporter;
using zencodeguy.ExcelImporter.Parsers;

namespace zencodeguy.ExcelImporter.Tests.Parsers
{
    [TestClass]
    public class TableDefinitionParserTests
    {
        [TestMethod]
        public void ParseTableNameAddsTableNameToImportDefinition()
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "TABLENAME customTableName";

            // Act
            TableDefinitionParser.Parse(s, id);

            // Assert
            Assert.AreEqual("customTableName", id.TableName);
        }

        private void ParseTableNameThrowsExceptionWhenLineIsNullEquivalent(string line)
        {
            // Arrange
            var id = new ImportDefinition();

            try
            {
                TableDefinitionParser.Parse(line, id);
                Assert.Fail("ArgumentNullException expected, not thrown.");
            }
            catch(ArgumentNullException ex)
            {
                Assert.AreEqual("Line", ex.ParamName);
            }
            catch(Exception ex)
            {
                Assert.Fail("Expected ArgumentNullException, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void ParseTableNameThrowsExceptionWhenLineIsNull()
        {
            ParseTableNameThrowsExceptionWhenLineIsNullEquivalent(null);
        }

        [TestMethod]
        public void ParseTableNameThrowsExceptionWhenLineIsEmptyString()
        {
            ParseTableNameThrowsExceptionWhenLineIsNullEquivalent(string.Empty);
        }

        [TestMethod]
        public void ParseTableNameThrowsExceptionWhenLineIsWhiteSpace()
        {
            ParseTableNameThrowsExceptionWhenLineIsNullEquivalent("   ");
        }

        [TestMethod]
        public void ParsetableNameThrowsExceptionWhenImportDefinitionIsNull()
        {
            try
            {
                TableDefinitionParser.Parse("A line", null);
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
        public void ParseTableNameThrowsExceptionWhenTooManyTokens()
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "TABLENAME too many tokens";

            // Act
            try
            {
                TableDefinitionParser.Parse(s, id);
                Assert.Fail("ArgumentException expected, not thrown.");
            }
            catch(ArgumentException ex)
            {
                Assert.AreEqual("TABLE definition has too many tokens: " + s,
                    ex.Message);
            }
            catch(Exception ex)
            {
                Assert.Fail("ArgumentException expected, " +
                    ex.GetType().ToString() +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void ParseTableNameThrowsExceptionWhenTooFewTokens()
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "TABLENAME";

            // Act
            try
            {
                TableDefinitionParser.Parse(s, id);
                Assert.Fail("InvalidOperationException expected, not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("TABLE definition has too few tokens: " + s,
                    ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("ArgumentException expected, " +
                    ex.GetType().ToString() +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void ParseTableNameThrowsExceptionWhenLineIsNotTableNameDeclaration()
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "NOTATABLE";

            // Act
            try
            {
                TableDefinitionParser.Parse(s, id);
                Assert.Fail("ArgumentException expected, not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Line is not a TABLENAME declaration.", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("ArgumentException expected, " +
                    ex.GetType().ToString() +
                    " thrown instead.");
            }

        }
    }
}
