using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsoleApp.Helpers;

namespace ConsoleApp
{
    public class DataMatcher
    {
        private readonly LogHelper _logHelper;

        public DataMatcher(LogHelper logHelper)
        {
            _logHelper = logHelper;
        }

        public async Task MatchAndUpdateAsync(IList<ImportedObject> importedObjects, IList<DataSourceObject> dataSource)
        {
            var tasks = importedObjects.Select(importedObject => UpdateMatchingObjectAsync(importedObject, dataSource));
            await Task.WhenAll(tasks);
            _logHelper.WriteLogEntries();
        }

        private async Task UpdateMatchingObjectAsync(ImportedObject importedObject, IList<DataSourceObject> dataSource)
        {
            var match = await Task.Run(() => dataSource.FirstOrDefault(x =>
                x.Type.ClearEquals(importedObject.Type) &&
                x.Name.ClearEquals(importedObject.Name) &&
                x.Schema.ClearEquals(importedObject.Schema)));

            if (match is null)
            {
                _logHelper.LogError("No match found", null, $"{importedObject.Type},{importedObject.Name},{importedObject.Schema}");
                return;
            }

            if (match.ParentId > 0 && !string.IsNullOrEmpty(importedObject.ParentType))
            {
                if (!await IsParentMatchAsync(importedObject, match, dataSource))
                {
                    _logHelper.LogError("Parent match failed", null, $"{importedObject.Type},{importedObject.Name},{importedObject.Schema}");
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
                x.Type.ClearEquals(match.ParentType)));

            return parent?.Name.ClearEquals(importedObject.ParentName) == true &&
                   parent?.Schema.ClearEquals(importedObject.ParentSchema) == true &&
                   parent?.Type.ClearEquals(importedObject.ParentType) == true;
        }
    }
}
