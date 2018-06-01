using System;
using System.Collections.Generic;
using zencodeguy.ExcelImporter.Rules;

namespace zencodeguy.ExcelImporter.Parsers
{
    public static class RuleIsDefinitionParser
    {
        public static void Parse(string Line, ImportDefinition ID)
        {
            Helpers.ParametersValid(Line, ID, "RULE-IS");

            var index = Line.IndexOf(' ') + 1;

            var rulename = Line.GetNextWord(index);
            index += rulename.Length + 1;

            var r = new ValueRule(rulename);

            // This is either the property name, or it may be "REQUIRED"
            var nw = Line.GetNextWord(index);
            index += nw.Length + 1;
            if (nw.Trim() == "REQUIRED")
            {
                r.Required();
                nw = Line.GetNextWord(index);
                index += nw.Length + 1;
            }

            r.Property(nw);

            var opAsString = Line.GetNextWord(index);
            index += opAsString.Length + 1;

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
                var extraction = Line.ExtractDelimitedSection('{', '}', index);
                if (string.IsNullOrWhiteSpace(extraction.Item1))
                {
                    throw new Exception("Rule " + rulename + " contains a valid value that is an empty string.");
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
                var extraction = Line.ExtractDelimitedSection('{', '}', index);
                List<string> values = extraction.Item1.ExtractListItems('"', '"', ',', 0);
                foreach (var v in values)
                {
                    if (string.IsNullOrWhiteSpace(v))
                    {
                        throw new Exception("Rule " + rulename + " contains a valid value that is an empty string.");
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

            ID.Rules.Add(r);
        }
    }
}
