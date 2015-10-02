# Datasource Indexer

<b>The Sitecore.SharedSource.DatasourceIndexer makes it possible to retrieve search results by content from items referenced by datasources.</b>

A widely used technique is the use of shared content blocks on pages.
The problem with this technique is the content which is displayed in these blocks.

A typical question from your customer: "I'm searching the website with the term '{BlockTitle}' but my page is not returned, but the content is obviously on the page."

From a technical point of view this makes a lot of sense, because the concept of 'Pages' don't really exist in Sitecore. Sitecore.ContentSearch searches through items instead.

This module is a solution to this problem.
It indexes fields on related datasource items into the indexed item, and results in those 'missing pages' to be returned from the search query!

<hr />

# Installation
The module is available on NuGet with id: Sitecore.ContentSearch.DatasourceIndexer (https://www.nuget.org/packages/Sitecore.SharedSource.DatasourceIndexer)
- Install Nuget Package
- Install Sitecore package (/Sitecore_Package/Sitecore.SharedSource.DatasourceIndexer-*.zip)

The Sitecore package installs a Field Section into the "/sitecore/templates/System/Layout/Sections/Rendering Options" item.
As a result, any rendering-definition (Sublayout, MVC Controller Rendering, Webcontrol) will have a new field available: "Index Datasource Fields"
- Publish the above item.

![](http://content.screencast.com/users/RMaas/folders/Jing/media/298ea2a1-7dee-47bd-890c-24708d9640e0/2015-10-02_1006.png)

# Usage
Of course you wouldn't want to index any field on any datasource item, you want to have control of what will be indexed.
For example: You have a Sublayout which renders the introduction of a related news-article. You (probably) wouldn't want to index the news-article's Content, but just the introduction-field (because only the introduction is visible on the page).

You can provide fields (pipe-separated) in the "Index Datasource Fields"-field of the rendering-definition to configure which fields from the datasource item will be indexed (screenshot above).

# Configuration
By default the datasource content will be indexed to a field named "datasourcecontent". This can be configured in the App_Config\Include\Sitecore.SharedSource.DatasourceIndexer\Sitecore.SharedSource.DatasourceIndexer.config file.
This is a simple patch to add the ComputedIndexField.
It would be possible to change this field to "_content" to include the datasources-content to the regular index content field. I personally like to have a bit more control, so I separated the content.

<b>Devices</b><br />
By default this module will only work for renderings on the Default device. If you want to control which devices are processed simply make a change to the "Sitecore.SharedSource.DatasourceIndexer:Devices" setting in the same config file.<br />You may enter pipe-separated ID's of the Devices ("{0000-...}|{0000-xxx}")<br />
If the setting is empty, it will use the Default device. If any ID has been provided, it will ONLY use the provided ID's.c

# Dependencies
To make sure any item which has datasource-content is updated correctly when making changes to those datasources I've added a processor to the 'indexing:getDependencies'-pipeline. It uses the Link-database to get all related items and adds them to the Indexing queue.

# Example
Enough talk, let's get some examples!
In the screenshot above we've confiured the Block-rendering to index the 'Title' and the 'Content' field from the datasource. <br />
I've made a 'Block', which is a shared content item.<br />
![](http://content.screencast.com/users/RMaas/folders/Jing/media/71de04ae-5ddd-429b-bfef-7681f9d62587/2015-10-02_1028.png)
<br />
Added the Block-rendering to a page called BlockPage, and selected the Block2-datasource item.

By default Sitecore ContentSearch will not return my BlockPage when I use the term 'module'. But this is where this module comes in:<br />
I've added a sample SearchResultItem-class (Sitecore.SharedSource.DatasourceIndexer.ContentSearch.SearchTypes.DatasourceSearchResultItem) to explain how to search the index content. <br />
``` c#
string term = Request["s"];

var index = Sitecore.ContentSearch.ContentSearchManager.GetIndex("sitecore_web_index");
using (var ctx = index.CreateSearchContext())
{
    var searchResults = ctx.GetQueryable<Sitecore.SharedSource.DatasourceIndexer.ContentSearch.SearchTypes.DatasourceSearchResultItem>();
    searchResults.Where(x => x.Path.StartsWith(Sitecore.Context.Site.RootPath));

    searchResults.Where(x => x.TemplateName == "Page");

    searchResults.Where(x => x.Content.Contains(term) || x.DatasourceContent.Contains(term));
}
```
