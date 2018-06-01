using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using zencodeguy.ExcelImporter.Rules;

namespace zencodeguy.ExcelImporter.Tests.Rules
{
    [TestClass]
    public class DecimalPrecisionRuleTests
    {
        private Dictionary<string, string> Values;

        [TestInitialize]
        public void Setup()
        {
            this.Values = new Dictionary<string, string>();
            this.Values.Add("Item1", "Value1");
        }

        #region Constructor Tests
        [TestMethod]
        public void ConstructorCreatesRule()
        {
            // Arrange
            var ruleName = "ARuleName";

            // Act
            var r = new DecimalPrecisionRule(ruleName);

            // Assert
            Assert.IsNotNull(r);
            Assert.AreEqual(ruleName, r.RuleName);
            Assert.IsTrue(string.IsNullOrWhiteSpace(r.PropertyName));
            Assert.IsFalse(r.PrecisionValue.HasValue);
            Assert.IsFalse(r.ScaleValue.HasValue);
            Assert.IsNull(r.ErrorMessage);
        }

        private void ConstructorThrowsExceptionOnNullEquivalent(string RuleName)
        {
            // Act
            var r = new DecimalPrecisionRule(RuleName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionOnNull()
        {
            ConstructorThrowsExceptionOnNullEquivalent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionOnEmptyString()
        {
            ConstructorThrowsExceptionOnNullEquivalent(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionOnWhiteSpace()
        {
            ConstructorThrowsExceptionOnNullEquivalent("    ");
        }
        #endregion

        #region Property tests
        private void PropertyThrowsExceptionOnNullEquivalent(string Property)
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName");

            // Act
            r.Property(Property);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyThrowsExceptionOnNull()
        {
            PropertyThrowsExceptionOnNullEquivalent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyThrowsExceptionOnEmptyString()
        {
            PropertyThrowsExceptionOnNullEquivalent(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyThrowsExceptionOnWhiteSpace()
        {
            PropertyThrowsExceptionOnNullEquivalent("  ");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PropertyThrowsExceptionIfPropertyAlreadySet()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName");

            // Act
            r.Property("BPropertyName");
        }

        [TestMethod]
        public void PropertySetsProperty()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName");
            var name = "APropertyName";

            // Act
            r.Property(name);

            // Assert
            Assert.AreEqual(name, r.PropertyName);
        }
        #endregion

        #region Precision Tests
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CannotSetPrecisionWhenPropertyNotSet()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName");

            // Act
            r.Precision(10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CannotSetPecisionToZero()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName");

            // Act
            r.Precision(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CannotSetPrecisionToNegativeValue()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName");

            // Act
            r.Precision(-10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CannotSetPrecisionAbove38()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName");

            // Act
            r.Precision(39);
        }

        [TestMethod]
        public void PrecisionSetsPrecision()
        { 
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName");

            // Act
            r.Precision(6);

            // Assert
            Assert.IsTrue(r.PrecisionValue.HasValue);
            Assert.AreEqual(6, r.PrecisionValue.Value);
        }

        [TestMethod]
        public void PrecisionCanBeSetTo38Precisely()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName");

            // Act
            r.Precision(38);

            // Assert
            Assert.IsTrue(r.PrecisionValue.HasValue);
            Assert.AreEqual(38, r.PrecisionValue.Value);
        }
        #endregion

        #region Scale Tests
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ScaleCannotBeSetIfPropertyNotSet()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName");

            // Act
            r.Scale(10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ScaleCannotBeSetToNegativeNumber()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName");

            // Act
            r.Scale(-10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ScaleCannotBeSetToZero()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName");

            // Act
            r.Scale(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ScaleCannotBeGreaterThan38()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName");

            // Act
            r.Scale(39);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ScaleCannotBeSetWhenPrecisionIsNotSet()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName");

            // Act
            r.Scale(10);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ScaleCannotBeGreaterThanPrecision()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10);

            // Act
            r.Scale(11);
        }

        [TestMethod]
        public void ScaleSetsScaleValue()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10);

            // Act
            r.Scale(5);

            // Assert
            Assert.IsTrue(r.ScaleValue.HasValue);
            Assert.AreEqual(5, r.ScaleValue.Value);
        }

        [TestMethod]
        public void ScaleCanBeEqualToPrecision()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10);

            // Act
            r.Scale(10);

            // Assert
            Assert.IsTrue(r.ScaleValue.HasValue);
            Assert.AreEqual(10, r.ScaleValue.Value);
        }
        #endregion

        #region IsValid Tests
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsValidThrowsExceptionWhenPropertyNotSet()
        {
            var r = new DecimalPrecisionRule("ARuleName");

            // Act
            var result = r.IsValid(this.Values);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsValidThrowsExceptionWhenPrecisionNotSet()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName");

            // Act
            var result = r.IsValid(this.Values);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsValidThrowsExceptionWhenScaleNotSet()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10);

            // Act
            var result = r.IsValid(this.Values);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsValidThrowsExceptionIfValuesIsNull()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10).Scale(5);

            // Act
            var result = r.IsValid(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IsValidThrowsExceptionIfDictionaryIsEmpty()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10).Scale(5);
            this.Values.Clear();

            // Act
            var result = r.IsValid(this.Values);
        }

        [TestMethod]
        // This may seem wrong, but the lact of a value is not an error
        // for this rule. That must be checked with a RequiredRule or a WhenRequiredRule.
        public void IsValidReturnsTrueIfPropertyIsNotInValuesDictionary()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10).Scale(5);

            // Act
            var result = r.IsValid(this.Values);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidReturnsTrueNumberWithDecimalPointIsLessThanPrecision()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10).Scale(5);
            this.Values.Add("APropertyName", "123.456");

            // Act
            var result = r.IsValid(this.Values);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenNumberIsExactlyAsLongAsPrecisionWithDecimalPoint()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10).Scale(5);
            this.Values.Add("APropertyName", "12345.67890");

