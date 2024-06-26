﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Helpers;
using Microsoft.VisualBasic.FileIO;

namespace ConsoleApp
{
    public class DataLoader
    {
        private readonly LogHelper _logHelper;

        public DataLoader(LogHelper logHelper)
        {
            _logHelper = logHelper;
        }

        public async Task<IList<DataSourceObject>> LoadAsync(string dataSource)
        {
            IList<DataSourceObject> dataSourceObjects = new List<DataSourceObject>();

            using (var parser = new TextFieldParser(dataSource))
            {
                parser.SetDelimiters(";");
                parser.ReadLine();

                while (!parser.EndOfData)
                {
                    var values = await Task.Run(() => parser.ReadFields());
                    var dataSourceObject = new DataSourceObject
                    {
                        Id = Convert.ToInt32(values[0]),
                        Type = values[1],
                        Name = values[2],
                        Schema = values[3],
                        ParentId = !string.IsNullOrEmpty(values[4]) ? Convert.ToInt32(values[4]) : 0,
                        ParentType = values[5],
                        Title = values[6],
                        Description = values[7],
                        CustomField1 = values[8],
                        CustomField2 = values[9],
                        CustomField3 = values[10]
                    };


                    dataSourceObjects.Add(dataSourceObject);
                }
            }

            return dataSourceObjects;
        }

        public async Task<IList<ImportedObject>> ImportAsync(string fileToImport)
        {
            IList<ImportedObject> importedObjects = new List<ImportedObject>();

            using (var reader = new StreamReader(fileToImport))
            {
                string line;
                int lineNumber = 0;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lineNumber++;
                    try
                    {
                        var values = line.Split(';');

                        var importedObject = new ImportedObject
                        {
                            Type = values[0].ToUpper(),
                            Name = values[1],
                            Schema = values[2],
                            ParentName = values[3],
                            ParentType = values[4],
                            ParentSchema = values[5],
                            Title = values[6],
                            Description = values[7],
                            CustomField1 = values[8],
                            CustomField2 = values[9],
                            CustomField3 = values[10]
                        };

                        importedObjects.Add(importedObject);
                    }
                    catch (Exception ex)
                    {
                        _logHelper.LogError("Import data error: " + ex.Message, lineNumber, line);
                    }
                }

                _logHelper.WriteLogEntries();
            }

            return importedObjects;
        }
    }
}
