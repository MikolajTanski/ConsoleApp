namespace ConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DataPrinter
    {
        public async Task PrintAsync(IList<DataSourceObject> dataSource)
        {
            var typesToPrint = new HashSet<string> { "AREA", "TERM", "DATABASE", "GLOSSARY", "DOMAIN" };

            foreach (var dataSourceObject in dataSource.Where(x => typesToPrint.Contains(x.Type)).OrderBy(x => x.Type))
            {
                await PrintDataSourceObjectAsync(dataSourceObject);
                await PrintChildrenAsync(dataSource, dataSourceObject);
            }

            Console.ReadKey();
        }

        private async Task PrintDataSourceObjectAsync(DataSourceObject dataSourceObject, int indentLevel = 0)
        {
            await Task.Run(() =>
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
            });
        }

        private async Task PrintChildrenAsync(IList<DataSourceObject> dataSource, DataSourceObject parentObject)
        {
            var childrenGroups = dataSource
                .Where(x => x.ParentId == parentObject.Id && x.ParentType == parentObject.Type)
                .GroupBy(x => x.Type);

            foreach (var childrenGroup in childrenGroups)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"\t{childrenGroup.Key}S ({childrenGroup.Count()}):");
                Console.ResetColor();

                var tasks = childrenGroup.OrderBy(x => x.Name).Select(child => PrintChildAndSubChildrenAsync(dataSource, child));
                await Task.WhenAll(tasks);
            }
        }

        private async Task PrintChildAndSubChildrenAsync(IList<DataSourceObject> dataSource, DataSourceObject child)
        {
            await PrintDataSourceObjectAsync(child, 2); // 2 tabs for children
            await PrintSubChildrenAsync(dataSource, child);
        }

        private async Task PrintSubChildrenAsync(IList<DataSourceObject> dataSource, DataSourceObject parentObject)
        {
            var subChildrenGroups = dataSource
                .Where(x => x.ParentId == parentObject.Id && x.ParentType == parentObject.Type)
                .GroupBy(x => x.Type);

            foreach (var subChildrenGroup in subChildrenGroups)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"\t\t\t{subChildrenGroup.Key}S ({subChildrenGroup.Count()}):");
                Console.ResetColor();

                var tasks = subChildrenGroup.OrderBy(x => x.Name).Select(subChild => PrintDataSourceObjectAsync(subChild, 4)); // 4 tabs for sub-children
                await Task.WhenAll(tasks);
            }
        }
    }
}
