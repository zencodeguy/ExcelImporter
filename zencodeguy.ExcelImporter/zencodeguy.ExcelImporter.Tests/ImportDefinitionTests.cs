using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using zencodeguy.ExcelImporter;


namespace zencodeguy.ExcelImporter.Tests
{
    [TestClass]
    public class ImportDefinitionTests
    {
        [TestMethod]
        public void ContainsSubstitutionsReturnsTrue()
        {
            // Arrange
            var id = new ImportDefinition();
            id.DestinationProperties.Add(new DestinationPropertyDefinition()
            {
                PropertyName = "APropertyName",
                DataType = Enums.DataTypes.STRING,
                Substitute = true
            });

            // Act
            var result = id.ContainsSubstitutions();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsSubstitutionsReturnsFalse()
        {
            // Arrange
            var id = new ImportDefinition();
            id.DestinationProperties.Add(new DestinationPropertyDefinition()
            {
                PropertyName = "APropertyName",
                DataType = Enums.DataTypes.STRING,
                Substitute = false
            });

            // Act
            var result = id.ContainsSubstitutions();

            // Assert
            Assert.IsFalse(result);
        }

    }
}
