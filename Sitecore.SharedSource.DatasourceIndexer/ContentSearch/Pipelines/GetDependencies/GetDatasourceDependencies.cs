using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Pipelines.GetDependencies;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.SharedSource.DatasourceIndexer.ContentSearch.Pipelines.GetDependencies
{
    public class GetDatasourceDependencies : Sitecore.ContentSearch.Pipelines.GetDependencies.BaseProcessor
    {
        public override void Process(GetDependenciesArgs context)
        {
            Func<ItemUri, bool> func = null;
            Assert.IsNotNull(context.IndexedItem, "Indexed item is null");
            Assert.IsNotNull(context.Dependencies, "Dependencies not found");
            Item item = (Item)(context.IndexedItem as SitecoreIndexableItem);
            if (item != null)
            {
                if (func == null)
                {
                    func = uri => (bool)((uri != null) && ((bool)(uri != item.Uri)));
                }
                System.Collections.Generic.IEnumerable<ItemUri> source = Enumerable.Where<ItemUri>(from l in Globals.LinkDatabase.GetReferrers(item, FieldIDs.LayoutField) select l.GetSourceItem().Uri, func).Distinct<ItemUri>();
                context.Dependencies.AddRange(source.Select(x => (SitecoreItemUniqueId)x));
            }
        }
    }
}