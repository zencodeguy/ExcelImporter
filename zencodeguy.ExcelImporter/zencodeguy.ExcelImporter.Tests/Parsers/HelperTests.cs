using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zencodeguy.ExcelImporter.Parsers;
using zencodeguy.ExcelImporter.Rules;

namespace zencodeguy.ExcelImporter.Tests.Parsers
{
    [TestClass]
    public class HelperTests
    {
        #region ParseNextTokenAsInteger tests
        [TestMethod]
        public void ParseNextTokenAsIntegerReturnsInteger()
        {
            // Arrange
            string[] tokens = { "COLUMN", "17", "ColumnName" };
            int currentToken = 0;

            // Act
            var result = Helpers.ParseNextTokenAsInteger(tokens, currentToken);

            // Assert
            Assert.AreEqual(17, result);

        }

        [TestMethod]
        public void ParseNextTokenAsIntegerFailsWhenTokenIsNotInteger()
        {
            // Arrange
            string[] tokens = { "COLUMN", "NotANumber", "ColumnName" };
            int currentToken = 0;

            // Act
            try
            {
                var result = Helpers.ParseNextTokenAsInteger(tokens, currentToken);
                Assert.Fail("InvalidCastException expected, not thrown.");
            }
            catch(InvalidCastException ex)
            {
                Assert.AreEqual("Invalid Token: Cannot cast value NotANumber to an integer.",
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
        public void ParseNextTokenAsIntegerFailsWhenIndexIsOutOfRange()
        {
            // Arrange
            string[] tokens = { "COLUMN", "NotANumber", "ColumnName" };
            int currentToken = 3;

            // Act
            try
            {
                var result = Helpers.ParseNextTokenAsInteger(tokens, currentToken);
                Assert.Fail("IndexOutOfRangeException expected, not thrown.");
            }
            catch (IndexOutOfRangeException ex)
            {
                Assert.AreEqual("Attempt to parse integer at position " +
                currentToken.ToString() +
                ", array contains " +
                tokens.Length.ToString() +
                " values.",
                    ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("IndexOutOfRange expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }
        #endregion

        #region IsColumnNumberAlreadyDefined tests
        [TestMethod]
        public void IsColumnNumberAlreadyDefinedThrowsExceptionWhenImportDefinitionIsNull()
        {
            try
            {
                Helpers.IsColumnNumberAlreadyDefined(1, null);
                Assert.Fail("ArgumentNullException expected, not thrown.");
            }
            catch(ArgumentNullException ex)
            {
                Assert.AreEqual("ID", ex.ParamName);
            }
            catch(Exception ex)
            {
                Assert.Fail("ArgumentNullException expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]   
        public void IsColumnNumberAlreadyDefinedReturnsTrue()
        {
            // Arrange
            var id = new ImportDefinition();
            id.Columns.Add(new ColumnDefinition()
            {
                ColumnNumber = 1,
                DataType = Enums.DataTypes.STRING,
                PropertyName = "ColumnName"
            });

            // Act
            var result = Helpers.IsColumnNumberAlreadyDefined(1, id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsColumnNumberAlreadyDefinedReturnsFalse()
        {
            // Arrange
            var id = new ImportDefinition();
            id.Columns.Add(new ColumnDefinition()
            {
                ColumnNumber = 2,
                DataType = Enums.DataTypes.STRING,
                PropertyName = "ColumnName"
            });

            // Act
            var result = Helpers.IsColumnNumberAlreadyDefined(1, id);

            // Assert
            Assert.IsFalse(result);
        }
        #endregion

        #region IsPropertyNameAlreadyDefined tests
        [TestMethod]
        public void IsPropertyNameAlreadyDefinedThrowsExceptionWhenImportDefinitionIsNull()
        {
            //Act
            try
            {
                var result = Helpers.IsPropertyNameAlreadyDefined("AName", null);
                Assert.Fail("Expected ArgumentNullException, not thrown.");
            }
            catch (ArgumentNullException ex)
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

        private void IsPropertyNameAlreadyDefinedThrowsExceptionWhenPropertyNameIsNullEquivalent(string propertyName)
        {
            //Act
            try
            {
                var result = Helpers.IsPropertyNameAlreadyDefined(propertyName, new ImportDefinition());
                Assert.Fail("Expected ArgumentNullException, not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.AreEqual("PropertyName", ex.ParamName);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected ArgumentNullException, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }

        }

        [TestMethod]
        public void IsPropertyNameAlreadyDefinedThrowsExceptionWhenPropertyNameIsNull()
        {
            IsPropertyNameAlreadyDefinedThrowsExceptionWhenPropertyNameIsNullEquivalent(null);
        }

        [TestMethod]
        public void IsPropertyNameAlreadyDefinedThrowsExceptionWhenPropertyNameIsEmptyString()
        {
            IsPropertyNameAlreadyDefinedThrowsExceptionWhenPropertyNameIsNullEquivalent(string.Empty);
        }
        [TestMethod]
        public void IsPropertyNameAlreadyDefinedThrowsExceptionWhenPropertyNameIsWhiteSpace()
        {
            IsPropertyNameAlreadyDefinedThrowsExceptionWhenPropertyNameIsNullEquivalent("   ");
        }

        [TestMethod]
        public void IsPropertyNameAlreadyDefinedReturnsTrue()
        {
            // Arrange
            var id = new ImportDefinition();
            id.Columns.Add(new ColumnDefinition()
            {
                ColumnNumber = 0,
                DataType = Enums.DataTypes.STRING,
                PropertyName = "IAlreadyExist"
            });

            // Act
            var result = Helpers.IsPropertyNameAlreadyDefined("IAlreadyExist", id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsPropertyNameAlreadyDefinedReturnsFalse()
        {
            // Arrange
            var id = new ImportDefinition();
            id.Columns.Add(new ColumnDefinition()
            {
                ColumnNumber = 0,
                DataType = Enums.DataTypes.STRING,
                PropertyName = "FirstPropertyName"
            });

            // Act
            var result = Helpers.IsPropertyNameAlreadyDefined("SecondPropertyName", id);

            // Assert
            Assert.IsFalse(result);
        }
        #endregion

        #region IsDestinationPropertyAlreadyDefined tests
        [TestMethod]
        public void IsDestinationPropertyAlreadyDefinedThrowsExceptionWhenImportDefinitionIsNull()
        {
            try
            {
                var result = Helpers.IsDestinationPropertyAlreadyDefined("PropertyName", null);
                Assert.Fail("ArgumentNullException expected, not thrown.");
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

        private void IsDestinationPropertyAlreadyDefinedThrowsExceptionWhenPropertyNameIsNullEquivalent(string propertyName)
        {
            try
            {
                var result = Helpers.IsPropertyNameAlreadyDefined(propertyName, new ImportDefinition());
                Assert.Fail("Expected ArgumentNullException, not thrown.");
            }
            catch(ArgumentNullException ex)
            {
                Assert.AreEqual("PropertyName", ex.ParamName);
            }
            catch(Exception ex)
            {
                Assert.Fail("Expected ArgumentNullException, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void IsDestinationPropertyAlreadyDefinedThrowsExceptionWhenPropertyNameIsNull()
        {
            IsDestinationPropertyAlreadyDefinedThrowsExceptionWhenPropertyNameIsNullEquivalent(null);
        }

        [TestMethod]
        public void IsDestinationPropertyAlreadyDefinedThrowsExceptionWhenPropertyNameIsEmptyString()
        {
            IsDestinationPropertyAlreadyDefinedThrowsExceptionWhenPropertyNameIsNullEquivalent(string.Empty);
        }

        [TestMethod]
        public void IsDestinationPropertyAlreadyDefinedThrowsExceptionWhenPropertyNameIsWhiteSpace()
        {
            IsDestinationPropertyAlreadyDefinedThrowsExceptionWhenPropertyNameIsNullEquivalent("   ");
        }

        [TestMethod]
        public void IsDestinationPropertyAlreadyDefinedReturnsTrue()
        {
            // Arrange
            var id = new ImportDefinition();
            id.DestinationProperties.Add(new DestinationPropertyDefinition()
            {
                PropertyName = "IAlreadyExist",
                DataType = Enums.DataTypes.STRING
            });

            // Act
            var result = Helpers.IsDestinationPropertyAlreadyDefined("IAlreadyExist", id);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsDestinationPropertyAlreadyDefinedReturnsFalse()
        {
            // Arrange
            var id = new ImportDefinition();
            id.DestinationProperties.Add(new DestinationPropertyDefinition()
            {
                PropertyName = "IAlreadyExist",
                DataType = Enums.DataTypes.STRING
            });

            // Act
            var result = Helpers.IsDestinationPropertyAlreadyDefined("ButIDont", id);

            // Assert
            Assert.IsFalse(result);
        }
        #endregion

        #region CanSetValueBeParsedAsType tests
        //TODO: Create tests
        #endregion

    }
}
