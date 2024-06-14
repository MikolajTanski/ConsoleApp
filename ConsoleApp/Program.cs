namespace ConsoleApp
{
    internal class Program
    {
        private static void Main()
        {
            var reader = new Parser();
            //reader.Do("sampleFile1.csv", "dataSource.csv");
            //reader.Do("sampleFile2.csv", "dataSource.csv");
            reader.Do("sampleFile3.csv", "dataSource.csv");
        }
    }
}
