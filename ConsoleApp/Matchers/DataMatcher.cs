namespace ConsoleApp
{
    using System.Collections.Generic;
    using System.Linq;

    public class DataMatcher
    {
        public void MatchAndUpdate(IList<ImportedObject> importedObjects, IList<DataSourceObject> dataSource)
        {
            foreach (var importedObject in importedObjects)
            {
                var match = dataSource.FirstOrDefault(x =>
                    x.Type == importedObject.Type &&
                    x.Name == importedObject.Name &&
                    x.Schema == importedObject.Schema);

                if (match is null)
                {
                    continue;
                }

                if (match.ParentId > 0 && !string.IsNullOrEmpty(importedObject.ParentType))
                {
                    var parent = dataSource.FirstOrDefault(x =>
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
