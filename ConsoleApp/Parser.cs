namespace ConsoleApp
{
    using Microsoft.VisualBasic.FileIO;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

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

        public async Task ProcessDataAsync(string fileToImport, string dataSource)
        {
            ImportedObjects = await _dataLoader.ImportAsync(fileToImport);
            DataSource = await _dataLoader.LoadAsync(dataSource);
            await _dataMatcher.MatchAndUpdateAsync(ImportedObjects, DataSource);
            _dataPrinter.Print(DataSource);
        }
    }
}
