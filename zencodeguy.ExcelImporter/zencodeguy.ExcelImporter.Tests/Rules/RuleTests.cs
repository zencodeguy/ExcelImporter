using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using zencodeguy.ExcelImporter.Rules;

namespace zencodeguy.ExcelImporter.Tests.Rules
{
    [TestClass]
    public class RuleTests
    {
        #region Constructor Tests
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionWhenRuleNameIsNull()
        {
            // Act
            var r = new Rule(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionWhenRuleNameIsEmptyString()
        {
            // Act
            var r = new Rule(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionWhenRuleNameIsWhiteSpace()
        {
            // Act
            var r = new Rule("     ");
        }

        [TestMethod]
        public void ConstructorPopulatesRule()
        {
            // Arrange
            var name = "New Rule Name";

            // Act
            var r = new Rule(name);

            // Assert
            Assert.IsNotNull(r);
            Assert.AreEqual(name, r.RuleName);
            Assert.IsTrue(string.IsNullOrWhiteSpace(r.ErrorMessage));
            Assert.IsTrue(string.IsNullOrWhiteSpace(r.PropertyName));
        }
        #endregion

        #region Unimplemented Methods
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void VirtualMethodIsValidThrowsNotImplementedException()
        {
            // Arrange
            var r = new Rule("Rule name");

            // Act
            var result = r.IsValid(new Dictionary<string, string>(1) { { "APropertyName", "A Value" } });
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void VirtualMethodGetPropertyNamesReferencedThrowsNotImplementedException()
        {
            // Arrange
            var r = new Rule("Rule name");

            // Act
            var result = r.GetPropertyNamesReferenced();
        }
        #endregion
    }
}
