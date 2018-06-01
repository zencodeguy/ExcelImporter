using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;

namespace zencodeguy.ExcelImporter
{
    // Importer leverages the ExcelDataReader for reading from the 
    // source file.
    // https://github.com/ExcelDataReader/ExcelDataReader
    public sealed class Importer: IDisposable
    {
        private string pathAndFileName;
        private ImportDefinition importDefinition;
        private int currentRow = 0;
        private int fieldCount = 0;
        private FileStream s;
        private IExcelDataReader r;

        public Importer(string PathAndFileName, 
            ImportDefinition ImportDefinition)
        {
            this.importDefinition = ImportDefinition ?? throw new ArgumentNullException("ImportDefinition");
            if(string.IsNullOrWhiteSpace(PathAndFileName))
            {
                throw new ArgumentNullException("PathAndFileName");
            }
            this.pathAndFileName = PathAndFileName;
        }

        public void Open()
        {
            this.s = File.Open(this.pathAndFileName, FileMode.Open, FileAccess.Read);
            this.r = CreateDataReader(s, this.importDefinition.FileType);
            this.fieldCount = this.r.FieldCount;
            if(this.importDefinition.HeaderRow)
            {
                this.r.Read();
            }
        }

        public void Close()
        {
            this.r.Close();
            this.s.Close();
        }

        public ImportResult ReadAll()
        {
            var result = new ImportResult();

            using (this.s = File.Open(this.pathAndFileName, FileMode.Open, FileAccess.Read))
            {
                this.r = CreateDataReader(s, this.importDefinition.FileType);

                this.fieldCount = this.r.FieldCount;

                if (this.importDefinition.HeaderRow)
                {
                    this.r.Read();
                }

                ImportedRow row = this.ReadRow();
                do
                {
                    result.Rows.Add(row);
                    row = this.ReadRow();
                } while (row != null);
            }

            return result;
        }

        public ImportedRow ReadRow()
        {
            var r = new ImportedRow();

            if(this.r.Read())
            {
                this.currentRow++;
                var o = new string[this.fieldCount];
                for (var i = 0; i < this.fieldCount; i++)
                {
                    if (!this.r.IsDBNull(i))
                        o[i] = this.r.GetValue(i).ToString();
                }

                return new ImportedRow(this.currentRow, o, this.importDefinition);
            }
            else
            {
                return null;
            }
        }

        public void Dispose()
        {
            this.Close();
        }

        private IExcelDataReader CreateDataReader(Stream stream, Enums.FileTypes fileType)
        {
            if (fileType == Enums.FileTypes.EXCEL)
            {
                return ExcelReaderFactory.CreateReader(stream);
            }
            else if (fileType == Enums.FileTypes.CSV)
            {
                return ExcelReaderFactory.CreateCsvReader(stream);
            }
            else
            {
                throw new InvalidOperationException("File Import Definition FileType value is invalid. " + fileType.ToString());
            }
        }
    }
}
