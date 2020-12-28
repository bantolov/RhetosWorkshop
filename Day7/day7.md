# Day 7

Topics:

1. [Unit testing](#unit-testing)
2. [Implementing business logic in a separate library](#implementing-business-logic-in-a-separate-library)
3. [Temporal data and change history](#temporal-data-and-change-history)
4. [Reporting](#reporting)
5. [Full-text search](#full-text-search)
6. [Internationalization and labels](#internationalization-and-labels)
7. [Rhetos on GitHub](#rhetos-on-github)

## Unit testing

Principles and techniques for developing unit tests on Rhetos applications
with Visual Studio Unit Testing Framework.

Documentation:

* Bookstore demo application
  * Overview of unit tests
    <https://github.com/Rhetos/Bookstore/blob/master/Readme.md#unit-testing>
  * Integration testing project
    [test/Bookstore.Service.Test](https://github.com/Rhetos/Bookstore/tree/master/test/Bookstore.Service.Test)
* TODO: "Unit testing" walkthrough (issue #314)

Contents:

* Introduction
  * Strictly speaking, we will develop *integration tests*, not pure unit tests,
    on VS unit testing framework.
  * We will use same technology as "playground" console application from
    [Using the Domain Object Model](https://github.com/Rhetos/Rhetos/wiki/Using-the-Domain-Object-Model).
  * **Demonstrate** adding a new unit testing projects in Bookstore application.
    See [test/Bookstore.Service.Test](https://github.com/Rhetos/Bookstore/tree/master/test/Bookstore.Service.Test)
    for code structure and references.
  * **Demonstrate** writing a simple unit test: add a book and two comments,
    expect the cache entity value BookInfo.NumberOfComments to be 2.
* Rhetos.ProcessContainer (a successor to RhetosTestContainer)
  * Unit tests should reuse a single static instance of `ProcessContainer`, to avoid running
    initialization code for each test (Entity Framework startup and plugin discovery).
  * **Develop** a helper class that initializes `ProcessContainer`
    (for example [BookstoreContainer](https://github.com/Rhetos/Bookstore/blob/ad7a1dddb99c266cb12a1a7d496bc8129464dc76/test/Bookstore.Service.Test/Tools/BookstoreContainer.cs))
    and use it in unit tests
    (see Bookstore [tests](https://github.com/Rhetos/Bookstore/blob/ad7a1dddb99c266cb12a1a7d496bc8129464dc76/test/Bookstore.Service.Test/BookTest.cs)).
  * Each TransactionScopeContainer instance represents a separate atomic database transaction
    (similar to a single web request).
* Rhetos.TestCommon.TestUtility class best practices
  * Use TestUtility.Dump and TestUtility.DumpSorted to format your expected result,
    and compare expected report vs. actual report.
    This will result with more concise code and more informative error message,
    especially for lists (the alternative is to make separate asserts for count and for each element values).
  * Use TestUtility.ShouldFail with exception type instead of ExpectedExceptionAttribute.
    It provide a better control over the error analysis
    (multiple substrings in error messages or manual exception analysis)
    and better control over execution flow
    (for example, the attribute could report false positive if the correct exception
    was thrown, but on the wrong line in test).
* Principles for writing maintainable unit tests (independence)
  * Test should prepare its own test data.
    It should be able to run on empty database, after deploying the application
    (it will include data created by data-migration scripts and similar features).
  * Test should not be affected by the existing data in database.
    It should be able to run on an existing database from the shared test environment.
  * Test should not change the existing data or leave new data in the database
    (rollback by default, or custom cleanup in specific circumstances).
* Testing user permissions
  * IUserInfo mock
  * Directly execute filters for read and write row permissions
  * Testing server commands with ProcessingEngine may help with testing end-user experience,
    because it includes both basic permissions (claims) and row permissions.
    It is usually better to directly test row permissions filters, because that
    provides a smaller scope for what is tested.

## Implementing business logic in a separate library

Documentation:

* Chapter "Using external code when developing actions" in article
  [Action concept](https://github.com/Rhetos/Rhetos/wiki/Action-concept#Using-external-code-when-developing-actions).

Contents:

* Instead of implementing large blocks of C# code in DSL script (in Action or ComposableFilterBy, e.g.),
  you can simply call some method implemented in a custom class.
* The custom class can be implemented directly in Rhetos application (Rhetos v4 and later). The main benefit for
  writing custom code in a separate class instead of in DSL script is that IntelliSense is available.
  IntelliSense even includes generated code, such as repository classes and Entity Framework model.
* Alternatively, the custom class can be implemented in an external library, with one of the following
  design options to prevent circular dependency between the Rhetos application's code and the external library:
  * A) External library does not reference the generated code from the Rhetos application (e.g. the repository classes).
    This is a common solution for generic algorithms implementation.
    C# code in DSL scripts and Rhetos application can directly reference the generated library (old Rhetos applications
    with DeployPackages need to use ExternalReference concept).
    The external library may provided interfaces that can be implemented by entities and other
    data sources in DSL scripts, to simplify reading and writing data without directly referencing the generated code
    (concepts Implements, ImplementsQueryable and RegisteredImplementation, and GenericRepository helper class).
    See RatingSystem example with external algorithm [implementation](https://github.com/Rhetos/Bookstore/blob/master/src/Bookstore.Algorithms/RatingSystem.cs)
    called from DSL [script](https://github.com/Rhetos/Bookstore/blob/master/src/Bookstore.Service/DslScripts/BookRating.rhe).
  * B) External library references the Rhetos application (to use repository classes, e.g.),
    but the Rhetos application cannot reference the library to avoid circular reference.
    This can be implemented by using reflection in Rhetos application to dynamically call the external library's methods.
    With Rhetos v4 and later it is simpler to implement the custom classes directly in the Rhetos application instead.

## Temporal data and change history

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/Temporal-data-and-change-history>

## Reporting

Documentation:

* TODO: Overview of reporting approaches on Rhetos applications (issue #315)
* Basic concepts: ReportData and ReportFile
  * Examples in unit tests [Computations.rhe](https://github.com/Rhetos/Rhetos/blob/master/CommonConcepts/CommonConceptsTest/DslScripts/Computations.rhe)
* TODO: TemplaterReport package is not published on GitHub yet, it requires a paid license.

## Full-text search

Documentation:

* [SqlObject](https://github.com/Rhetos/Rhetos/wiki/SqlObject-concept),
  for creating full-text search catalog and index
  * Example in unit test
    [FullTextSearchTest.rhe](https://github.com/Rhetos/Rhetos/blob/master/CommonConcepts/CommonConceptsTest/DslScripts/FullTextSearchTest.rhe)
  * FTS objects cannot be created in a database transaction.
* Rhetos contains EF LINQ extension methods for FullTextSearch
  * See [DatabaseExtensionFunctions.cs](https://github.com/Rhetos/Rhetos/blob/master/CommonConcepts/Plugins/Rhetos.Dom.DefaultConcepts.Interfaces/DatabaseExtensionFunctions.cs)
  * Alternative integer key instead of GUID ID.
  * Limiting the search subquery results with top_n_by_rank.
* TODO: "Full-text search" tutorial (issue #316)

Contents:

* Short overview of two Rhetos features that can be used to implement FTS:
  * SqlObject without transaction
  * Extension methods for Entity Framework LINQ (implemented in CommonConcepts package),
    allow using FTS in LINQ queries (for example in ComposableFilterBy).

## Internationalization and labels

Two available plugins:

* [Rhetos.I18NFormatter](https://github.com/Rhetos/I18NFormatter/blob/master/Readme.md) -
  Support for generic ASP.NET module, [i18n](https://github.com/turquoiseowl/i18n),
  that works with [GetText / PO](http://en.wikipedia.org/wiki/Gettext) language files.
  I18N localization module can be placed directly on Rhetos application or on a proxy/gateway web application.
* [Rhetos.MvcModelGenerator](https://github.com/Rhetos/MvcModelGenerator) -
  Support for front-end localization with MVC metadata attributed and captions in *resx* files.

## Rhetos on GitHub

Documentation:

* Rhetos plugins
  * [Recommended plugins](https://github.com/Rhetos/Rhetos/wiki#recommended-plugins)
  * Most application development projects have some additional custom concepts
    developed.
* Rhetos documentation
  * [Tutorials and samples](https://github.com/Rhetos/Rhetos/wiki#application-development-with-rhetos)
  * [List of DSL concepts in CommonConcepts](https://github.com/Rhetos/Rhetos/wiki/List-of-DSL-concepts-in-CommonConcepts)
* Support
  * Questions and Issues: [GitHub issues](https://github.com/Rhetos/Rhetos/issues?q=is%3Aissue)
* Rhetos framework development
  * <https://github.com/Rhetos/Rhetos/wiki/Release-management>
    * [Short-term: Milestones](https://github.com/Rhetos/Rhetos/milestones?direction=asc&sort=title&state=open)
    * [Long-term: Roadmap](https://github.com/Rhetos/Rhetos/wiki/Rhetos-platform-roadmap)
    * [Previous: Release notes](https://github.com/Rhetos/Rhetos/blob/master/ChangeLog.md)
  * <https://github.com/Rhetos/Rhetos/wiki/Rhetos-coding-standard>
  * <https://github.com/Rhetos/Rhetos/wiki/How-to-Contribute>

Assignment:

* Read the article <https://github.com/Rhetos/Rhetos/wiki/Rhetos-coding-standard>
