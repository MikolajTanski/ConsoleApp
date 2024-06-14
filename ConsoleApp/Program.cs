namespace ConsoleApp
{
    internal class Program
    {
        private static void Main()
        {
            var dataLoader = new DataLoader();
            var dataPrinter = new DataPrinter();
            var dataMatcher = new DataMatcher();
            var parser = new Parser(dataLoader, dataPrinter, dataMatcher);

            parser.Do("Data\\sampleFile1.csv", "Data\\dataSource.csv");
            //parser.Do("sampleFile2.csv", "dataSource.csv");
            //parser.Do("sampleFile3.csv", "dataSource.csv");
        }
    }
}
