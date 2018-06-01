using System;
using System.Collections.Generic;
using System.Linq;
using zencodeguy.ExcelImporter.Parsers;
using zencodeguy.ExcelImporter.Rules;

namespace zencodeguy.ExcelImporter
{
    public static class ImportDefinitionFactory
    {
        public static ImportDefinitionResult Create(string ImportDefinitionString)
        {
            List<string> errorMessages = new List<string>();
            int lineCount = 0;

            if (string.IsNullOrWhiteSpace(ImportDefinitionString))
            {
                throw new ArgumentNullException("ImportDefinitionString");
            }

            ImportDefinition id = new ImportDefinition()
            {
                HeaderRow = false
            };

            foreach (var line in ImportDefinitionString.ReadLine())
            {
                lineCount++;
                if (!string.IsNullOrWhiteSpace(line))
                {
                    try
                    {
                        ParseLine(line.Trim(), id);
                    }
                    catch (Exception ex)
                    {
                        errorMessages.Add("Line " + 
                            lineCount.ToString() +
                            ": " + 
                            line + 
                            "\n" + 
                            ex.Message);
                    }
                }
            }

            ValidateImportDefinition(id, errorMessages);

            return new ImportDefinitionResult(id, errorMessages);
        }

        private static void ParseLine(string line, ImportDefinition id)
        {
            if(line.StartsWith("#"))
            {
                // This is a comment
                return;
            }
            if (line.StartsWith("COLUMN"))
            {
                ColumnDefinitionParser.Parse(line, id);
                return;
            }
            if(line.StartsWith("FILETYPE"))
            {
                FileTypeParser.Parse(line, id);
                return;
            }
            else if(line.StartsWith("HEADERROW"))
            {
                HeaderRowParser.Parse(line, id);
                return;
            }
            else if (line.StartsWith("TABLE"))
            {
                TableDefinitionParser.Parse(line, id);
                return;
            }
            else if (line.StartsWith("RULE"))
            {
                RuleDefinitionParser.Parse(line, id);
                return;
            }
            else if (line.StartsWith("DESTINATION"))
            {
                DestinationParser.Parse(line, id);
                return;
            }
            else
            {
                throw new ArgumentException("Invalid token at beginning of line: " + line);
            }
        }

        #region Validation
        public static void ValidateImportDefinition(ImportDefinition id, List<string> errorMessages)
        {
            if (string.IsNullOrWhiteSpace(id.TableName))
            {
                errorMessages.Add("Table name is not defined.");
            }

            if (id.Columns.Count == 0)
            {
                errorMessages.Add("No columns are defined.");
            }

            VerifyAllRulesReferenceDefinedColumns(id, errorMessages);
        }

        private static void VerifyAllRulesReferenceDefinedColumns(ImportDefinition id, List<string> errorMessages)
        {
            foreach (var rule in id.Rules)
            {
                var l = rule.GetPropertyNamesReferenced();
                foreach (var propertyName in l)
                {
                    if (id.Columns.Where(x => x.PropertyName == propertyName).Count() == 0)
                    {
                        errorMessages.Add("Rule " + rule.RuleName + " applies to column " + propertyName + ", which is not a defined column.");
                    }
                }
            }
        }
        #endregion
    }
}
