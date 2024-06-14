namespace ConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DataPrinter
    {
        public void Print(IList<DataSourceObject> dataSource)
        {
            //var uniqueDataSourceObjects = dataSource.GroupBy(x => x.Type).Select(g => g.First()).OrderBy(x => x.Type).ToList();

            foreach (var dataSourceObject in dataSource.OrderBy(x => x.Type))
            {
                switch (dataSourceObject.Type)
                {
                    case "AREA":
                    case "TERM":
                    case "DATABASE":
                    case "GLOSSARY":
                    case "DOMAIN":
                        PrintDataSourceObject(dataSourceObject);

                        // Direct children of database like tables, procedures, lookups
                        PrintChildren(dataSource, dataSourceObject);

                        break;
                }
            }

            Console.ReadKey();
        }

        private void PrintDataSourceObject(DataSourceObject dataSourceObject, int indentLevel = 0) // 0 as default of \t
        {
            string indent = new string('\t', indentLevel);
            string title = string.IsNullOrEmpty(dataSourceObject.Title) ? "No Title" : dataSourceObject.Title;
            string description = string.IsNullOrEmpty(dataSourceObject.Description) ? "No Description" : dataSourceObject.Description;

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
