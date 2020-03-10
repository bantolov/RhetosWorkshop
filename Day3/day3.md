# Day 3

Topics:

1. [Filters](#filters)
2. [Validations](#validations)
3. [REST Web API](#rest-web-api)
4. [Low-level database concepts](#low-level-database-concepts)

## Filters

We have mentioned simple filters previously.
This topic will cover a complete overview of filters in Rhetos.

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/Filters-and-other-read-methods>

Contents:

* Filters
  * **Demonstrate** ItemFilter, ComposableFilterBy and predefined filters.
  * Note that InvalidData, Lock and other similar concepts can use both ComposableFilterBy or ItemFilter.
* Other read methods
  * **Demonstrate** FilterBy.
  * You can **skip** the Query concept for now, it is rarely used.
* Development guidelines and advanced topics
  * When not to write filters
  * Using additional data or system components in filters
  * Filter name is Filter parameter type
    * For example `System.String[]`.
  * Combining filters
    * You can **skip** this chapter for now.

Assignment:

1. Read the article <https://github.com/Rhetos/Rhetos/wiki/Filters-and-other-read-methods>.
2. Add at least one example of each concept to the Bookstore application:
   ItemFilter, ComposableFilterBy and FilterBy.

## Validations

We have mentioned simple validations previously.
This topic will cover a complete overview of validations in Rhetos.

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/Data-validations>

Contents:

* Property value constraints
  * We have already covered this previously in the workshop.
* InvalidData concept
  * Basic and most commonly used concept for validations.
  * **Demonstrate** a validation, review the REST response.
* Understanding data validations in Rhetos
  * The principles behind InvalidData.
  * What is and what is not a data validation.
* Additional validation examples
  * Read a few examples of more complex filters to specify a validations.
* Error response and metadata
  * Just a short overview:
    The error response can be customized with additional metadata or programmable error message.
    For example, the metadata is commonly used to show the error on web form next to the specified property.

## REST Web API

`Rhetos.RestGenerator` plugin creates a RESTful JSON web API for all entities and other
readable or writable data structures that are specified in a Rhetos application.
Additionally it allows executing custom actions and downloading reports.

Documentation:

* <https://github.com/Rhetos/RestGenerator/blob/master/Readme.md>

Contents:

* Introduction
  * Rhetos supports custom plugins that generate web API, for example REST, SOAP and OData
    are all available on GitHub.
    Most popular plugin is `Rhetos.RestGenerator`.
  * SOAP API is currently shipped with the Rhetos framework core.
  * Review RhetosPackages.config, it should contain Rhetos.RestGenerator.
* We have already tested the generated REST web API in previous workshop topics.
  * If needed, **demonstrate** GET request in the browser address bar.
  * If needed, **demonstrate** GET and POST request in a browser extension
    (see [instruction](https://github.com/Rhetos/Rhetos/wiki/Create-your-first-Rhetos-application#test-and-review)).

Assignments:

1. Review RhetosPackages.config, it should contain Rhetos.RestGenerator.
2. Read the article <https://github.com/Rhetos/RestGenerator/blob/master/Readme.md>
3. Use the address bar in the browser to get the following data
   1. All records from an entity
   2. Records from an entity with an ItemFilter applied
   3. Records from an entity with an ComposableFilterBy with a parameter
   4. Read data with a generic property filter
   5. Paging: Read data from a second page, while displaying 5 records per page.
4. Use any browser REST plugin to modify the data
   (see [instruction](https://github.com/Rhetos/Rhetos/wiki/Create-your-first-Rhetos-application#test-and-review)):
   1. POST
   2. PUT
   3. DELETE

## Low-level database concepts

Low-level database concepts allow developers to directly control the database structure,
in order to achieve specific performance optimizations,
or implement features that are not readily available as DSL concepts.

Documentation:

1. <https://github.com/Rhetos/Rhetos/wiki/Database-objects>
2. <https://github.com/Rhetos/Rhetos/wiki/SqlObject-concept>

Contents:

* How to (and how not to) modify the database
  * Why should developers avoid direct modifications of database structure,
    outside of Rhetos deployment process.
  * Best practices: Custom object that are not controlled by Rhetos could be placed in a `dbo` schema.
  * A good rule of thumb is to *avoid* these low-level concepts if you are implementing a standard business pattern.
    Seeing low-level concepts in DSL scripts is a sign that we are not looking at a standard feature, but **a very specific uncommon feature**.
    For example, if the purpose of the feature is "data validation", please use the InvalidData concept instead.
  * Breaking down the requirements to a set of **standard business features** is a good way to make your software more maintainable.
    Also consider developing a new concept if you need to implement a standard business pattern, but there is no existing concept available.
* SqlProcedure
* SqlTrigger
* SqlFunction
* SqlView
* SqlIndex
* SqlDefault
* SqlObject
  * Common usage is for creating complex database indexes (covering properties, filtered index),
    and full-text search index (cannot be created inside a transaction).
* Dependencies between database objects

TODO: LegacyEntity tutorial with examples (issue #309)

Assignments:

1. Read the article <https://github.com/Rhetos/Rhetos/wiki/Database-objects>
2. Read the article <https://github.com/Rhetos/Rhetos/wiki/SqlObject-concept>
3. Create an SqlView (don't forget SqlDependsOn or AutoDetectSqlDependencies),
   and use it from another SqlQueryable.

Assignment notes and common issues:

* What is the difference between SqlView and SqlQueryable?
  SqlView and SqlQueryable are similar concepts, because they both create a view in the database.
  SqlQueryable additionally creates C# class, Entity Framework mapping and Web API.
  Use SqlView when you only want to extract a common SQL subquery that can be used
  in multiple database objects (for example, in multiple SqlQueryable objects).
  Use SqlQueryable if you need to access this data directly from the application code.
