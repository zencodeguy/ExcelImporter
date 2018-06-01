using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using zencodeguy.ExcelImporter.Parsers;
using zencodeguy.ExcelImporter.Rules;

namespace zencodeguy.ExcelImporter.Tests.Parsers
{
    [TestClass]
    public class RuleDefinitionParserTests
    {
        private void RuleDefinitionParserThrowsExceptionWhenLineIsNullEquivalent(string Line)
        {
            try
            {
                RuleDefinitionParser.Parse(Line, new ImportDefinition());
                Assert.Fail("ArgumentNullException expected, not thrown.");
            }
            catch(ArgumentNullException ex)
            {
                Assert.AreEqual("Line", ex.ParamName);
            }
            catch(Exception ex)
            {
                Assert.Fail("ArgumentNullException expected, " +
                    ex.GetType().Name +
                    " thrown instead.");
            }
        }

        [TestMethod]
        public void RuleDefinitionParserThrowsExceptionWhenLineIsNull()
        {
            RuleDefinitionParserThrowsExceptionWhenLineIsNullEquivalent(null);
        }

        [TestMethod]
        public void RuleDefinitionParserThrowsExceptionWhenLineIsEmptyString()
        {
            RuleDefinitionParserThrowsExceptionWhenLineIsNullEquivalent(string.Empty);
        }

        [TestMethod]
        public void RuleDefinitionParserThrowsExceptionWhenLineIsWhiteSpace()
        {
            RuleDefinitionParserThrowsExceptionWhenLineIsNullEquivalent("   ");
        }

        [TestMethod]
        public void RuleDefinitionParserThrowsExceptionWhenImportDefinitionIsNull()
        {
            try
            {
                RuleDefinitionParser.Parse("RULE-IS Property EQ {A Value}", null);
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
        public void RuleDefinitionParserThrowsExceptionWhenRuleTokenIsInvalid()
        {
            // Arrange
            var id = new ImportDefinition();
            var s = "RULE-NOTATYPE RuleName Property EQ {A value}";
            var expectedException = "The RULE token RULE-NOTATYPE is not a valid rule type.";

            // Act
            try
            {
                RuleDefinitionParser.Parse(s, id);
                Assert.Fail("ArgumentException expected, not thrown.");
            }
            catch(ArgumentException ex)
            {
                Assert.AreEqual(expectedException, ex.Message);
            }
        }

        [TestMethod]
        public void RuleDefinitionParserCreatesValueRule()
        {
            // Arrange
            var s = "RULE-IS RuleName Property EQ {Value}";
            var id = new ImportDefinition();

            // Act
            RuleDefinitionParser.Parse(s, id);

            // Assert
            Assert.AreEqual(1, id.Rules.Count);
            var r = (ValueRule)id.Rules[0];
            Assert.AreEqual("RuleName", r.RuleName);
            Assert.AreEqual("Property", r.Comparison.PropertyName);
            Assert.AreEqual(Enums.ComparisonOperators.EQ, r.Comparison.ComparisonOperator);
            Assert.AreEqual(1, r.Comparison.ValidValues.Count);
            Assert.AreEqual("Value", r.Comparison.ValidValues[0]);
        }

        [TestMethod]
        public void RuleDefinitionParserCreatesWhenRuleThen()
        {
            var s = "RULE-WHEN WhenRuleName AProperty EQ { AVALUE } THEN BProperty EQ { BVALUE }";
            var id = new ImportDefinition();

            // Act
            RuleDefinitionParser.Parse(s, id);

            // Act
            Assert.AreEqual(1, id.Rules.Count);
            var r = (WhenRuleThen)id.Rules[0];
            Assert.AreEqual("WhenRuleName", r.RuleName);
            Assert.AreEqual(1, r.WhenComparisons.Count);
            var wc = r.WhenComparisons[0];
            Assert.AreEqual("AProperty", wc.PropertyName);
            Assert.AreEqual(Enums.ComparisonOperators.EQ, wc.ComparisonOperator);
            Assert.AreEqual(1, wc.ValidValues.Count);
            Assert.AreEqual("AVALUE", wc.ValidValues[0]);
            Assert.AreEqual(1, r.ThenComparisons.Count);
            var tc = r.ThenComparisons[0];
            Assert.AreEqual("BProperty", tc.PropertyName);
            Assert.AreEqual(Enums.ComparisonOperators.EQ, tc.ComparisonOperator);
            Assert.AreEqual(1, tc.ValidValues.Count);
            Assert.AreEqual("BVALUE", tc.ValidValues[0]);
        }

        [TestMethod]
        public void RuleDefinitionCreatesRuleRequiredWhen()
        {
            // Arrange
            var s = "RULE-REQUIRED-WHEN RuleName AProperty EQ {AVALUE} THEN {BProperty}";
            var id = new ImportDefinition();

            // Act
            RuleDefinitionParser.Parse(s, id);

            // Assert
            Assert.AreEqual(1, id.Rules.Count);
            var r = (WhenRequiredRule)id.Rules[0];
            Assert.AreEqual(1, r.WhenComparisons.Count);
            var wc = r.WhenComparisons[0];
            Assert.AreEqual("AProperty", wc.PropertyName);
            Assert.AreEqual(Enums.ComparisonOperators.EQ, wc.ComparisonOperator);
            Assert.AreEqual(1, wc.ValidValues.Count);
            Assert.AreEqual("AVALUE", wc.ValidValues[0]);
            Assert.AreEqual(1, r.RequiredProperties.Count);
            Assert.AreEqual("BProperty", r.RequiredProperties[0]);
        }
    }
}
