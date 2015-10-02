# Datasource Indexer

<b>The Sitecore.SharedSource.DatasourceIndexer makes it possible to retrieve search results by content from items referenced by datasources.</b>

A widely used technique is the use of shared content blocks on pages.
The problem with this technique is the content which is displayed in these blocks.

A typical question from your customer: "I'm searching the website with the term '{BlockTitle}' but my page is not returned, but the content is obviously on the page."

From a technical point of view this makes a lot of sense, because the concept of 'Pages' don't really exist in Sitecore. Sitecore.ContentSearch searches through items instead.

This module is a solution to this problem.
It indexes fields on related datasource items into the indexed item, and results in those 'missing pages' to be returned from the search query!

<hr />

<b>Installation</b>
The module is available on NuGet with id: Sitecore.ContentSearch.DatasourceIndexer (https://www.nuget.org/packages/Sitecore.SharedSource.DatasourceIndexer)
- Install Nuget Package
- Install Sitecore package (/Sitecore_Package/Sitecore.SharedSource.DatasourceIndexer-*.zip)

The Sitecore package installs a Field Section into the "/sitecore/templates/System/Layout/Sections/Rendering Options" item.
As a result, any rendering-definition (Sublayout, MVC Controller Rendering, Webcontrol) will have a new field available: "Index Datasource Fields"

<b>Usage</b>
