using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zencodeguy.ExcelImporter.Parsers;

namespace zencodeguy.ExcelImporter.Tests.Parsers
{
    [TestClass]
    public class DestinationParserTests
    {
        private void DestinationParserThrowsExceptionWhenLineIsNullEquivalent(string line)
        {
            // Arrange
            var id = new ImportDefinition();

            // Act
            try
            {
                DestinationParser.Parse(line, id);
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
        public void DestinationParserThrowsExceptionWhenLineIsNull()
        {
            DestinationParserThrowsExceptionWhenLineIsNullEquivalent(null);
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenLineIsEmpty()
        {
            DestinationParserThrowsExceptionWhenLineIsNullEquivalent(string.Empty);
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenLineIsWhiteSpace()
        {
            DestinationParserThrowsExceptionWhenLineIsNullEquivalent("   ");
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenImportDefinitionIsNull()
        {
            // Act
            try
            {
                DestinationParser.Parse("DESTINATION ColumnName STRING", null);
                Assert.Fail("Expected ArgumentNullException, not thrown.");
            }
            catch(ArgumentNullException ex)
            {
                Assert.AreEqual("ID", ex.ParamName);
            }
            catch(Exception ex)
            {
                Assert.Fail("Expected ArgumentNullException, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenLineNotADestination()
        {
            try
            {
                DestinationParser.Parse("COLUMN 0 MyColumn STRING", new ImportDefinition());
                Assert.Fail("Expected ArgumentException, not thrown.");
            }
            catch(ArgumentException ex)
            {
                Assert.AreEqual("Line is not a DESTINATION declaration.", ex.Message);
            }
            catch(Exception ex)
            {
                Assert.Fail("Expected ArgumentException, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenLineHasTooFewTokens()
        {
            try
            {
                DestinationParser.Parse("DESTINATION PropertyName", new ImportDefinition());
                Assert.Fail("Expected ArgumentException, not thrown.");
            }
            catch(ArgumentException ex)
            {
                Assert.AreEqual("Destination definition line must contain at least 4 tokens.", ex.Message);
            }
            catch(Exception ex)
            {
                Assert.Fail("Expected ArgumentException, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionOnDuplicatePropertyName()
        {
            // Arrange
            var id = new ImportDefinition();
            id.DestinationProperties.Add(new DestinationPropertyDefinition()
            {
                PropertyName = "APropertyName",
                DataType = Enums.DataTypes.STRING
            });

            // Act
            try
            {
                DestinationParser.Parse("DESTINATION APropertyName STRING SET {A Value}", id);
                Assert.Fail("InvalidOperationException expected, not thrown.");
            }
            catch(InvalidOperationException ex)
            {
                Assert.AreEqual("A destination property named APropertyName is defined more than once.", ex.Message);
            }
            catch(Exception ex)
            {
                Assert.Fail("InvalidOperationException expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenDataTypeIsInvalid()
        {
            try
            {
                DestinationParser.Parse("DESTINATION APropertyName NOTATYPE SET {Not a Value}", new ImportDefinition());
                Assert.Fail("InvalidOperationException expected, not thrown.");
            }
            catch (InvalidCastException ex)
            {
                Assert.AreEqual("Cannot convert the token NOTATYPE to a valid DataType.", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("InvalidOperationException expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]


        private void DestinationParserCreatesDestinationOfType(Enums.DataTypes dataType, string SetValue)
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "DESTINATION Name " + dataType.ToString() + " SET {" + SetValue + "}";

            // Act
            DestinationParser.Parse(s, id);

            // Assert
            Assert.AreEqual(1, id.DestinationProperties.Count);
            var d = id.DestinationProperties[0];
            Assert.AreEqual("Name", d.PropertyName);
            Assert.AreEqual(dataType, d.DataType);
            Assert.IsFalse(d.Generate);
            Assert.AreEqual(SetValue, d.SetValue);
            Assert.IsFalse(d.Substitute);
            Assert.IsNull(d.SubstitutionName);
        }

        [TestMethod]
        public void DestinationParserCreatesDestinationOfTypeBOOLEAN()
        {
            DestinationParserCreatesDestinationOfType(Enums.DataTypes.BOOLEAN, "true");
        }

        [TestMethod]
        public void DestinationParserCreatesDestinationOfTypeBYTE()
        {
            DestinationParserCreatesDestinationOfType(Enums.DataTypes.BYTE, "254");
        }

        [TestMethod]
        public void DestinationParserCreatesDestinationOfTypeCHAR()
        {
            DestinationParserCreatesDestinationOfType(Enums.DataTypes.CHAR, "A");
        }

        [TestMethod]
        public void DestinationParserCreatesDestinationOfTypeDATETIME()
        {
            DestinationParserCreatesDestinationOfType(Enums.DataTypes.DATETIME, "2017-01-01T01:01:00Z");
        }

        [TestMethod]
        public void DestinationParserCreatesDestinationOfTypeDECIMAL()
        {
            DestinationParserCreatesDestinationOfType(Enums.DataTypes.DECIMAL, "12.34");
        }

        [TestMethod]
        public void DestinationParserCreatesDestinationOfTypeDOUBLE()
        {
            DestinationParserCreatesDestinationOfType(Enums.DataTypes.DOUBLE, "12.34");
        }

        [TestMethod]
        public void DestinationParserCreatesDestinationOfTypeGUID()
        {
            DestinationParserCreatesDestinationOfType(Enums.DataTypes.GUID, Guid.NewGuid().ToString());
        }

        [TestMethod]
        public void DestinationParserCreatesDestinationOfTypeINT16()
        {
            DestinationParserCreatesDestinationOfType(Enums.DataTypes.INT16, "32500");
        }

        [TestMethod]
        public void DestinationParserCreatesDestinationOfTypeINT32()
        {
            DestinationParserCreatesDestinationOfType(Enums.DataTypes.INT32, "2047483647");
        }

        [TestMethod]
        public void DestinationParserCreatesDestinationOfTypeINT64()
        {
            DestinationParserCreatesDestinationOfType(Enums.DataTypes.INT64, "8223372036854775807");
        }

        [TestMethod]
        public void DestinationParserCreatesDestinationOfTypeSTRING()
        {
            DestinationParserCreatesDestinationOfType(Enums.DataTypes.STRING, "Set a String");
        }

        #region Destination Set value invalid cast
        private void DestinationParserThrowsExceptionWhenSetValueDoesNotMatchDataType(Enums.DataTypes dataType, string SetValue)
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "DESTINATION Name " + dataType.ToString() + " SET {" + SetValue + "}";
            var expectedException = "Destination Property Name " +
                    "attempted to declare a Set Value of " + SetValue +
                    " which cannot be cast to the declared data type of " +
                    dataType.ToString();

            // Act
            try
            {
                DestinationParser.Parse(s, id);
                Assert.Fail("InvalidCastException expected, not thrown.");
            }
            catch(InvalidCastException ex)
            {
                Assert.AreEqual(expectedException, ex.Message);
            }
            catch(Exception ex)
            {
                Assert.Fail("InvalidCastException expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenSetValueDoesNotMatchBOOLEAN()
        {
            DestinationParserThrowsExceptionWhenSetValueDoesNotMatchDataType(Enums.DataTypes.BOOLEAN, "987.35");
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenSetValueDoesNotMatchBYTE()
        {
            DestinationParserThrowsExceptionWhenSetValueDoesNotMatchDataType(Enums.DataTypes.BYTE, "This is not a BYTE");
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenSetValueDoesNotMatchCHAR()
        {
            DestinationParserThrowsExceptionWhenSetValueDoesNotMatchDataType(Enums.DataTypes.CHAR, "987.35");
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenSetValueDoesNotMatchDATETIME()
        {
            DestinationParserThrowsExceptionWhenSetValueDoesNotMatchDataType(Enums.DataTypes.DATETIME, "This is not a date!");
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenSetValueDoesNotMatchDECIMAL()
        {
            DestinationParserThrowsExceptionWhenSetValueDoesNotMatchDataType(Enums.DataTypes.DECIMAL, "This isn't a decimal either.");
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenSetValueDoesNotMatchDOUBLE()
        {
            DestinationParserThrowsExceptionWhenSetValueDoesNotMatchDataType(Enums.DataTypes.DOUBLE, "Not even a number");
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenSetValueDoesNotMatchGUID()
        {
            DestinationParserThrowsExceptionWhenSetValueDoesNotMatchDataType(Enums.DataTypes.GUID, "987.35");
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenSetValueDoesNotMatchINT16()
        {
            DestinationParserThrowsExceptionWhenSetValueDoesNotMatchDataType(Enums.DataTypes.INT16, "9,223,372,036,854,775,807");
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenSetValueDoesNotMatchINT32()
        {
            DestinationParserThrowsExceptionWhenSetValueDoesNotMatchDataType(Enums.DataTypes.INT32, "9,223,372,036,854,775,807");
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenSetValueDoesNotMatchINT64()
        {
            DestinationParserThrowsExceptionWhenSetValueDoesNotMatchDataType(Enums.DataTypes.INT64, "9,223,372,036,854,775,807.5");
        }






        #endregion

        #region Destination Generate tests
        [TestMethod]
        public void DestinationParserCreatesGuidWithGenerate()
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "DESTINATION PropertyName GUID GENERATE";

            // Act
            DestinationParser.Parse(s, id);

            // Assert
            Assert.AreEqual(1, id.DestinationProperties.Count);
            var d = id.DestinationProperties[0];
            Assert.AreEqual("PropertyName", d.PropertyName);
            Assert.AreEqual(Enums.DataTypes.GUID, d.DataType);
            Assert.IsTrue(d.Generate);
            Assert.IsNull(d.SetValue);
        }

        [TestMethod]
        public void DestinationParserCreatesDateTimeWithGenerate()
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "DESTINATION PropertyName DATETIME GENERATE";

            // Act
            DestinationParser.Parse(s, id);

            // Assert
            Assert.AreEqual(1, id.DestinationProperties.Count);
            var d = id.DestinationProperties[0];
            Assert.AreEqual("PropertyName", d.PropertyName);
            Assert.AreEqual(Enums.DataTypes.DATETIME, d.DataType);
            Assert.IsTrue(d.Generate);
            Assert.IsFalse(d.GenerateDateOnly);
            Assert.IsNull(d.SetValue);
        }

        [TestMethod]
        public void DestinationParserCreatesDateTimeWithGenerateDateOnly()
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "DESTINATION PropertyName DATETIME GENERATE DATEONLY";

            // Act
            DestinationParser.Parse(s, id);

            // Assert
            Assert.AreEqual(1, id.DestinationProperties.Count);
            var d = id.DestinationProperties[0];
            Assert.AreEqual("PropertyName", d.PropertyName);
            Assert.AreEqual(Enums.DataTypes.DATETIME, d.DataType);
            Assert.IsTrue(d.Generate);
            Assert.IsTrue(d.GenerateDateOnly);
            Assert.IsNull(d.SetValue);
        }

        private void DestinationParserThrowsExceptionWhenSettingGenerateOnInvalidDataType(Enums.DataTypes dataType)
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "DESTINATION ColumnName " + dataType.ToString() + " GENERATE";

            // Act
            try
            {
                DestinationParser.Parse(s, id);
                Assert.Fail("Expected ArgumentException, not thrown.");
            }
            catch(ArgumentException ex)
            {
                Assert.AreEqual("Cannot set GENERATE on a data type other than GUID and DATETIME.",
                    ex.Message);
            }
            catch(Exception ex)
            {
                Assert.Fail("Expected ArgumentException, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenGENERATEDeclaredOnBOOLEAN()
        {
            DestinationParserThrowsExceptionWhenSettingGenerateOnInvalidDataType(Enums.DataTypes.BOOLEAN);
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenGENERATEDeclaredOnBYTE()
        {
            DestinationParserThrowsExceptionWhenSettingGenerateOnInvalidDataType(Enums.DataTypes.BYTE);
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenGENERATEDeclaredOnCHAR()
        {
            DestinationParserThrowsExceptionWhenSettingGenerateOnInvalidDataType(Enums.DataTypes.CHAR);
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenGENERATEDeclaredOnDECIMAL()
        {
            DestinationParserThrowsExceptionWhenSettingGenerateOnInvalidDataType(Enums.DataTypes.DECIMAL);
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenGENERATEDeclaredOnDOUBLE()
        {
            DestinationParserThrowsExceptionWhenSettingGenerateOnInvalidDataType(Enums.DataTypes.DOUBLE);
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenGENERATEDeclaredOnINT16()
        {
            DestinationParserThrowsExceptionWhenSettingGenerateOnInvalidDataType(Enums.DataTypes.INT16);
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenGENERATEDeclaredOnINT32()
        {
            DestinationParserThrowsExceptionWhenSettingGenerateOnInvalidDataType(Enums.DataTypes.INT32);
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenGENERATEDeclaredOnINT64()
        {
            DestinationParserThrowsExceptionWhenSettingGenerateOnInvalidDataType(Enums.DataTypes.INT64);
        }

        [TestMethod]
        public void DestinationParserThrowsExceptionWhenGENERATEDeclaredOnSTRING()
        {
            DestinationParserThrowsExceptionWhenSettingGenerateOnInvalidDataType(Enums.DataTypes.STRING);
        }
        #endregion

        #region SUBSTITUTE tests
        [TestMethod]
        public void DestinationParserThrowsExceptionWhenSUBSTITUTEIsMissingPlaceholder()
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "DESTINATION PropertyName STRING SUBSTITUTE";

            // Act
            try
            {
                DestinationParser.Parse(s, id);
                Assert.Fail("IndexOutOfRangeException expected, none thrown.");
            }
            catch(IndexOutOfRangeException ex)
            {
                Assert.AreEqual("SUBSTITUTE called with no parameter name specified.", ex.Message);
            }
            catch(Exception ex)
            {
                Assert.Fail("Expected IndexOutOfRangeException, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void DestinationParserCreatesSubstitution()
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "DESTINATION AProperty STRING SUBSTITUTE {PlaceholderName}";

            // Act
            DestinationParser.Parse(s, id);

            // Assert
            Assert.AreEqual(1, id.DestinationProperties.Count);
            var d = id.DestinationProperties[0];
            Assert.AreEqual("AProperty", d.PropertyName);
            Assert.AreEqual(Enums.DataTypes.STRING, d.DataType);
            Assert.IsFalse(d.Generate);
            Assert.IsFalse(d.GenerateDateOnly);
            Assert.IsNull(d.SetValue);
            Assert.IsTrue(d.Substitute);
            Assert.AreEqual("PlaceholderName", d.SubstitutionName);
        }

        #endregion
        [TestMethod]
        public void DestinationParserThrowsExceptionWhenInvalidTokenFollowsDataType()
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "DESTINATION AColumn STRING NOTAVALIDTOKEN";

            // Act
            try
            {
                DestinationParser.Parse(s, id);
                Assert.Fail("ArgumentException expected, not thrown.");
            }
            catch(ArgumentException ex)
            {
                Assert.AreEqual("Invalid token for Destination declaration: NOTAVALIDTOKEN",
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
