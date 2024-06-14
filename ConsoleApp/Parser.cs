namespace ConsoleApp
{
    using Microsoft.VisualBasic.FileIO;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Parser
    {
        private readonly DataLoader _dataLoader;
        private IList<ImportedObject> ImportedObjects;
        private IList<DataSourceObject> DataSource;
        
        public Parser(DataLoader dataLoader)
        {
            _dataLoader = dataLoader;
        }

        public void Do(string fileToImport, string dataSource)
        {
            ImportedObjects = _dataLoader.Import(fileToImport);
            DataSource = _dataLoader.Load(dataSource);
            MatchAndUpdate();
            Print();
        }

        private void Print()
        {
            foreach (var dataSourceObject in this.DataSource.OrderBy(x => x.Type))
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
                        var childrenGroups = this.DataSource
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
                                var subChildrenGroups = this.DataSource
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

        private void MatchAndUpdate()
        {
            foreach (var importedObject in this.ImportedObjects)
            {
                var match = this.DataSource.FirstOrDefault(x =>
                    x.Type == importedObject.Type &&
                    x.Name == importedObject.Name &&
                    x.Schema == importedObject.Schema);

                if (match is null)
                {
                    continue;
                }

                if (match.ParentId > 0 && !string.IsNullOrEmpty(importedObject.ParentType))
                {
                    var parent = this.DataSource.FirstOrDefault(x =>
                        x.Id == match.ParentId &&
                        x.Type == match.ParentType);

                    if (parent?.Name != importedObject.ParentName
                        || parent?.Schema != importedObject.ParentSchema
                        || parent?.Type != importedObject.ParentType)
                    {
                        continue;
                    }
                }
                
                match.Title = importedObject.Title;
                match.Description = importedObject.Description;
                match.CustomField1 = importedObject.CustomField1;
                match.CustomField2 = importedObject.CustomField2;
                match.CustomField3 = importedObject.CustomField3;
            }
        }
    }
}
