using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using zencodeguy.ExcelImporter;

namespace zencodeguy.ExcelImporter.Tests
{
    [TestClass]
    public class DestinationPropertyDefinitionTests
    {
        [TestMethod]
        public void GetValueToSetReturnsGeneratedGuid()
        {
            // Arrange
            var dp = new DestinationPropertyDefinition()
            {
                PropertyName = "APropertyName",
                DataType = Enums.DataTypes.GUID,
                Generate = true
            };

            // Act
            var guidValueAsString = dp.GetValueToSet();

            // Assert
            Assert.IsTrue(Guid.TryParse(guidValueAsString, out Guid result));
        }

        [TestMethod]
        public void GetValuetoSetReturnsGeneratedDateTime()
        {
            // Arrange
            var dp = new DestinationPropertyDefinition()
            {
                PropertyName = "APropertyName",
                DataType = Enums.DataTypes.DATETIME,
                Generate = true
            };

            // Act
            var dateTimeAsString = dp.GetValueToSet();

            // Assert
            Assert.IsTrue(dateTimeAsString.EndsWith("Z"));
            Assert.IsTrue(DateTime.TryParse(dateTimeAsString, out DateTime result));
        }

        [TestMethod]
        public void GetValuetoSetReturnsGeneratedDateOnly()
        {
            // Arrange
            var dp = new DestinationPropertyDefinition()
            {
                PropertyName = "APropertyName",
                DataType = Enums.DataTypes.DATETIME,
                Generate = true,
                GenerateDateOnly = true
            };

            // Act
            var dateTimeAsString = dp.GetValueToSet();

            // Assert
            Assert.IsFalse(dateTimeAsString.EndsWith("Z"));
            Assert.AreEqual(10, dateTimeAsString.Length);
            Assert.IsTrue(DateTime.TryParse(dateTimeAsString, out DateTime result));
        }

        public void GetValueToSetReturnsSetGuid()
        {
            // Arrange
            var gValue = Guid.NewGuid();
            var dp = new DestinationPropertyDefinition()
            {
                PropertyName = "APropertyName",
                DataType = Enums.DataTypes.GUID,
                SetValue = gValue.ToString()
            };

            // Act
            var gValueAsString = dp.GetValueToSet();

            //Assert
            Assert.AreEqual(gValue.ToString(), gValueAsString);
        }

        public void GetValueToSetReturnsValue()
        {
            // Arrange
            var value = "This is a value";
            var dp = new DestinationPropertyDefinition()
            {
                PropertyName = "APropertyName",
                DataType = Enums.DataTypes.STRING,
                SetValue = value
            };

            // Act
            var returnedValue = dp.GetValueToSet();

            // Assert
            Assert.AreEqual(value, returnedValue);
        }
    }
}
