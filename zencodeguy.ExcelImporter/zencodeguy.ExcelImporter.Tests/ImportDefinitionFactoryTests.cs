using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using zencodeguy.ExcelImporter.Rules;

namespace zencodeguy.ExcelImporter.Tests
{
    [TestClass]
    public class ImportDefinitionFactoryTests
    {
        #region Bad String Errors
        private void CreateThrowsExceptionOnNullEquivalent(string definition)
        {
            try
            {
                var id = ImportDefinitionFactory.Create(definition);
                Assert.Fail("Expected exception, not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("ImportDefinitionString", ex.ParamName);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected ArgumentNullException, " + ex.GetType().Name + " thrown instead.");
            }
        }
        [TestMethod]
        public void ThrowsExceptionWhenImportDefinitionStringIsNull()
        {
            CreateThrowsExceptionOnNullEquivalent(null);
        }

        [TestMethod]
        public void ThrowsExceptionWhenImportDefinitionStringIsEmptyString()
        {
            CreateThrowsExceptionOnNullEquivalent(string.Empty);
        }

        [TestMethod]
        public void ThrowsExceptionWhenImportDefinitionStringIsWhiteSpace()
        {
            CreateThrowsExceptionOnNullEquivalent("   ");
        }
        #endregion

        [TestMethod]
        public void CreateParsesHeaderRowFalse()
        {
            // Arrange
            var input = "TABLENAME customTableName;\nCOLUMN 0 MyColumnName STRING;";

            // Act
            var id = ImportDefinitionFactory.Create(input);

            // Assert
            Assert.IsTrue(id.IsValid());
            Assert.IsFalse(id.Definition.HeaderRow);
        }

        [TestMethod]
        public void CreateParsesHeaderRowTrue()
        {
            // Arrange
            var input = "TABLENAME customTableName;\nHEADERROW;\nCOLUMN 0 MyColumnName STRING;";

            // Act
            var id = ImportDefinitionFactory.Create(input);

            // Assert
            Assert.IsTrue(id.IsValid());
            Assert.IsTrue(id.Definition.HeaderRow);
        }
    }
}
