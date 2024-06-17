namespace ConsoleApp
{

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Parser
    {
        private readonly DataPrinter _dataPrinter;
        private readonly DataLoader _dataLoader;
        private readonly DataMatcher _dataMatcher;

        public Parser(DataLoader dataLoader, DataPrinter dataPrinter, DataMatcher dataMatcher)
        {
            _dataLoader = dataLoader;
            _dataPrinter = dataPrinter;
            _dataMatcher = dataMatcher;
        }

        public async Task ProcessDataAsync(string fileToImport, string dataSource)
        {
            var importTask = _dataLoader.ImportAsync(fileToImport);
            var loadTask = _dataLoader.LoadAsync(dataSource);

            await Task.WhenAll(importTask, loadTask);

            var importedObjects = await importTask;
            var dataSourceObjects = await loadTask;

            await _dataMatcher.MatchAndUpdateAsync(importedObjects, dataSourceObjects);
            _dataPrinter.Print(dataSourceObjects);
        }
    }
}
