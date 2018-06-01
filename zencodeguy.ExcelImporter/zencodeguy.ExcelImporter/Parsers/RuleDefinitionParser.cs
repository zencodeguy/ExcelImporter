using System;
using System.Collections.Generic;

using zencodeguy.ExcelImporter.Rules;

namespace zencodeguy.ExcelImporter.Parsers
{
    public static class RuleDefinitionParser
    {
        public static void Parse(string Line, ImportDefinition ID)
        {
            Helpers.ParametersValid(Line, ID);

            var ruleType = Line.GetNextWord(0);

            if (ruleType == "RULE-IS")
            {
                RuleIsDefinitionParser.Parse(Line, ID);
            }
            else if (ruleType == "RULE-WHEN")
            {
                RuleWhenParser.Parse(Line, ID);
            }
            else if (ruleType == "RULE-REQUIRED-WHEN")
            {
                RuleRequiredWhenParser.Parse(Line, ID);
            }
            else
            {
                throw new ArgumentException("The RULE token " + ruleType + " is not a valid rule type.");
            }
        }
    }
}
