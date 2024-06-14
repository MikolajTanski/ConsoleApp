﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace ConsoleApp
{
    public class DataLoader
    {
        
        public IList<DataSourceObject> Load(string dataSource)
        {
            IList<DataSourceObject> dataSourceObjects = new List<DataSourceObject>();
            using (var parser = new TextFieldParser(dataSource))
            {
                parser.SetDelimiters(";");
                parser.ReadLine(); 

                while (!parser.EndOfData)
                {
                    var values = parser.ReadFields();
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

       
        public IList<ImportedObject> Import(string fileToImport)
        {
            IList<ImportedObject> importedObjects = new List<ImportedObject>();
            using (var reader = new StreamReader(fileToImport))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(';');
                    var importedObject = new ImportedObject
                    {
                        Type = values[0],
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
            }

            return importedObjects;
        }
    }
}