            // Act
            var result = r.IsValid(this.Values);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenNumberIsExactlyAsLongAsPrecisionWithDecimalPointAndNegativeSign()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(9).Scale(6);
            this.Values.Add("APropertyName", "-123.123456");

            // Act
            var result = r.IsValid(this.Values);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenNumberIsExactlyAsLongAsPrecisionWithoutDecimalPoint()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10).Scale(5);
            this.Values.Add("APropertyName", "1234567890");

            // Act
            var result = r.IsValid(this.Values);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenNumberIsLongerThanPrecision()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10).Scale(5);
            this.Values.Add("APropertyName", "1234567890123");

            // Act
            var result = r.IsValid(this.Values);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("APropertyName value of \"1234567890123\" is greater than the specified Precision of 10",
                r.ErrorMessage);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenDecimalPortionIsLessThanScale()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10).Scale(5);
            this.Values.Add("APropertyName", "123.456");

            // Act
            var result = r.IsValid(this.Values);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenDecimalPortionIsExactlyScale()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10).Scale(5);
            this.Values.Add("APropertyName", "123.12345");

            // Act
            var result = r.IsValid(this.Values);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenDecimalDoesNotExist()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10).Scale(5);
            this.Values.Add("APropertyName", "12312345");

            // Act
            var result = r.IsValid(this.Values);

            // Assert
            Assert.IsTrue(result);
        }

        private void TestIsValidWithNullEquivalent(string Value)
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10).Scale(5);
            this.Values.Add("APropertyName", Value);

            // Act
            var result = r.IsValid(this.Values);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenValueIsNull()
        {
            TestIsValidWithNullEquivalent(null);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenValueIsEmptyString()
        {
            TestIsValidWithNullEquivalent(string.Empty);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenValueIsWhiteSpace()
        {
            TestIsValidWithNullEquivalent("   ");
        }


        [TestMethod]
        public void IsValidReturnsFalseWhenDecimalIsGreaterThanScale()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10).Scale(5);
            this.Values.Add("APropertyName", "123.123456");

            // Act
            var result = r.IsValid(this.Values);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("APropertyName value of \"123.123456\" has more digits in the decimal part of the number than the specified Scale of 5",
                r.ErrorMessage);
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenValueCannotBeCastAsDecimal()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10).Scale(5);
            this.Values.Add("APropertyName", "I am not a number");

            // Act
            var result = r.IsValid(this.Values);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("APropertyName value of \"I am not a number\" cannot be interpreted as a Decimal value. Rule not applied.",
                r.ErrorMessage);
        }
        #endregion

        #region GetPropertyNamesReferencedTests
        [TestMethod]
        public void GetPropertyNamesReferencedReturnsPropertyName()
        {
            // Arrange
            var r = new DecimalPrecisionRule("ARuleName").Property("APropertyName").Precision(10).Scale(5);

            // Act
            var propertyNames = r.GetPropertyNamesReferenced();

            // Assert
            Assert.IsNotNull(propertyNames);
            Assert.AreEqual(1, propertyNames.Count);
            Assert.AreEqual("APropertyName", propertyNames[0]);

        }
        #endregion

    }
}
