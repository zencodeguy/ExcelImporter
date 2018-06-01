# ExcelImporter
The purpose of the ExcelImporter is to speed development of etl tasks where an Excel spreadsheet file is the data source. By passing the Importer class a path and file name and an ImportDefinition class, the Importer will return an ImportResult object. This object will contain a collection of ImportedRow objects, each one representing a parsed row from the source. It also contains a collection of error messages that were created while reading and parsing the file.
You create the ImportDefinition using the ImportDefinitionFactory static class’s Create method. The source for this a text string that defines, in English phrases, the layout of the excel file, the data types of the columns, and some validation parameters. Rules can be written to verify the content of each column, even depending on the contents of other columns for the rule. This grammer is referred to as Import Definition Language. 
Alternatively, you can programmatically create the import definition.
Because you can map the resulting records from a spreadsheet to either a class or a database table using the included Mapper classes, the terms “column” and “property” are used interchangeably when discussing the destination of an import.

ExcelImporter uses the ExcelDataReader project (https://github.com/ExcelDataReader/ExcelDataReader) to actually read the Excel source. As such, the ExcelImporter tools can also be used to import comma separated value files.


