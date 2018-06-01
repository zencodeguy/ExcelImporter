using System;
using System.Collections.Generic;

using zencodeguy.ExcelImporter.Rules;

namespace zencodeguy.ExcelImporter.Parsers
{
    public static class RuleWhenParser
    {
        public static void Parse(string Line, ImportDefinition ID)
        {
            Helpers.ParametersValid(Line, ID, "RULE-WHEN");

            var index = Line.IndexOf(' ') + 1;

            var rulename = Line.GetNextWord(index);
            index += rulename.Length + 1;

            var r = new WhenRuleThen(rulename);

            while (index < Line.Length)
            {
                index = ParseWhenRulePhrase(Line, index, r, ID);
            }

            ID.Rules.Add(r);
        }

        private static int ParseWhenRulePhrase(string line, int index, WhenRuleThen r, ImportDefinition id)
        {
            bool createRequiredRule = false;

            var propertyName = line.GetNextWord(index);
            index += propertyName.Length + 1;
            if (propertyName == "THEN")
            {
                r.Then();
                propertyName = line.GetNextWord(index);
                index += propertyName.Length + 1;
            }
            r.Property(propertyName);

            var opAsString = line.GetNextWord(index);
            index += opAsString.Length + 1;
            if (opAsString == "REQUIRED")
            {
                createRequiredRule = true;
                opAsString = line.GetNextWord(index);
                index += opAsString.Length + 1;
            }

            Enums.ComparisonOperators compop = Enums.ComparisonOperators.EQ;
            try
            {
                compop = (Enums.ComparisonOperators)Enum.Parse(typeof(Enums.ComparisonOperators), opAsString);
            }
            catch
            {
                throw new InvalidOperationException("Operation type is not a valid value: " + opAsString);
            }

            if (compop == Enums.ComparisonOperators.EQ ||
                compop == Enums.ComparisonOperators.NOTEQ)
            {
                var extraction = line.ExtractDelimitedSection('{', '}', index);
                index += extraction.Item2 + 3; // Includes opening delimiter, closing delimiter, and whitespace
                if (string.IsNullOrWhiteSpace(extraction.Item1))
                {
                    throw new Exception("Rule " + r.RuleName + " contains a valid value that is an empty string.");
                }

                if (compop == Enums.ComparisonOperators.EQ)
                {
                    r.Equals(extraction.Item1);
                }
                else if (compop == Enums.ComparisonOperators.NOTEQ)
                {
                    r.NotEquals(extraction.Item1);
                }

            }

            if (compop == Enums.ComparisonOperators.IN ||
                compop == Enums.ComparisonOperators.NOTIN)
            {
                var extraction = line.ExtractDelimitedSection('{', '}', index);
                List<string> values = extraction.Item1.ExtractListItems('"', '"', ',', 0);
                index += extraction.Item2 + 3;

                foreach (var v in values)
                {
                    if (string.IsNullOrWhiteSpace(v))
                    {
                        throw new Exception("Rule " + r.RuleName + " contains a valid value that is an empty string.");
                    }
                }

                if (compop == Enums.ComparisonOperators.IN)
                {
                    r.In(values);
                }
                else if (compop == Enums.ComparisonOperators.NOTIN)
                {
                    r.NotIn(values);
                }
            }

            if (createRequiredRule)
            {
                var reqRule = new WhenRequiredRule(propertyName + "-WHEN-REQUIRED");
                reqRule.WhenComparisons.AddRange(r.WhenComparisons);
                reqRule.Required(propertyName);
                id.Rules.Add(reqRule);
            }

            return index;
        }

    }
}
