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
        private readonly DataMatcher _dataMatcher;
        private IList<ImportedObject> ImportedObjects;
        private IList<DataSourceObject> DataSource;

        public Parser(DataLoader dataLoader, DataPrinter dataPrinter, DataMatcher dataMatcher)
        {
            _dataLoader = dataLoader;
            _dataPrinter = dataPrinter;
            _dataMatcher = dataMatcher;
        }

        public void Do(string fileToImport, string dataSource)
        {
            ImportedObjects = _dataLoader.Import(fileToImport);
            DataSource = _dataLoader.Load(dataSource);
            _dataMatcher.MatchAndUpdate(ImportedObjects, DataSource);
            _dataPrinter.Print(DataSource);
        }
    }
}
