namespace ConsoleApp
{
    internal class Program
    {
        private static void Main()
        {
            DataLoader dataLoader = new DataLoader();
            var parser = new Parser(dataLoader);
            parser.Do("sampleFile1.csv", "dataSource.csv");
            //parser.Do("sampleFile2.csv", "dataSource.csv");
            //parser.Do("sampleFile3.csv", "dataSource.csv");
        }
    }
}
