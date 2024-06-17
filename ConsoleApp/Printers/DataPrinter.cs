namespace ConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DataPrinter
    {
        public void Print(IList<DataSourceObject> dataSource)
        {
            var typesToPrint = new HashSet<string> {"DATABASE", "GLOSSARY", "DOMAIN" };

            foreach (var dataSourceObject in dataSource.Where(x => typesToPrint.Contains(x.Type)).OrderBy(x => x.Type))
            {
                PrintDataSourceObject(dataSourceObject);
                PrintChildren(dataSource, dataSourceObject);
            }

            Console.ReadKey();
        }

        private void PrintDataSourceObject(DataSourceObject dataSourceObject, int indentLevel = 0)
        {
            string indent = new string('\t', indentLevel);
            string title = !string.IsNullOrEmpty(dataSourceObject.Title) ? dataSourceObject.Title : "No title";
            string description = !string.IsNullOrEmpty(dataSourceObject.Description) ? dataSourceObject.Description : "No Description";

            if (indentLevel == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }

            Console.WriteLine($"{indent}{dataSourceObject.Type} '{dataSourceObject.Name} ({title})'");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"{indent}{description}");
            Console.ResetColor();
        }

        private void PrintChildren(IList<DataSourceObject> dataSource, DataSourceObject parentObject)
        {
            var childrenGroups = dataSource
                .Where(x => x.ParentId == parentObject.Id && x.ParentType == parentObject.Type)
                .GroupBy(x => x.Type);

            foreach (var childrenGroup in childrenGroups)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"\t{childrenGroup.Key}S ({childrenGroup.Count()}):");
                Console.ResetColor();

                foreach (var child in childrenGroup.OrderBy(x => x.Name))
                {
                    PrintDataSourceObject(child, 2); // 2 tabs for children

                    // Direct sub-children like columns, parameters, values
                    PrintSubChildren(dataSource, child);
                }
            }
        }

        private void PrintSubChildren(IList<DataSourceObject> dataSource, DataSourceObject parentObject)
        {
            var subChildrenGroups = dataSource
                .Where(x => x.ParentId == parentObject.Id && x.ParentType == parentObject.Type)
                .GroupBy(x => x.Type);

            foreach (var subChildrenGroup in subChildrenGroups)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"\t\t\t{subChildrenGroup.Key}S ({subChildrenGroup.Count()}):");
                Console.ResetColor();

                foreach (var subChild in subChildrenGroup.OrderBy(x => x.Name))
                {
                    PrintDataSourceObject(subChild, 4); // 4 tabs for sub-children
                }
            }
        }
    }
}
