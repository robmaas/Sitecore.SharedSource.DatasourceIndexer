# DatasourceIndexer

<b>The Sitecore.SharedSource.DatasourceIndexer makes it possible to retrieve search results by content from items referenced by datasources.</b>

A widely used technique is the use of shared content blocks on pages.
The problem with this technique is the content which is displayed in these blocks.

A typical question from your customer: "I'm searching the website with the term '{BlockTitle}' but my page is not returned, but the content is obviously on the page."

From a technical point of view this makes a lot of sense, because the concept of 'Pages' don't really exist in Sitecore. Sitecore.ContentSearch searches through items instead.

This module is a solution to this problem.
It indexes fields on related datasource items into the indexed item, and results in those 'missing pages' to be returned from the search query!

Further documentation is available on GitHub (https://github.com/robmaas/Sitecore.SharedSource.DatasourceIndexer).