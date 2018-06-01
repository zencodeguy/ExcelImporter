using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using zencodeguy.ExcelImporter.Rules;

namespace zencodeguy.ExcelImporter.Tests.Rules
{
    [TestClass]
    public class ValueRuleTests
    {
        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionWhenRuleNameIsNull()
        {
            var rule = new ValueRule(null);
        }

        [TestMethod]
        public void ConstructorPopulatesRuleName()
        {
            // Arrange
            var ruleName = "This is a rule name.";
            // Act
            var rule = new ValueRule(ruleName);
            // Assert
            Assert.AreEqual(ruleName, rule.RuleName);
        }
        #endregion

        #region PropertyName Tests
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PropertyNameThrowsExceptionWhenPropertyNameCalledTwice()
        {
            // Arrange
            var rule = new ValueRule("TestRule").Property("ANane").Equals("A Value");

            // Act
            rule.Property("BName");
        }

        private void TestPropertyNameWithNullEquivalent(string Value)
        {
            // Arrange
            var rule = new ValueRule("TestRule");

            // Act
            rule.Property(Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyNameThrowsExceptionWhenPropertyNameIsNull()
        {
            TestPropertyNameWithNullEquivalent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyNameThrowsExceptionWhenPropertyNameIsEmptyString()
        {
            TestPropertyNameWithNullEquivalent(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PropertyNameThrowsExceptionWhenPropertyNameIsWhiteSpace()
        {
            TestPropertyNameWithNullEquivalent("   ");
        }

        [TestMethod]
        public void PropertyNameSetsPropertyName()
        {
            // Arrange
            var r = new ValueRule("ARuleName");

            // Act
            r.Property("APropertyName");

            // Assert
            Assert.AreEqual("APropertyName", r.PropertyName);
        }
        #endregion

        #region Operator Setup tests
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EqualsThrowsExceptionIfComparisonAlreadySetup()
        {
            // Arrange
            var rule = new ValueRule("A Rule").Property("A name").Equals("A Value");

            // Act
            rule.Equals("B Value");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NotEqualsThrowsExceptionIfComparisonAlreadySetup()
        {
            // Arrange
            var rule = new ValueRule("A Rule").Property("A name").Equals("A Value");

            // Act
            rule.NotEquals("B Value");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InThrowsExceptionIfComparisonAlreadySetup()
        {
            // Arrange
            var rule = new ValueRule("A Rule").Property("A name").Equals("A Value");

            // Act
            rule.In(new List<string>(1) { "A Value" });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NotInThrowsExceptionIfComparisonAlreadySetup()
        {
            // Arrange
            var rule = new ValueRule("A Rule").Property("A name").Equals("A Value");

            // Act
            rule.NotIn(new List<string>(1) { "A Value" });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EqualsThrowsExceptionIfPropertyNameNotSet()
        {
            // Arrange
            var rule = new ValueRule("TestRule");

            // Act
            rule.Equals("A Value");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NotEqualsThrowsExceptionIfPropertyNameIsNotSet()
        {
            // Arrange
            var rule = new ValueRule("TestRule");

            // Act
            rule.NotEquals("A Value");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InThrowsExceptionIfPropertyNameIsNotSet()
        {
            // Arrange
            var rule = new ValueRule("TestRule");

            // Act
            rule.In(new List<string>(){ "A Value" });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NotInThrowsExceptionIfPropertyNameIsNotSet()
        {
            // Arrange
            var rule = new ValueRule("TestRule");

            // Act
            rule.NotIn(new List<string>() { "A Value" });
        }

        private void TestEqualsWithNullEquivalent(string Value)
        {
            // Arrange
            var r = new ValueRule("RuleName").Property("APropertyName");

            // Act
            r.Equals(Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EqualsThrowsExceptionIsValueIsNull()
        {
            TestEqualsWithNullEquivalent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EqualsThrowsExceptionIsValueIsEmptyString()
        {
            TestEqualsWithNullEquivalent(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EqualsThrowsExceptionIsValueIsWhiteSpace()
        {
            TestEqualsWithNullEquivalent("   ");
        }

        private void TestNotEqualsWithNullEquivalent(string Value)
        {
            // Arrange
            var r = new ValueRule("RuleName").Property("APropertyName");

            // Act
            r.NotEquals(Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotEqualsThrowsExceptionIfValueIsNull()
        {
            TestNotEqualsWithNullEquivalent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotEqualsThrowsExceptionIfValueIsEmptyString()
        {
            TestNotEqualsWithNullEquivalent(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotEqualsThrowsExceptionIfValueIsWhiteSpace()
        {
            TestNotEqualsWithNullEquivalent("   ");
        }

        private void TestINWithNullEquivalent(string Value)
        {
            // Arrange
            var r = new ValueRule("ARuleName").Property("APropertyName");

            // Act
            r.In(new List<string>(1) { Value });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InThrowsExceptionIfAllValuesAreNull()
        {
            TestINWithNullEquivalent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InThrowsExceptionIfAllValuesAreEmptyString()
        {
            TestINWithNullEquivalent(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void InThrowsExceptionIfAllValuesAreWhiteSpace()
        {
            TestINWithNullEquivalent("   ");
        }

        private void TestNOTINWithNullEquivalent(string Value)
        {
            // Arrange
            var r = new ValueRule("ARuleName").Property("APropertyName");

            // Act
            r.NotIn(new List<string>(1) { Value });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotInThrowsExceptionIfAllValuesAreNull()
        {
            TestNOTINWithNullEquivalent(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotInThrowsExceptionIfAllValuesAreEmptyString()
        {
            TestNOTINWithNullEquivalent(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NotInThrowsExceptionIfAllValuesAreWhiteSpace()
        {
            TestNOTINWithNullEquivalent("   ");
        }

        [TestMethod]
        public void EqualsCreatedValueComparison()
        {
            // Arrange
            var r = new ValueRule("ARuleName").Property("APropertyName");

            // Act
            var returnedRule = r.Equals("A Value");

            // Assert
            Assert.AreSame(r, returnedRule);
            Assert.IsNotNull(r.Comparison);
            Assert.AreEqual("APropertyName", r.Comparison.PropertyName);
            Assert.AreEqual(Enums.ComparisonOperators.EQ, r.Comparison.ComparisonOperator);
            Assert.AreEqual(1, r.Comparison.ValidValues.Count);
            Assert.AreEqual("A Value", r.Comparison.ValidValues[0]);
        }

        [TestMethod]
        public void NotEqualsCreatedValueComparison()
        {
            // Arrange
            var r = new ValueRule("ARuleName").Property("APropertyName");

            // Act
            var returnedRule = r.NotEquals("A Value");

            // Assert
            Assert.AreSame(r, returnedRule);
            Assert.IsNotNull(r.Comparison);
            Assert.AreEqual("APropertyName", r.Comparison.PropertyName);
            Assert.AreEqual(Enums.ComparisonOperators.NOTEQ, r.Comparison.ComparisonOperator);
            Assert.AreEqual(1, r.Comparison.ValidValues.Count);
            Assert.AreEqual("A Value", r.Comparison.ValidValues[0]);
        }

        [TestMethod]
        public void InCreatesValueComparison()
        {
            // Arrange
            var r = new ValueRule("ARuleName").Property("APropertyName");

            // Act
            var returnedRule = r.In(new List<string>(2) { "A Value", "B Value" });

            // Assert
            Assert.AreSame(r, returnedRule);
            Assert.IsNotNull(r.Comparison);
            Assert.AreEqual("APropertyName", r.Comparison.PropertyName);
            Assert.AreEqual(Enums.ComparisonOperators.IN, r.Comparison.ComparisonOperator);
            Assert.AreEqual(2, r.Comparison.ValidValues.Count);
            Assert.AreEqual("A Value", r.Comparison.ValidValues[0]);
            Assert.AreEqual("B Value", r.Comparison.ValidValues[1]);
        }

        [TestMethod]
        public void NotInCreatesValueComparison()
        {
            // Arrange
            var r = new ValueRule("ARuleName").Property("APropertyName");

            // Act
            var returnedRule = r.NotIn(new List<string>(2) { "A Value", "B Value" });

            // Assert
            Assert.AreSame(r, returnedRule);
            Assert.IsNotNull(r.Comparison);
            Assert.AreEqual("APropertyName", r.Comparison.PropertyName);
            Assert.AreEqual(Enums.ComparisonOperators.NOTIN, r.Comparison.ComparisonOperator);
            Assert.AreEqual(2, r.Comparison.ValidValues.Count);
            Assert.AreEqual("A Value", r.Comparison.ValidValues[0]);
            Assert.AreEqual("B Value", r.Comparison.ValidValues[1]);
        }

        [TestMethod]
        public void InSanitizesValueList()
        {
            // Arrange
            var r = new ValueRule("ARuleName").Property("APropertyName");

            // Act
            var returnedRule = r.In(new List<string>(5) { "A Value", null, string.Empty, "   ", "B Value" });

            // Assert
            Assert.AreSame(r, returnedRule);
            Assert.IsNotNull(r.Comparison);
            Assert.AreEqual("APropertyName", r.Comparison.PropertyName);
            Assert.AreEqual(Enums.ComparisonOperators.IN, r.Comparison.ComparisonOperator);
            Assert.AreEqual(2, r.Comparison.ValidValues.Count);
            Assert.AreEqual("A Value", r.Comparison.ValidValues[0]);
            Assert.AreEqual("B Value", r.Comparison.ValidValues[1]);
        }

        [TestMethod]
        public void NotInSanitizesValueList()
        {
            // Arrange
            var r = new ValueRule("ARuleName").Property("APropertyName");

            // Act
            var returnedRule = r.NotIn(new List<string>(5) { "A Value", null, string.Empty, "   ", "B Value" });

            // Assert
            Assert.AreSame(r, returnedRule);
            Assert.IsNotNull(r.Comparison);
            Assert.AreEqual("APropertyName", r.Comparison.PropertyName);
            Assert.AreEqual(Enums.ComparisonOperators.NOTIN, r.Comparison.ComparisonOperator);
            Assert.AreEqual(2, r.Comparison.ValidValues.Count);
            Assert.AreEqual("A Value", r.Comparison.ValidValues[0]);
            Assert.AreEqual("B Value", r.Comparison.ValidValues[1]);
        }

        #endregion

        #region IsValid Tests
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsValidThrowsExceptionWhenPropertyNameIsNotSet()
        {
            // Arrange
            var rule = new ValueRule("Test Rule");
            // Act
            rule.IsValid(new Dictionary<string, string>(1) { { "APropertyName", "A Value" } });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void IsValidThrowsExceptionWhenComparisonNotSet()
        {
            // Arrange
            var rule = new ValueRule("TestRule").Property("APropertyName");
            // Act
            rule.IsValid(new Dictionary<string, string>(1) { { "APropertyName", "A Value" } });
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenRequiredButValueIsNull()
        {
            // Arrange
            var rule = new ValueRule("TestRule").Property("APropertyName").Equals("Value").Required();
            // Act
            var result = rule.IsValid(new Dictionary<string, string>(1) { { "APropertyName", null } });
            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("APropertyName is required, but is null, an empty string, or whitespace.",
                rule.ErrorMessage);
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenRequiredButValueIsEmptyString()
        {
            // Arrange
            var rule = new ValueRule("TestRule").Property("APropertyName").Equals("Value").Required();
            // Act
            var result = rule.IsValid(new Dictionary<string, string>(1) { { "APropertyName", string.Empty } });
            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("APropertyName is required, but is null, an empty string, or whitespace.",
                rule.ErrorMessage);
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenRequiredButValueIsWhiteSpace()
        {
            // Arrange
            var rule = new ValueRule("TestRule").Property("APropertyName").Equals("Value").Required();
            // Act
            var result = rule.IsValid(new Dictionary<string, string>(1) { { "APropertyName", "    " } });
            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("APropertyName is required, but is null, an empty string, or whitespace.",
                rule.ErrorMessage);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenNotRequiredAndValueIsNull()
        {
            // Arrange
            var rule = new ValueRule("TestRule").Property("APropertyName").Equals("Value");
            // Act
            var result = rule.IsValid(new Dictionary<string, string>(1) { { "APropertyName", null } });
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenNotRequiredAndValueIsEmptyString()
        {
            // Arrange
            var rule = new ValueRule("TestRule").Property("APropertyName").Equals("Value");
            // Act
            var result = rule.IsValid(new Dictionary<string, string>(1) { { "APropertyName", string.Empty } });
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenNotRequiredAndValueIsWhiteSpace()
        {
            // Arrange
            var rule = new ValueRule("TestRule").Property("APropertyName").Equals("Value");
            // Act
            var result = rule.IsValid(new Dictionary<string, string>(1) { { "APropertyName", "    " } });
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenEqualsIsTrue()
        {
            // Arrange
            var rule = new ValueRule("TestRule").Property("APropertyName").Equals("I Am Valid");
            // Act
            var result = rule.IsValid(new Dictionary<string, string>(1) { { "APropertyName", "I Am Valid" } });
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenNotEqualsIsTrue()
        {
            // Arrange
            var rule = new ValueRule("TestRule").Property("APropertyName").NotEquals("I Am Valid");
            // Act
            var result = rule.IsValid(new Dictionary<string, string>(1) { { "APropertyName", "I Am NOT Valid" } });
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenInIsTrue()
        {
            // Arrange
            var rule = new ValueRule("TestRule").Property("APropertyName").In(new List<string>() { "one", "two", "three" });
            // Act
            var result = rule.IsValid(new Dictionary<string, string>(1) { { "APropertyName", "two" } });
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidReturnsTrueWhenNotInIsTrue()
        {
            // Arrange
            var rule = new ValueRule("TestRule").Property("APropertyName").NotIn(new List<string>() { "one", "two", "three" });
            // Act
            var result = rule.IsValid(new Dictionary<string, string>(1) { { "APropertyName", "four" } });
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenEqualsIsFalse()
        {
            // Arrange
            var rule = new ValueRule("TestRule").Property("APropertyName").Equals("hello");
            // Act
            var result = rule.IsValid(new Dictionary<string, string>(1) { { "APropertyName", "goodbye" } });
            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("APropertyName value of 'goodbye' is invalid.",
                rule.ErrorMessage);
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenNotEqualsIsFalse()
        {
            // Arrange
            var rule = new ValueRule("TestRule").Property("APropertyName").NotEquals("hello");
            // Act
            var result = rule.IsValid(new Dictionary<string, string>(1) { { "APropertyName", "hello" } });
            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("APropertyName value of 'hello' is invalid.",
                rule.ErrorMessage);
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenInIsFalse()
        {
            // Arrange
            var rule = new ValueRule("TestRule").Property("APropertyName").In(new List<string>() { "one", "two", "three" });
            // Act
            var result = rule.IsValid(new Dictionary<string, string>(1) { { "APropertyName", "four" } });
            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("APropertyName value of 'four' is invalid.",
                rule.ErrorMessage);
        }

        [TestMethod]
        public void IsValidReturnsFalseWhenNotInIsFalse()
        {
            // Arrange
            var rule = new ValueRule("TestRule").Property("APropertyName").NotIn(new List<string>() { "one", "two", "three" });
            // Act
            var result = rule.IsValid(new Dictionary<string, string>(1) { { "APropertyName", "two" } });
            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual("APropertyName value of 'two' is invalid.",
                rule.ErrorMessage);
        }

        #endregion
    }
}
