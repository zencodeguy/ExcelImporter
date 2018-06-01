using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zencodeguy.ExcelImporter.Parsers;
using zencodeguy.ExcelImporter.Rules;

namespace zencodeguy.ExcelImporter.Tests.Parsers
{
    [TestClass]
    public class ColumnDefinitionParserTests
    {
        private void ColumnDefinitionParserThrowsExceptionWhenLineIsNullEquivalent(string line)
        {
            // Arrange
            var id = new ImportDefinition();

            // Act
            try
            {
                ColumnDefinitionParser.Parse(line, id);
                Assert.Fail("Expected ArgumentNullException, not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("Line", ex.ParamName);
            }
            catch (Exception ex)
            {
                Assert.Fail("ArgumentNullException expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenLineIsNull()
        {
            ColumnDefinitionParserThrowsExceptionWhenLineIsNullEquivalent(null);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenLineIsEmptyString()
        {
            ColumnDefinitionParserThrowsExceptionWhenLineIsNullEquivalent(string.Empty);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenLineIsWhiteSpace()
        {
            ColumnDefinitionParserThrowsExceptionWhenLineIsNullEquivalent("   ");
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenImportDefinitionIsNull()
        {
            // Act
            try
            {
                ColumnDefinitionParser.Parse("A Line", null);
                Assert.Fail("Expected ArgumentNullException, not thrown.");
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
        public void ColumnDefinitionParserThrowsExceptionWhenLineIsNotColumnDeclaration()
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "NOTACOLUMN 0 ColumnName";

            // Act
            try
            {
                ColumnDefinitionParser.Parse(s, id);
                Assert.Fail("ArgumentException expected, not thrown.");
            }
            catch(ArgumentException ex)
            {
                Assert.AreEqual("Line is not a COLUMN declaration.",
                    ex.Message);
            }
            catch(Exception ex)
            {
                Assert.Fail("ArgumentException expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenLineHasTooFewTokens()
        {
            // Arrange
            var s = "COLUMN 1 PropertyName";
            var id = new ImportDefinition();

            // Act
            try
            {
                ColumnDefinitionParser.Parse(s, id);
                Assert.Fail("ArgumentException expected, not thrown.");
            }
            catch(ArgumentException ex)
            {
                Assert.AreEqual("Column definition line must contain at least 4 tokens.",
                    ex.Message);
            }
            catch(Exception ex)
            {
                Assert.Fail("ArgumentException expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenColumnNumberIsNotANumber()
        {
            // Arrange
            var s = "COLUMN NOTANUMBER ColumnName STRING";
            var id = new ImportDefinition();

            // Act
            try
            {
                ColumnDefinitionParser.Parse(s, id);
                Assert.Fail("InvalidCaseException expected, not thrown.");
            }
            catch(InvalidCastException ex)
            {
                Assert.AreEqual("Cannot convert the token NOTANUMBER to an integer value.",
                    ex.Message);
            }
            catch(Exception ex)
            {
                Assert.Fail("InvalidCastException expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenColumnNumberDefinedTwice()
        {
            // Arrange
            var id = new ImportDefinition();
            id.Columns.Add(new ColumnDefinition()
            {
                ColumnNumber = 0,
                DataType = Enums.DataTypes.STRING,
                PropertyName = "ColumnNameZero"
            });
            var s = "COLUMN 0 ColumnZeroAgain STRING";

            try
            {
                ColumnDefinitionParser.Parse(s, id);
                Assert.Fail("InvalidOperationException expected, not thrown.");
            }
            catch(InvalidOperationException ex)
            {
                Assert.AreEqual("A column with the number 0 is defined more than once.",
                    ex.Message);
            }
            catch(Exception ex)
            {
                Assert.Fail("InvalidOperationException expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenPropertyNameDefinedMoreThanOnce()
        {
            // Arrange
            var id = new ImportDefinition();
            id.Columns.Add(new ColumnDefinition()
            {
                ColumnNumber = 0,
                DataType = Enums.DataTypes.STRING,
                PropertyName = "DuplicatePropertyName"
            });
            var s = "COLUMN 1 DuplicatePropertyName STRING";

            // Act
            try
            {
                ColumnDefinitionParser.Parse(s, id);
                Assert.Fail("InvalidOperationException expected, not thrown");
            }
            catch(InvalidOperationException ex)
            {
                Assert.AreEqual("A column with the name 'DuplicatePropertyName' is defined more than once.",
                    ex.Message);
            }
            catch(Exception ex)
            {
                Assert.Fail("InvalidOperationException expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenDataTypeIsInvalid()
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "COLUMN 0 ColumnName INVALIDTYPE";

            // Act
            try
            {
                ColumnDefinitionParser.Parse(s, id);
                Assert.Fail("InvalidCastException expected, not thrown.");
            }
            catch(InvalidCastException ex)
            {
                Assert.AreEqual("Cannot convert the token INVALIDTYPE to a valid DataType.",
                    ex.Message);
            }
            catch(Exception ex)
            {
                Assert.Fail("InvalidCastException expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        #region Column DataType tests
        private void ColumnDefinitionParserCreatesColumn(Enums.DataTypes dataType)
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "COLUMN 1 ColumnName " + dataType.ToString();

            // Act
            ColumnDefinitionParser.Parse(s, id);

            // Assert
            Assert.AreEqual(1, id.Columns.Count);
            var c = id.Columns[0];
            Assert.AreEqual(dataType, c.DataType);
            Assert.AreEqual(1, c.ColumnNumber);
            Assert.AreEqual("ColumnName", c.PropertyName);
        }

        [TestMethod]
        public void ColumnDefinitionParserCreatesColumnOfDataTypeBOOLEAN()
        {
            ColumnDefinitionParserCreatesColumn(Enums.DataTypes.BOOLEAN);
        }

        [TestMethod]
        public void ColumnDefinitionParserCreatesColumnOfDataTypeBYTE()
        {
            ColumnDefinitionParserCreatesColumn(Enums.DataTypes.BYTE);
        }

        [TestMethod]
        public void ColumnDefinitionParserCreatesColumnOfDataTypeCHAR()
        {
            ColumnDefinitionParserCreatesColumn(Enums.DataTypes.CHAR);
        }

        [TestMethod]
        public void ColumnDefinitionParserCreatesColumnOfDataTypeDATETIME()
        {
            ColumnDefinitionParserCreatesColumn(Enums.DataTypes.DATETIME);
        }

        [TestMethod]
        public void ColumnDefinitionParserCreatesColumnOfDatTypeDECIMAL()
        {
            ColumnDefinitionParserCreatesColumn(Enums.DataTypes.DECIMAL);
        }

        [TestMethod]
        public void ColumnDefinitionParserCreatesColumnOfDatTypeDOUBLE()
        {
            ColumnDefinitionParserCreatesColumn(Enums.DataTypes.DOUBLE);
        }

        [TestMethod]
        public void ColumnDefinitionParserCreatesColumnOfDatTypeGUID()
        {
            ColumnDefinitionParserCreatesColumn(Enums.DataTypes.GUID);
        }

        [TestMethod]
        public void ColumnDefinitionParserCreatesColumnOfDatTypeINT16()
        {
            ColumnDefinitionParserCreatesColumn(Enums.DataTypes.INT16);
        }

        [TestMethod]
        public void ColumnDefinitionParserCreatesColumnOfDatTypeINT32()
        {
            ColumnDefinitionParserCreatesColumn(Enums.DataTypes.INT32);
        }

        [TestMethod]
        public void ColumnDefinitionParserCreatesColumnOfDatTypeINT64()
        {
            ColumnDefinitionParserCreatesColumn(Enums.DataTypes.INT64);
        }

        [TestMethod]
        public void ColumnDefinitionParserCreatesColumnOfDatTypeSTRING()
        {
            ColumnDefinitionParserCreatesColumn(Enums.DataTypes.STRING);
        }
        #endregion

        [TestMethod]
        public void ColumnDefinitionParserCreatesRequiredRule()
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "COLUMN 0 ColumnName STRING REQUIRED";

            // Act
            ColumnDefinitionParser.Parse(s, id);

            // Assert
            Assert.AreEqual(1, id.Columns.Count);
            var c = id.Columns[0];
            Assert.AreEqual(0, c.ColumnNumber);
            Assert.AreEqual(Enums.DataTypes.STRING, c.DataType);
            Assert.AreEqual("ColumnName", c.PropertyName);

            Assert.AreEqual(1, id.Rules.Count);
            var r = id.Rules[0];
            Assert.AreEqual(typeof(RequiredRule), r.GetType());
            Assert.AreEqual("ColumnName", r.PropertyName);
        }

        #region MINLENGTH rule tests
        [TestMethod]
        public void ColumnDefinitionParserCreatesStringMinimumLengthRule()
        {
            // Arrange
            var s = "COLUMN 1 ColumnName STRING MINLENGTH 10";
            var id = new ImportDefinition();

            // Act
            ColumnDefinitionParser.Parse(s, id);

            // Assert
            Assert.AreEqual(1, id.Rules.Count);
            var r = (StringMinimumLengthRule)id.Rules[0];
            Assert.AreEqual(typeof(StringMinimumLengthRule), r.GetType());
            Assert.AreEqual("ColumnName", r.PropertyName);
            Assert.IsTrue(r.Length.HasValue);
            Assert.AreEqual(10, r.Length.Value);
        }

        private void ColumnDefinitionParserThrowsExceptionOnMinLengthDeclaredOnNonString(Enums.DataTypes dataType)
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "COLUMN 0 ColumnName " + dataType.ToString() + " MINLENGTH 10";

            // Act
            try
            {
                ColumnDefinitionParser.Parse(s, id);
                Assert.Fail("ArgumentException expected, not thrown.");
            }
            catch(ArgumentException ex)
            {
                Assert.AreEqual("MINLENGTH validation token can only be specified for data type STRING.",
                    ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Argument Exception expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMinLengthDeclaredOnBOOLEAN()
        {
            ColumnDefinitionParserThrowsExceptionOnMinLengthDeclaredOnNonString(Enums.DataTypes.BOOLEAN);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMinLengthDeclaredOnBYTE()
        {
            ColumnDefinitionParserThrowsExceptionOnMinLengthDeclaredOnNonString(Enums.DataTypes.BYTE);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMinLengthDeclaredOnCHAR()
        {
            ColumnDefinitionParserThrowsExceptionOnMinLengthDeclaredOnNonString(Enums.DataTypes.CHAR);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMinLengthDeclaredOnDATETIME()
        {
            ColumnDefinitionParserThrowsExceptionOnMinLengthDeclaredOnNonString(Enums.DataTypes.DATETIME);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMinLengthDeclaredOnDECIMAL()
        {
            ColumnDefinitionParserThrowsExceptionOnMinLengthDeclaredOnNonString(Enums.DataTypes.DECIMAL);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMinLengthDeclaredOnDOUBLE()
        {
            ColumnDefinitionParserThrowsExceptionOnMinLengthDeclaredOnNonString(Enums.DataTypes.DOUBLE);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMinLengthDeclaredOnGUID()
        {
            ColumnDefinitionParserThrowsExceptionOnMinLengthDeclaredOnNonString(Enums.DataTypes.GUID);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMinLengthDeclaredOnINT16()
        {
            ColumnDefinitionParserThrowsExceptionOnMinLengthDeclaredOnNonString(Enums.DataTypes.INT16);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMinLengthDeclaredOnINT32()
        {
            ColumnDefinitionParserThrowsExceptionOnMinLengthDeclaredOnNonString(Enums.DataTypes.INT32);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMinLengthDeclaredOnINT64()
        {
            ColumnDefinitionParserThrowsExceptionOnMinLengthDeclaredOnNonString(Enums.DataTypes.INT64);
        }
        #endregion

        #region MAXLENGTH Rule tests
        [TestMethod]
        public void ColumnDefinitionParserCreatesStringMaximumLengthRule()
        {
            // Arrange
            var s = "COLUMN 1 ColumnName STRING MAXLENGTH 10";
            var id = new ImportDefinition();

            // Act
            ColumnDefinitionParser.Parse(s, id);

            // Assert
            Assert.AreEqual(1, id.Rules.Count);
            var r = (StringMaximumLengthRule)id.Rules[0];
            Assert.AreEqual(typeof(StringMaximumLengthRule), r.GetType());
            Assert.AreEqual("ColumnName", r.PropertyName);
            Assert.IsTrue(r.Length.HasValue);
            Assert.AreEqual(10, r.Length.Value);
        }

        private void ColumnDefinitionParserThrowsExceptionOnMaxLengthDeclaredOnNonString(Enums.DataTypes dataType)
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "COLUMN 0 ColumnName " + dataType.ToString() + " MAXLENGTH 10";

            // Act
            try
            {
                ColumnDefinitionParser.Parse(s, id);
                Assert.Fail("ArgumentException expected, not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("MAXLENGTH validation token can only be specified for data type STRING.",
                    ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Argument Exception expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMaxLengthDeclaredOnBOOLEAN()
        {
            ColumnDefinitionParserThrowsExceptionOnMaxLengthDeclaredOnNonString(Enums.DataTypes.BOOLEAN);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMaxLengthDeclaredOnBYTE()
        {
            ColumnDefinitionParserThrowsExceptionOnMaxLengthDeclaredOnNonString(Enums.DataTypes.BYTE);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMaxLengthDeclaredOnCHAR()
        {
            ColumnDefinitionParserThrowsExceptionOnMaxLengthDeclaredOnNonString(Enums.DataTypes.CHAR);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMaxLengthDeclaredOnDATETIME()
        {
            ColumnDefinitionParserThrowsExceptionOnMaxLengthDeclaredOnNonString(Enums.DataTypes.DATETIME);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMaxLengthDeclaredOnDECIMAL()
        {
            ColumnDefinitionParserThrowsExceptionOnMaxLengthDeclaredOnNonString(Enums.DataTypes.DECIMAL);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMaxLengthDeclaredOnDOUBLE()
        {
            ColumnDefinitionParserThrowsExceptionOnMaxLengthDeclaredOnNonString(Enums.DataTypes.DOUBLE);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMaxLengthDeclaredOnGUID()
        {
            ColumnDefinitionParserThrowsExceptionOnMaxLengthDeclaredOnNonString(Enums.DataTypes.GUID);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMaxLengthDeclaredOnINT16()
        {
            ColumnDefinitionParserThrowsExceptionOnMaxLengthDeclaredOnNonString(Enums.DataTypes.INT16);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMaxLengthDeclaredOnINT32()
        {
            ColumnDefinitionParserThrowsExceptionOnMaxLengthDeclaredOnNonString(Enums.DataTypes.INT32);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenMaxLengthDeclaredOnINT64()
        {
            ColumnDefinitionParserThrowsExceptionOnMaxLengthDeclaredOnNonString(Enums.DataTypes.INT64);
        }
        #endregion

        #region PRECISION rule tests
        // There are no rule tests validating values for Precision and Scale.
        // Verifying they are integers is handled by the Helper tests.
        // Verifying they are within range is handled in the Precision rule itself.
        [TestMethod]
        public void ColumnDefinitionParserCreatesPRECISIONRule()
        {
            // Arrange
            var s = "COLUMN 1 ColumnName DECIMAL PRECISION 6 3";
            var id = new ImportDefinition();

            // Act
            ColumnDefinitionParser.Parse(s, id);

            // Assert
            Assert.AreEqual(1, id.Rules.Count);
            var r = (DecimalPrecisionRule)id.Rules[0];
            Assert.AreEqual("ColumnName", r.PropertyName);
            Assert.AreEqual(6, r.PrecisionValue);
            Assert.AreEqual(3, r.ScaleValue);
        }

        private void ColumnDefinitionParserThrowsExceptionOnPRECISIONDeclaredOnNonString(Enums.DataTypes dataType)
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "COLUMN 0 ColumnName " + dataType.ToString() + " PRECISION 6 3";

            // Act
            try
            {
                ColumnDefinitionParser.Parse(s, id);
                Assert.Fail("ArgumentException expected, not thrown.");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("PRECISION validation token can only be specified for data type DECIMAL.",
                    ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Argument Exception expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenPRECISIONDeclaredOnBOOLEAN()
        {
            ColumnDefinitionParserThrowsExceptionOnPRECISIONDeclaredOnNonString(Enums.DataTypes.BOOLEAN);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenPRECISIONDeclaredOnBYTE()
        {
            ColumnDefinitionParserThrowsExceptionOnPRECISIONDeclaredOnNonString(Enums.DataTypes.BYTE);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenPRECISIONDeclaredOnCHAR()
        {
            ColumnDefinitionParserThrowsExceptionOnPRECISIONDeclaredOnNonString(Enums.DataTypes.CHAR);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenPRECISIONDeclaredOnDATETIME()
        {
            ColumnDefinitionParserThrowsExceptionOnPRECISIONDeclaredOnNonString(Enums.DataTypes.DATETIME);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenPRECISIONDeclaredOnDOUBLE()
        {
            ColumnDefinitionParserThrowsExceptionOnPRECISIONDeclaredOnNonString(Enums.DataTypes.DOUBLE);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenPRECISIONDeclaredOnGUID()
        {
            ColumnDefinitionParserThrowsExceptionOnPRECISIONDeclaredOnNonString(Enums.DataTypes.GUID);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenPRECISIONDeclaredOnINT16()
        {
            ColumnDefinitionParserThrowsExceptionOnPRECISIONDeclaredOnNonString(Enums.DataTypes.INT16);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenPRECISIONDeclaredOnINT32()
        {
            ColumnDefinitionParserThrowsExceptionOnPRECISIONDeclaredOnNonString(Enums.DataTypes.INT32);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenPRECISIONDeclaredOnINT64()
        {
            ColumnDefinitionParserThrowsExceptionOnPRECISIONDeclaredOnNonString(Enums.DataTypes.INT64);
        }

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenPRECISIONDeclaredOnSTRING()
        {
            ColumnDefinitionParserThrowsExceptionOnPRECISIONDeclaredOnNonString(Enums.DataTypes.STRING);
        }




        #endregion

        [TestMethod]
        public void ColumnDefinitionParserThrowsExceptionWhenExtraTokenIsInvalid()
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "COLUMN 0 ColumnName STRING NOTAVALIDTOKEN";

            // Act
            try
            {
                ColumnDefinitionParser.Parse(s, id);
                Assert.Fail("ArgumentException expected, not thrown.");
            }
            catch(ArgumentException ex)
            {
                Assert.AreEqual("Invalid token: NOTAVALIDTOKEN", ex.Message);
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
