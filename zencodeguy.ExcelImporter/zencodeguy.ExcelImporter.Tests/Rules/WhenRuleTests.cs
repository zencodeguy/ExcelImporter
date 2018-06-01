using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using zencodeguy.ExcelImporter.Rules;

namespace zencodeguy.ExcelImporter.Tests.Rules
{
    [TestClass]
    public class WhenRuleThenThenTests
    {
        #region Constructor Tests
        [TestMethod]
        public void ConstructorSetsRuleName()
        {
            // Act
            var r = new WhenRuleThen("Rule Name");

            // Assert
            Assert.AreEqual("Rule Name", r.RuleName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionWhenRuleThenNameIsNull()
        {
            // Act
            var r = new WhenRuleThen(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionWhenRuleThenNameIsEmptyString()
        {
            // Act
            var r = new WhenRuleThen(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowsExceptionWhenRuleThenNameIsWhiteSpace()
        {
            // Act
            var r = new WhenRuleThen("   ");
        }
        #endregion

        #region Then tests
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThenThrowsExceptionWhenNoWhenComparisonsSet()
        {
            // Act
            var r = new WhenRuleThen("Test Rule").Then();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ThenThrowsExceptionWhenPriorClauseNotCompleted()
        {
            // Act
            var r = new WhenRuleThen("Test Rule").Property("A Property").Then();
        }
        #endregion

        #region When Equals tests
        [TestMethod]
        public void WhenEqualsThenEqualsTestPasses()
        {
            // Arrange
            var r = new WhenRuleThen("Test Rule").Property("Name").Equals("George").Then().Property("State").Equals("Washington");
            var values = new Dictionary<string, string>()
            {
                { "Name", "George" },
                { "State", "Washington" }
            };

            // Act
            var result = r.IsValid(values);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WhenEqualsThenEqualsTestFails()
        {
            // Arrange
            var r = new WhenRuleThen("Test Rule").Property("Name").Equals("George").Then().Property("State").Equals("Washington");
            var values = new Dictionary<string, string>()
            {
                {  "Name", "George" },
                {  "State", "California" }
            };

            // Act
            var result = r.IsValid(values);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void WhenEqualsTwoComparisonsThenEqualsPasses()
        {
            // Arrange
            var r = new WhenRuleThen("Test Rule").Property("NameFirst").Equals("George").Property("NameLast").Equals("Washington").Then().Property("State").Equals("Washington");
            var values = new Dictionary<string, string>()
            {
                { "NameFirst", "George" },
                { "NameLast", "Washington" },
                { "State", "Washington" }
            };

            // Act
            var result = r.IsValid(values);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WhenEqualsReturnsTrueIfWhenClauseIsNotMet()
        {
            // Arrange
            var r = new WhenRuleThen("Test Rule").Property("NameFirst").Equals("George").Property("NameLast").Equals("Washington").Then().Property("State").Equals("Washington");
            var values = new Dictionary<string, string>()
            {
                { "NameFirst", "Abraham" },
                { "NameLast", "Washington" },
                { "State", "Washington" }
            };

            // Act
            var result = r.IsValid(values);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WhenEqualsThenInPasses()
        {
            // Arrange
            var r = new WhenRuleThen("Test Rule").Property("NameFirst").Equals("George").Then().Property("State").In(new List<string>() { "D.C.", "Delaware", "Maryland" });
            var values = new Dictionary<string, string>()
            {
                { "NameFirst", "George" },
                { "NameLast", "Washington" },
                { "State", "Delaware" }
            };

            // Act
            var result = r.IsValid(values);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void WhenEqualsThenInFails()
        {
            // Arrange
            var r = new WhenRuleThen("Test Rule").Property("NameFirst").Equals("George").Then().Property("State").In(new List<string>() { "D.C.", "Delaware", "Maryland" });
            var values = new Dictionary<string, string>()
            {
                { "NameFirst", "George" },
                { "NameLast", "Washington" },
                { "State", "Alaska" }
            };

            // Act
            var result = r.IsValid(values);

            // Assert
            Assert.IsFalse(result);
        }
        #endregion
    }
}
