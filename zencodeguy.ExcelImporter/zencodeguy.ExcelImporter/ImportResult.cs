using System.Collections.Generic;
using System.Linq;

namespace zencodeguy.ExcelImporter
{
    public class ImportResult
    {
        public ImportResult()
        {
            this.rows = new List<ImportedRow>();
        }

        private List<ImportedRow> rows;

        public List<ImportedRow> Rows
        {
            get
            {
                return this.rows;
            }
        }

        public List<string> ErrorMessages
        {
            get
            {
                var l = new List<string>();
                foreach(var r in this.Rows)
                {
                    l.AddRange(r.ErrorMessages);
                }
                return l;
            }
        }

        public bool IsValid
        {
            get
            {
                return this.Rows.Where(x => x.ErrorMessages.Count != 0).Count() == 0;
            }
        }
    }
}
