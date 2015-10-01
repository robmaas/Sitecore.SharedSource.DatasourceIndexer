using Sitecore.ContentSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.SharedSource.DatasourceIndexer.ContentSearch.SearchTypes
{
    public class DatasourceSearchResultItem : Sitecore.ContentSearch.SearchTypes.SearchResultItem
    {
        [IndexField("datasource_content")]
        public string DatasourceContent { get; set; }
    }
}