using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using zencodeguy.ExcelImporter.Rules;

namespace zencodeguy.ExcelImporter.Tests.Rules
{
    [TestClass]
    public class ValueComparisonTests
    {
        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionIfPropertyNameIsNull()
        {
            // Act
            var c = new ValueComparison(null, Enums.ComparisonOperators.EQ, new List<string>(1) { "A Value" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionIfPropertyNameIsEmptyString()
        {
            // Act
            var c = new ValueComparison(String.Empty, Enums.ComparisonOperators.EQ, new List<string>(1) { "A Value" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionIfPropertyNameIsWhiteSpace()
        {
            // Act
            var c = new ValueComparison("   ", Enums.ComparisonOperators.EQ, new List<string>(1) { "A Value" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionIfValidValuesIsNull()
        {
            // Act
            var c = new ValueComparison("A property", Enums.ComparisonOperators.EQ, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorThrowsExceptionIfValidValuesIsEmptyList()
        {
            // Act
            var c = new ValueComparison("A property", Enums.ComparisonOperators.EQ, new List<string>());
        }
        #endregion

        #region Compare Tests
        [TestMethod]
        public void ValueComparisonReturnsTrueOnEquals()
        {
            // Arrange
            var c = new ValueComparison("A Property", Enums.ComparisonOperators.EQ, new List<string>(1) { "A Value" });

            // Act
            var result = c.Compare("A Value");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValueComparisionReturnsFalseOnEquals()
        {
            // Arrange
            var c = new ValueComparison("A Property", Enums.ComparisonOperators.EQ, new List<string>(1) { "A Value" });

            // Act
            var result = c.Compare("Not A Value");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValueComparisonReturnsTrueOnNotEquals()
        {
            // Arrange 
            var c = new ValueComparison("A Property", Enums.ComparisonOperators.NOTEQ, new List<string>(1) { "A Value" });

            // Act
            var result = c.Compare("Not A Value");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValueComparisonReturnsFalseOnNotEquals()
        {
            // Arrange
            var c = new ValueComparison("A Property", Enums.ComparisonOperators.NOTEQ, new List<string>(1) { "A Value" });

            // Act
            var result = c.Compare("A Value");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValueComparisonReturnsTrueOnIn()
        {
            // Arrange
            var c = new ValueComparison("A Property", Enums.ComparisonOperators.IN, new List<string>(2) { "A Value", "B Value" });

            // Act
            var result = c.Compare("A Value");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValueComparisonReturnsFalseOnIn()
        {
            // Arrange
            var c = new ValueComparison("A Property", Enums.ComparisonOperators.IN, new List<string>(2) { "A Value", "B Value" });

            // Act
            var result = c.Compare("Not A Value");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValueComparisonReturnsTrueOnNotIn()
        {
            // Arrange
            var c = new ValueComparison("A Property", Enums.ComparisonOperators.NOTIN, new List<string>(2) { "A Value", "B Value" });

            // Act
            var result = c.Compare("Not A Value");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValueComparisonReturnsFalseOnNotIn()
        {
            // Arrange
            var c = new ValueComparison("A Property", Enums.ComparisonOperators.NOTIN, new List<string>(2) { "A Value", "B Value" });

            // Act
            var result = c.Compare("A Value");

            // Assert
            Assert.IsFalse(result);
        }
        #endregion

        #region tests with null values
        private void TestWithNull(string value, Enums.ComparisonOperators comparison)
        {
            var c = new ValueComparison("A Property", comparison, new List<string>(1) { "A Value" });
            var result = c.Compare(value);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValueComparisonEQReturnsTrueWhenValueIsNullOrWhiteSpace()
        {
            TestWithNull(null, Enums.ComparisonOperators.EQ);
        }

        [TestMethod]
        public void ValueComparisonEQReturnsTrueWhenValueIsEmptyString()
        {
            TestWithNull(string.Empty, Enums.ComparisonOperators.EQ);
        }

        [TestMethod]
        public void ValueComparisonEQReturnsTrueWhenValueIsWhiteSpace()
        {
            TestWithNull("   ", Enums.ComparisonOperators.EQ);
        }

        [TestMethod]
        public void ValueComparisonNOTEQReturnsTrueWhenValueIsNull()
        {
            TestWithNull(null, Enums.ComparisonOperators.NOTEQ);
        }

        [TestMethod]
        public void ValueComparisonNOTEQReturnsTrueWhenValueIsEmptyString()
        {
            TestWithNull(string.Empty, Enums.ComparisonOperators.NOTEQ);
        }

        [TestMethod]
        public void ValueComparisonNOTEQReturnsTrueWhenValueIsWhiteSpace()
        {
            TestWithNull("   ", Enums.ComparisonOperators.NOTEQ);
        }

        [TestMethod]
        public void ValueComparisonINReturnsTrueWhenValueIsNull()
        {
            TestWithNull(null, Enums.ComparisonOperators.IN);
        }

        [TestMethod]
        public void ValueComparisonINReturnsTrueWhenValueIsEmptyString()
        {
            TestWithNull(string.Empty, Enums.ComparisonOperators.IN);
        }

        [TestMethod]
        public void ValueComparisonINReturnsTrueWhenValueIsWhiteSpace()
        {
            TestWithNull("    ", Enums.ComparisonOperators.IN);
        }

        [TestMethod]
        public void ValueComparisonNOTINReturnsTrueWhenValueIsNull()
        {
            TestWithNull(null, Enums.ComparisonOperators.NOTIN);
        }

        [TestMethod]
        public void ValueComparisonNOTINReturnsTrueWhenValueIsEmptyString()
        {
            TestWithNull(string.Empty, Enums.ComparisonOperators.NOTIN);
        }

        [TestMethod]
        public void ValueComparisonNOTINReturnsTrueWhenValueIsWhiteSpace()
        {
            TestWithNull("   ", Enums.ComparisonOperators.NOTIN);
        }
        #endregion

    }
}
