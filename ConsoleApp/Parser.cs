namespace ConsoleApp
{
    using Microsoft.VisualBasic.FileIO;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public class Parser
    {
        private readonly DataPrinter _dataPrinter;
        private readonly DataLoader _dataLoader;
        private IList<ImportedObject> ImportedObjects;
        private IList<DataSourceObject> DataSource;
        
        public Parser(DataLoader dataLoader, DataPrinter dataPrinter)
        {
            _dataLoader = dataLoader;
            _dataPrinter = dataPrinter;
        }

        public void Do(string fileToImport, string dataSource)
        {
            ImportedObjects = _dataLoader.Import(fileToImport);
            DataSource = _dataLoader.Load(dataSource);
            MatchAndUpdate();
            _dataPrinter.Print(DataSource);
        }

        private void MatchAndUpdate()
        {
            foreach (var importedObject in this.ImportedObjects)
            {
                var match = this.DataSource.FirstOrDefault(x =>
                    x.Type == importedObject.Type &&
                    x.Name == importedObject.Name &&
                    x.Schema == importedObject.Schema);

                if (match is null)
                {
                    continue;
                }

                if (match.ParentId > 0 && !string.IsNullOrEmpty(importedObject.ParentType))
                {
                    var parent = this.DataSource.FirstOrDefault(x =>
                        x.Id == match.ParentId &&
                        x.Type == match.ParentType);

                    if (parent?.Name != importedObject.ParentName
                        || parent?.Schema != importedObject.ParentSchema
                        || parent?.Type != importedObject.ParentType)
                    {
                        continue;
                    }
                }
                
                match.Title = importedObject.Title;
                match.Description = importedObject.Description;
                match.CustomField1 = importedObject.CustomField1;
                match.CustomField2 = importedObject.CustomField2;
                match.CustomField3 = importedObject.CustomField3;
            }
        }
    }
}
