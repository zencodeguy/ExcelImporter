using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using zencodeguy.ExcelImporter.Rules;

namespace zencodeguy.ExcelImporter.Tests.Rules
{
    [TestClass]
    public class StringMaximumLengthRuleTests
    {
        #region ConstructorTests
        private void TestConstructorWithNullEquivalent(string Value)
        {
            // Act
            var r = new StringMaximumLengthRule(Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionWhenRuleNameIsNull()
        {
            TestConstructorWithNullEquivalent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionWhenRuleNameIsEmptyString()
        {
            TestConstructorWithNullEquivalent(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionWhenRuleNameIsWhiteSpace()
        {
            TestConstructorWithNullEquivalent("   ");
        }

        [TestMethod]
        public void ConstructorReturnsRule()
        {
            // Arrange
            var ruleName = "ARuleName";

            // Act
            var r = new StringMaximumLengthRule(ruleName);

            // Assert
            Assert.IsNotNull(r);
            Assert.AreEqual(ruleName, r.RuleName);
            Assert.IsNull(r.PropertyName);
            Assert.IsNull(r.ErrorMessage);
            Assert.IsFalse(r.Length.HasValue);
        }
        #endregion

        #region Property Tests
        private void TestPropertyWithNullEquivalent(string Value)
        {
            // Arrange
            var r = new StringMaximumLengthRule("RuleName");

            // Act
            r.Property(Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyThrowsExceptionWhenNull()
        {
            TestPropertyWithNullEquivalent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyThrowsExceptionWhenEmptyString()
        {
            TestPropertyWithNullEquivalent(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyThrowsExceptionWhenWhiteSpace()
        {
            TestPropertyWithNullEquivalent("   ");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PropertyThrowsExceptionIfAlreadySet()
        {
            // Arrange
            var r = new StringMaximumLengthRule("RuleName").Property("AProperty");

            // Act
            r.Property("AnotherName");
        }

        [TestMethod]
        public void PropertySetsPropertyName()
        {
            // Arrange
            var r = new StringMaximumLengthRule("ARuleName");
            var pName = "APropertyName";

            // Act
            var returnedRule = r.Property(pName);

            // Assert
            Assert.AreEqual(returnedRule, r);
            Assert.AreEqual(pName, r.PropertyName);
        }
        #endregion

        #region MaximumLength
        [TestMethod]
        public void MinimumLengthSetsLength()
        {
            // Arrange
            var r = new StringMaximumLengthRule("ARuleName").Property("APropertyName");

            // Act
            var returnedRule = r.MaximumLength(28);

            // Assert
            Assert.AreSame(returnedRule, r);
            Assert.AreEqual(28, r.Length);
        }
        #endregion

        #region IsValid
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsValidThrowsExceptionIfPropertyNameNotSet()
        {
            // Arrange
            var r = new StringMaximumLengthRule("ARuleName");

            // Act
            r.IsValid(new Dictionary<string, string>(1) { { "APropertyName", "A big string" } });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsValidThrowsExceptionIfMinimumLengthNotSet()
        {
            // Arrange
            var r = new StringMaximumLengthRule("ARuleName").Property("APropertyName");

            // Act
            r.IsValid(new Dictionary<string, string>(1) { { "APropertyName", "This is also a big string." } });
        }

        private void IsValidReturnsTrueIfNullEquivalent(string Value)
        {
            // Arrange
            var r = new StringMaximumLengthRule("ARuleName").Property("APropertyName").MaximumLength(10);

            // Act
            var result = r.IsValid(new Dictionary<string, string>(1) { { "APropertyName", Value } });

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(r.ErrorMessage);
        }

        [TestMethod]
        public void IsValidReturnsTrueIfNull()
        {
            IsValidReturnsTrueIfNullEquivalent(null);
        }

        [TestMethod]
        public void IsValidReturnsTrueIfEmptyString()
        {
            IsValidReturnsTrueIfNullEquivalent(string.Empty);
        }

        [TestMethod]
        public void IsValidReturnsTrueIfWhiteSpace()
        {
            IsValidReturnsTrueIfNullEquivalent("  ");
        }

        private void TestIsValid(int lengthRequired, int lengthToTest, bool valid)
        {
            // Arrange
            var testString = "X".PadRight(lengthToTest);
            var r = new StringMaximumLengthRule("ARuleName").Property("APropertyName").MaximumLength(lengthRequired);

            // Act
            var result = r.IsValid(new Dictionary<string, string>(1) { { "APropertyName", testString } });

            // Assert
            if (valid)
            {
                Assert.IsTrue(result);
                Assert.IsNull(r.ErrorMessage);
            }
            else
            {
                Assert.IsFalse(result);
                Assert.AreEqual(r.PropertyName + " value is " + lengthToTest.ToString() +
                    " characters, which is longer than the maximum required length of " +
                    lengthRequired.ToString() + " characters.",
                    r.ErrorMessage);
            }
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenStringIsExactlyMaximumLength()
        {
            TestIsValid(30, 30, true);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenStringIsExactlyOneCharacterShortOfMaximum()
        {
            TestIsValid(30, 29, true);
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenStringIsExactlyOneCharacterOverMaximum()
        {
            TestIsValid(30, 31, false);
        }
        #endregion
    }
}
