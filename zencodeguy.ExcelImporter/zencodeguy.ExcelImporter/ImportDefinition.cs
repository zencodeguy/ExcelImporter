using System.Collections.Generic;
using System.Linq;

using zencodeguy.ExcelImporter.Rules;

namespace zencodeguy.ExcelImporter
{
    public sealed class ImportDefinition
    {
        public ImportDefinition()
        {
            this.Columns = new List<ColumnDefinition>();
            this.DestinationProperties = new List<DestinationPropertyDefinition>();
            this.Rules = new List<Rule>();
            this.FileType = Enums.FileTypes.EXCEL;
        }
        public List<ColumnDefinition> Columns { get; set; }

        public List<DestinationPropertyDefinition> DestinationProperties { get; set; }

        public List<Rule> Rules { get; set; }
        public bool HeaderRow { get; set; }
        public Enums.FileTypes FileType { get; set; }
        public string TableName { get; set; }
     
        public bool ContainsSubstitutions()
        {
            return this.DestinationProperties.Where(x => x.Substitute == true).Count() > 0;
        }
    }
}
