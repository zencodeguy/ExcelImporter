using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zencodeguy.ExcelImporter
{
    public class ImportDefinitionResult
    {
        private ImportDefinition importDefinition;
        private List<string> errorMessages;

        public ImportDefinitionResult(ImportDefinition ImportDefinition, 
            List<string> ErrorMessages)
        {
            this.importDefinition = ImportDefinition ?? throw new ArgumentNullException("ImportDefinition");
            this.errorMessages = ErrorMessages ?? throw new ArgumentNullException("ErrorMessages");
        }

        public ImportDefinition Definition
        {
            get
            {
                return this.importDefinition;
            }
        }

        public List<string> ErrorMessages
        {
            get
            {
                return this.errorMessages;
            }
        }

        public bool IsValid()
        {
            return this.errorMessages.Count == 0;
        }

    }
}
