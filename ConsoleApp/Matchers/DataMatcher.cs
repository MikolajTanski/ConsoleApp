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
                UpdateMatchingObject(importedObject, dataSource);
            }
        }

        private void UpdateMatchingObject(ImportedObject importedObject, IList<DataSourceObject> dataSource)
        {
            var match = dataSource.FirstOrDefault(x =>
                x.Type == importedObject.Type &&
                x.Name == importedObject.Name &&
                x.Schema == importedObject.Schema);

            if (match is null)
            {
                return;
            }

            if (match.ParentId > 0 && !string.IsNullOrEmpty(importedObject.ParentType))
            {
                if (!IsParentMatch(importedObject, match, dataSource))
                {
                    return;
                }
            }

            match.Title = importedObject.Title;
            match.Description = importedObject.Description;
            match.CustomField1 = importedObject.CustomField1;
            match.CustomField2 = importedObject.CustomField2;
            match.CustomField3 = importedObject.CustomField3;
        }

        private bool IsParentMatch(ImportedObject importedObject, DataSourceObject match, IList<DataSourceObject> dataSource)
        {
            var parent = dataSource.FirstOrDefault(x =>
                x.Id == match.ParentId &&
                x.Type == match.ParentType);

            return parent?.Name == importedObject.ParentName &&
                   parent?.Schema == importedObject.ParentSchema &&
                   parent?.Type == importedObject.ParentType;
        }
    }
}
