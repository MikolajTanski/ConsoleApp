namespace ConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DataPrinter
    {
        public void Print(IList<DataSourceObject> dataSource)
        {
            foreach (var dataSourceObject in dataSource.OrderBy(x => x.Type))
            {
                switch (dataSourceObject.Type)
                {
                    case "DATABASE":
                    case "GLOSSARY":
                    case "DOMAIN":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{dataSourceObject.Type} '{dataSourceObject.Name} ({dataSourceObject.Title})'");
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine(dataSourceObject.Description);
                        Console.ResetColor();

                        // direct children of database like tables, procedures, lookups
                        var childrenGroups = dataSource
                            .Where(x =>
                                x.ParentId == dataSourceObject.Id &&
                                x.ParentType == dataSourceObject.Type)
                            .GroupBy(x => x.Type);

                        foreach (var childrenGroup in childrenGroups)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine($"\t{childrenGroup.Key}S ({childrenGroup.Count()}):");
                            Console.ResetColor();

                            foreach (var child in childrenGroup.OrderBy(x => x.Name))
                            {
                                // direct sub children like columns, parameters, values
                                var subChildrenGroups = dataSource
                                    .Where(x =>
                                        x.ParentId == child.Id &&
                                        x.ParentType == child.Type)
                                    .GroupBy(x => x.Type);

                                Console.WriteLine($"\t\t{child.Schema}.{child.Name} ({child.Title})");
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine($"\t\t{child.Description}");
                                Console.ResetColor();

                                foreach (var subChildrenGroup in subChildrenGroups)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                    Console.WriteLine($"\t\t\t{subChildrenGroup.Key}S ({subChildrenGroup.Count()}):");
                                    Console.ResetColor();

                                    foreach (var subChild in subChildrenGroup.OrderBy(x => x.Name))
                                    {
                                        Console.WriteLine($"\t\t\t\t{subChild.Name} ({subChild.Title})");
                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                        Console.WriteLine($"\t\t\t\t{subChild.Description}");
                                        Console.ResetColor();
                                    }
                                }
                            }
                        }

                        break;
                }
            }

            Console.ReadKey();
        }
    }
}
