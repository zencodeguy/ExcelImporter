using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using zencodeguy.ExcelImporter.Rules;

namespace zencodeguy.ExcelImporter.Tests.Rules
{
    [TestClass]
    public class RequiredRuleTests
    {
        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionIfRuleNameIsNull()
        {
            // Act
            var r = new RequiredRule(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionIfRuleNameIsEmptyString()
        {
            // Act
            var r = new RequiredRule(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionIfRuleNameIsWhiteSpace()
        {
            // Act
            var r = new RequiredRule("   ");
        }

        [TestMethod]
        public void ConstructorReturnsRequiredRule()
        {
            // Arrange
            var ruleName = "Required Rule Name";

            // Act
            var r = new RequiredRule(ruleName);

            // Assert
            Assert.IsNotNull(r);
            Assert.AreEqual(ruleName, r.RuleName);
            Assert.IsNull(r.PropertyName);
            Assert.IsNull(r.ErrorMessage);
        }
        #endregion

        #region PropertyName
        private void TestPropertyWithNullEquivalent(string Value)
        {
            // Arrange
            var r = new RequiredRule("Test Rule Name");

            // Act
            r.Property(Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyThrowsExceptionIfPropertyNameIsNull()
        {
            TestPropertyWithNullEquivalent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyThrowsExceptionIfPropertyNameIsEmptyString()
        {
            TestPropertyWithNullEquivalent(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyThrowsExceptionIfPropertyNameIsWhiteSpace()
        {
            TestPropertyWithNullEquivalent("  ");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PropertyThrowsExceptionIfPropertyNameIsAlreadySet()
        {
            // Arrange
            var pName = "APropertyName";
            var r = new RequiredRule("ARuleName").Property(pName);

            // Act
            r.Property(pName);
        }

        [TestMethod]
        public void PropertySetsPropertyName()
        {
            // Arrange
            var pName = "PropertyName";

            var r = new RequiredRule("TestRule");

            // Act
            var returnedRule = r.Property(pName);

            // Assert
            Assert.AreSame(r, returnedRule);
            Assert.AreEqual(pName, r.PropertyName);
        }
        #endregion

        #region IsValid
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsValidThrowsExceptionIfPropertyNameIsNotSet()
        {
            // Arrange
            var r = new RequiredRule("ARuleName");

            // Act
            r.IsValid(new Dictionary<string, string>() { { "AProperty", "Blah" } });
        }

        private void TestIsValidWithNullEquivalent(string Value)
        {
            // Arrange
            var r = new RequiredRule("ARuleName").Property("APropertyName");

            // Act
            var result = r.IsValid(new Dictionary<string, string>(1) { { "APropertyName", Value } });

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("APropertyName is required, but is null, an empty string, or whitespace.",
                r.ErrorMessage);
        }

        [TestMethod]
        public void IsValidReturnsFalseWithNullValue()
        {
            TestIsValidWithNullEquivalent(null);
        }

        [TestMethod]
        public void IsValidReturnsFalseWithEmptyString()
        {
            TestIsValidWithNullEquivalent(string.Empty);
        }

        [TestMethod]
        public void IsValidReturnsFalseWithWhiteSpace()
        {
            TestIsValidWithNullEquivalent("    ");
        }

        [TestMethod]
        public void IsValidReturnsTrueWithNonNullString()
        {
            // Arrange
            var r = new RequiredRule("ARuleName").Property("APropertyName");

            // Act
            var result = r.IsValid(new Dictionary<string, string>(1) { { "APropertyName", "This is not a null string." } });

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(r.ErrorMessage);
        }
        #endregion

        #region GetPropertyNamesReferenced
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetPropertyNamesReferencedThrowsExceptionIfPropertyNameNotSet()
        {
            // Arrange
            var r = new RequiredRule("ARuleName");

            // Act
            var pNames = r.GetPropertyNamesReferenced();
        }

        [TestMethod]
        public void GetPropertyNamesReferencedReturnsValues()
        {
            // Arrange
            var r = new RequiredRule("ARuleName").Property("ARequiredProperty");

            // Act
            var pNames = r.GetPropertyNamesReferenced();

            // Assert
            Assert.IsNotNull(pNames);
            Assert.AreEqual(1, pNames.Count);
            Assert.AreEqual("ARequiredProperty", pNames[0]);
        }
        #endregion



    }
}
