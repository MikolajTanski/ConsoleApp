namespace ConsoleApp
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DataMatcher
    {
        public async Task MatchAndUpdateAsync(IList<ImportedObject> importedObjects, IList<DataSourceObject> dataSource)
        {
            var tasks = importedObjects.Select(importedObject => UpdateMatchingObjectAsync(importedObject, dataSource));
            await Task.WhenAll(tasks);
        }

        private async Task UpdateMatchingObjectAsync(ImportedObject importedObject, IList<DataSourceObject> dataSource)
        {
            var match = await Task.Run(() => dataSource.FirstOrDefault(x =>
                x.Type == importedObject.Type &&
                x.Name == importedObject.Name &&
                x.Schema == importedObject.Schema));

            if (match is null)
            {
                return;
            }

            if (match.ParentId > 0 && !string.IsNullOrEmpty(importedObject.ParentType))
            {
                if (!await IsParentMatchAsync(importedObject, match, dataSource))
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

        private async Task<bool> IsParentMatchAsync(ImportedObject importedObject, DataSourceObject match, IList<DataSourceObject> dataSource)
        {
            var parent = await Task.Run(() => dataSource.FirstOrDefault(x =>
                x.Id == match.ParentId &&
                x.Type == match.ParentType));

            return parent?.Name == importedObject.ParentName &&
                   parent?.Schema == importedObject.ParentSchema &&
                   parent?.Type == importedObject.ParentType;
        }
    }
}
