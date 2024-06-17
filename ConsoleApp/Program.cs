using ConsoleApp;
using ConsoleApp.Helpers;
using System;
using System.Threading.Tasks;

internal class Program
{
    private static async Task Main()
    {
        string logFilePath = "Data\\log_data.csv";
        var logHelper = new LogHelper(logFilePath);
        var dataLoader = new DataLoader(logHelper);
        var dataPrinter = new DataPrinter();
        var dataMatcher = new DataMatcher(logHelper);
        var parser = new Parser(dataLoader, dataPrinter, dataMatcher);

        Console.WriteLine("Choose a file to import:");
        Console.WriteLine("1 - sampleFile1.csv");
        Console.WriteLine("2 - sampleFile2.csv");
        Console.WriteLine("3 - sampleFile3.csv");

        var key = Console.ReadKey();
        Console.WriteLine();

        string fileToImport = null;

        switch (key.Key)
        {
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
                fileToImport = "Data\\sampleFile1.csv";
                break;
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
                fileToImport = "Data\\sampleFile2.csv";
                break;
            case ConsoleKey.D3:
            case ConsoleKey.NumPad3:
                fileToImport = "Data\\sampleFile3.csv";
                break;
        }

        if (fileToImport != null)
        {
            await parser.ProcessDataAsync(fileToImport, "Data\\dataSource.csv");
        }
    }
}
