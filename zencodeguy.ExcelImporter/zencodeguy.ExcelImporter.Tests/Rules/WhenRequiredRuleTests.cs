using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using zencodeguy.ExcelImporter.Rules;
using System.Collections.Generic;

namespace zencodeguy.ExcelImporter.Tests.Rules
{
    [TestClass]
    public class WhenRequiredRuleTests
    {
        #region Constructor Tests
        private void ConstructorThrowsExceptionWhenRuleNameIsNullEquivalent(string ruleName)
        {
            // Act
            var r = new WhenRequiredRule(ruleName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionWhenRuleNameIsNull()
        {
            ConstructorThrowsExceptionWhenRuleNameIsNullEquivalent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionWhenRuleNameIsEmptyString()
        {
            ConstructorThrowsExceptionWhenRuleNameIsNullEquivalent(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionWhenRuleNameIsWhiteSpace()
        {
            ConstructorThrowsExceptionWhenRuleNameIsNullEquivalent("   ");
        }

        [TestMethod]
        public void ConstructorReturnsRule()
        {
            // Arrange
            var ruleName = "A rule name";

            // Act
            var r = new WhenRequiredRule(ruleName);

            // Assert
            Assert.IsNotNull(r);
            Assert.AreEqual(ruleName, r.RuleName);
            Assert.AreEqual(0, r.WhenComparisons.Count);
            Assert.AreEqual(0, r.RequiredProperties.Count);
        }
        #endregion

        #region Property Tests
        private void PropertyThrowsExceptionOnNullEquivalent(string property)
        {
            // Arrange
            var r = new WhenRequiredRule("ARuleName");

            // Act
            r.Property(property);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyThrowsExceptionWhenPropertyNameIsNull()
        {
            PropertyThrowsExceptionOnNullEquivalent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyThrowsExceptionWhenPropertyNameIsEmptyString()
        {
            PropertyThrowsExceptionOnNullEquivalent(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyThrowsExceptionWhenPropertyNameIsWhiteSpace()
        {
            PropertyThrowsExceptionOnNullEquivalent("   ");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PropertyThrowsExceptionWhenPropertyNameIsSetTwice()
        {
            // Arrange
            var r = new WhenRequiredRule("ARuleName");
            r.Property("APropertyName");

            // Act
            r.Property("BPropertyName");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PropertyThrowsExceptionWhenThenHasBeenCalled()
        {
            // Arrange
            var r = new WhenRequiredRule("ARuleName").Property("AProperty").Equals("AValue").Then();

            // Act
            r.Property("BProperty");
        }
        #endregion

        #region Then Tests
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThenThrowsExceptionIfPropertyDeclaredWithoutComparison()
        {
            // Arrange
            var r = new WhenRequiredRule("ARuleName");
            r.Property("APropertyName");

            // Act
            r.Then();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThenThrowsExceptionIfNoWhenComparisonsDefined()
        {
            // Arrange
            var r = new WhenRequiredRule("ARuleName");

            // Act
            r.Then();
        }
        #endregion

        #region Equals Operators
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EqualsThrowsExceptionWhenPropertyNameNotDeclared()
        {
            // Arrange
            var r = new WhenRequiredRule("ARuleName");

            // Act
            r.Equals("AStringValue");
        }

        private void EqualsThrowsExceptionWhenValueIsNullEquivalent(string Value)
        {
            // Arrange
            var r = new WhenRequiredRule("ARuleName").Property("AProperty");

            // Act
            r.Equals(Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EqualsThrowsExceptionWhenValueIsNull()
        {
            EqualsThrowsExceptionWhenValueIsNullEquivalent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EqualsThrowsExceptionWhenValueIsEmptyString()
        {
            EqualsThrowsExceptionWhenValueIsNullEquivalent(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EqualsThrowsExceptionWhenValueIsWhiteSpace()
        {
            EqualsThrowsExceptionWhenValueIsNullEquivalent("   ");
        }
        #endregion

        #region Not Equals Tests
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NotEqualsThrowsExceptionWhenPropertyNameNotDeclared()
        {
            // Arrange
            var r = new WhenRequiredRule("ARuleName");

            // Act
            r.NotEquals("AStringValue");
        }

        private void NotEqualsThrowsExceptionWhenValueIsNullEquivalent(string Value)
        {
            // Arrange
            var r = new WhenRequiredRule("ARuleName").Property("AProperty");

            // Act
            r.NotEquals(Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotEqualsThrowsExceptionWhenValueIsNull()
        {
            NotEqualsThrowsExceptionWhenValueIsNullEquivalent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotEqualsThrowsExceptionWhenValueIsEmptyString()
        {
            NotEqualsThrowsExceptionWhenValueIsNullEquivalent(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotEqualsThrowsExceptionWhenValueIsWhiteSpace()
        {
            NotEqualsThrowsExceptionWhenValueIsNullEquivalent("   ");
        }
        #endregion

        #region In Tests
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InThrowsExceptionWhenPropertyNameNotDeclared()
        {
            // Arrange
            var r = new WhenRequiredRule("ARuleName");

            // Act
            r.In(new List<string>(1) { "AStringValue" });
        }

        private void InThrowsExceptionWhenValueIsNullEquivalent(string Value)
        {
            // Arrange
            var r = new WhenRequiredRule("ARuleName").Property("AProperty");

            // Act
            r.In(new List<string>(1) { Value });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InThrowsExceptionWhenValueIsNull()
        {
            NotEqualsThrowsExceptionWhenValueIsNullEquivalent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InThrowsExceptionWhenValueIsEmptyString()
        {
            NotEqualsThrowsExceptionWhenValueIsNullEquivalent(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InThrowsExceptionWhenValueIsWhiteSpace()
        {
            NotEqualsThrowsExceptionWhenValueIsNullEquivalent("   ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InThrowsExceptionIfPassedEmptyCollection()
        {
            // Arrange
            var r = new WhenRequiredRule("ARule").Property("AProperty");

            // Act
            r.In(new List<string>());
        }
        #endregion

        #region NotIn Tests
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NotInThrowsExceptionWhenPropertyNameNotDeclared()
        {
            // Arrange
            var r = new WhenRequiredRule("ARuleName");

            // Act
            r.NotIn(new List<string>(1) { "AStringValue" });
        }

        private void NotInThrowsExceptionWhenValueIsNullEquivalent(string Value)
        {
            // Arrange
            var r = new WhenRequiredRule("ARuleName").Property("AProperty");

            // Act
            r.NotIn(new List<string>(1) { Value });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotInThrowsExceptionWhenValueIsNull()
        {
            NotEqualsThrowsExceptionWhenValueIsNullEquivalent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotInThrowsExceptionWhenValueIsEmptyString()
        {
            NotEqualsThrowsExceptionWhenValueIsNullEquivalent(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotInThrowsExceptionWhenValueIsWhiteSpace()
        {
            NotEqualsThrowsExceptionWhenValueIsNullEquivalent("   ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NotInThrowsExceptionIfPassedEmptyCollection()
        {
            // Arrange
            var r = new WhenRequiredRule("ARule").Property("AProperty");

            // Act
            r.In(new List<string>());
        }
        #endregion

        #region Required Tests
        private void RequiredThrowsExceptionOnNullEquivalent(string RequiredProperty)
        {
            // Arrange
            var r = new WhenRequiredRule("ARuleName").Property("AProperty").Equals("AValue").Then();

            // Act
            r.Required(RequiredProperty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RequiredThrowsExceptionOnNull()
        {
            RequiredThrowsExceptionOnNullEquivalent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RequiredThrowsExceptionOnEmptyString()
        {
            RequiredThrowsExceptionOnNullEquivalent(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RequiredThrowsExceptionOnWhiteSpace()
        {
            RequiredThrowsExceptionOnNullEquivalent("   ");
        }

        [TestMethod]
        public void RequiredAddsToRequiredProperties()
        {
            // Arrange
            var r = new WhenRequiredRule("ARuleName").Property("AProperty").Equals("AValue").Then();

            // Act
            r.Required("RequiredProperty");

            // Assert
            Assert.AreEqual(1, r.RequiredProperties.Count);
            Assert.AreEqual("RequiredProperty", r.RequiredProperties[0]);
        }
        #endregion

        #region IsValid tests
        [TestMethod]
        public void IsValidReturnsTrue()
        {
            // Arrange
            var r = new WhenRequiredRule("ARequiredRule").Property("Name").Equals("George").Then().Required("State");
            Dictionary<string, string> Values = new Dictionary<string, string>()
            {
                { "Name", "George" },
                { "State", "This is not null" }
            };

            // Act
            var result = r.IsValid(Values);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(r.ErrorMessage);
        }

        [TestMethod]
        public void IsValidReturnsTrueWithMultipleWhenClauses()
        {
            // Arrange
            var r = new WhenRequiredRule("ARequiredRule").Property("FirstName").Equals("George").Property("LastName").Equals("Washington").Then().Required("State");
            Dictionary<string, string> Values = new Dictionary<string, string>()
            {
                { "FirstName", "George" },
                { "LastName", "Washington" },
                { "State", "This is not null" }
            };

            // Act
            var result = r.IsValid(Values);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(r.ErrorMessage);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenMultiplePropertiesAreTested()
        {
            // Arrange
            var r = new WhenRequiredRule("ARequiredRule").Property("FirstName").Equals("George").Then().Required("State").Required("LastName");
            Dictionary<string, string> Values = new Dictionary<string, string>()
            {
                { "FirstName", "George" },
                { "LastName", "This is not null" },
                { "State", "This is also not null" }
            };

            // Act
            var result = r.IsValid(Values);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(r.ErrorMessage);
        }

        private void IsValidReturnsFalseWhenRequiredPropertyIsNullEquivalent(string missingValue)
        {
            // Arrange
            var r = new WhenRequiredRule("ARequiredRule").Property("Name").Equals("George").Then().Required("State");
            Dictionary<string, string> Values = new Dictionary<string, string>()
            {
                { "Name", "George" },
                { "State", missingValue }
            };

            // Act
            var result = r.IsValid(Values);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("The following properties are required but not provided: State", r.ErrorMessage);
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenRequirdPropertyIsNull()
        {
            IsValidReturnsFalseWhenRequiredPropertyIsNullEquivalent(null);
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenRequirdPropertyIsEmptyString()
        {
            IsValidReturnsFalseWhenRequiredPropertyIsNullEquivalent(string.Empty);
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenRequirdPropertyIsWhiteSpace()
        {
            IsValidReturnsFalseWhenRequiredPropertyIsNullEquivalent("  ");
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenMultiplePropertiesAreTestedAndOneFails()
        {
            // Arrange
            var r = new WhenRequiredRule("ARequiredRule").Property("FirstName").Equals("George").Then().Required("State").Required("LastName");
            Dictionary<string, string> Values = new Dictionary<string, string>()
            {
                { "FirstName", "George" },
                { "LastName", null },
                { "State", "This is also not null" }
            };

            // Act
            var result = r.IsValid(Values);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("The following properties are required but not provided: LastName", r.ErrorMessage);
        }

        #endregion

        #region GetPropertyNamesReferenced Tests
        [TestMethod]
        public void GetPropertyNamesReferencedReturnsValue()
        {
            // Arrange
            var r = new WhenRequiredRule("ARuleName").Property("Name").Equals("George").Then().Required("City").Required("State");

            // Act
            var properties = r.GetPropertyNamesReferenced();

            // Assert
            Assert.AreEqual(3, properties.Count);
            Assert.IsTrue(properties.Contains("Name"));
            Assert.IsTrue(properties.Contains("City"));
            Assert.IsTrue(properties.Contains("State"));
        }
        #endregion


    }
}
