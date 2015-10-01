using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.SharedSource.DatasourceIndexer.Utils
{
    public class DSIUtil
    {
        public const string SettingDevices = "Sitecore.SharedSource.DatasourceIndexer:Devices";
        public const string DefaultDevice = "{FE5D7FDF-89C0-4D99-9AA3-B5FBD009C9F3}";

        public const string ControllerParamFields = "Index Datasource Fields";

        public static Item ResolveDatasource(string datasource, Item contextItem)
        {
            if (!string.IsNullOrWhiteSpace(datasource))
            {
                if (Sitecore.Data.ID.IsID(datasource))
                {
                    return contextItem.Database.GetItem(new Sitecore.Data.ID(new Guid(datasource)));
                }
                else if (datasource.StartsWith("/sitecore"))
                {
                    return contextItem.Database.GetItem(datasource);
                }
                else if (datasource.StartsWith("query:"))
                {
                    return contextItem.Axes.SelectSingleItem(datasource.Remove(0, 6));
                }
            }
            return null;
        }
    }
}