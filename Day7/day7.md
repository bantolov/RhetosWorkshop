# Day 7

Topics:

1. [Unit testing](#unit-testing)
2. [Implementing business logic in a separate class or library](#implementing-business-logic-in-a-separate-class-or-library)
3. [Temporal data and change history](#temporal-data-and-change-history)
4. [Reporting](#reporting)
5. [Full-text search](#full-text-search)
6. [Internationalization and labels](#internationalization-and-labels)
7. [Rhetos on GitHub](#rhetos-on-github)

## Unit testing

Principles and techniques for developing unit tests on Rhetos apps
with Visual Studio Unit Testing Framework.

Documentation:

* Bookstore demo application
  * Overview of unit tests
    <https://github.com/Rhetos/Bookstore/blob/master/Readme.md#unit-testing>
  * Integration testing project
    [test/Bookstore.Service.Test](https://github.com/Rhetos/Bookstore/tree/master/test/Bookstore.Service.Test)
* TODO: "Unit testing" walkthrough (issue #314)

Contents:

* See the presentation [Unit tests in Rhetos applications](https://omegasoftware365-my.sharepoint.com/:v:/r/personal/bantolovic_omega-software_hr/Documents/Snimke/Interna%20edukacija_%20Unit%20testovi%20u%20Rhetos%20aplikacijama-20210205_100435-Meeting%20Recording.mp4?csf=1&web=1&e=RKaaom)
  (Croatian).
* Introduction
  * Strictly speaking, we will develop *integration tests*, not pure unit tests,
    on VS unit testing framework.
  * We will use same technology as "playground" console application from
    [Using the Domain Object Model](https://github.com/Rhetos/Rhetos/wiki/Using-the-Domain-Object-Model).
* Unit test project
  * **Demonstrate** adding a new unit testing project in Bookstore application.
    * Add a unit test project Bookstore.Services.Test to the Bookstore solution
      (MSTest to use old Rhetos helpers, or XUnit for newer test framework).
    * On the project add a project reference to Bookstore.Service. No need to add Rhetos NuGet packages.
    * Rhetos.TestCommon NuGet package contains some optional helpers for MSTest test projects.
    * See [test/Bookstore.Service.Test](https://github.com/Rhetos/Bookstore/tree/master/test/Bookstore.Service.Test)
      for code structure and references.
  * Unit tests should reuse a single static instance of `RhetosHost`, to avoid running
    slow initialization code for each test (Entity Framework startup and plugins discovery).
    * **Develop** a helper class `TestScope` that initializes `RhetosHost` and use it in unit tests.
      For example, see [TestScope class](https://github.com/Rhetos/Bookstore/tree/master/test/Bookstore.Service.Test/Tools/TestScope.cs)),
      and its usage the Bookstore test [CommonMisspellingValidation](https://github.com/Rhetos/Bookstore/blob/e0e6e555396cd68ad4cea7d7838e78b4c6fa1c90/test/Bookstore.Service.Test/BookTest.cs#L58).
    * RhetosHost will automatically load the references application's runtime configuration,
      such as **database connection** and similar settings, so there is no need to add any
      more dependencies or config files to the test project.
* Typical unit test:
  * **Demonstrate** writing a simple unit test: add a book and two chapters,
    expect the cache entity value BookInfo.NumberOfChapters to be 2:
    [AutomaticallyUpdateNumberOfChapters](https://github.com/Rhetos/Bookstore/blob/e0e6e555396cd68ad4cea7d7838e78b4c6fa1c90/test/Bookstore.Service.Test/BookTest.cs#L23)
  * Note that each scope instance represents a separate atomic database transaction
    (similar to a single web request).
    All operations executed within the a single scope will be committed together or rolled back
    at the and of the scope.
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
  1. Test should prepare its own test data.
     It should be able to run on empty database, after deploying the application
     (it will include data created by data-migration scripts and similar features).
  2. Test should not be affected by the existing data in database.
     It should be able to run on an existing database from the shared test environment.
  3. Test should not change the existing data or leave new data in the database
     (rollback by default, or custom cleanup in specific circumstances).
* Testing user permissions
  * IUserInfo mock
  * Directly execute filters for read and write row permissions
  * Testing server commands with ProcessingEngine may help with testing end-user experience,
    because it includes both basic permissions (claims) and row permissions.
    It is usually better to directly test row permissions filters, because that
    provides a smaller scope for what is tested.
* Review examples in [Bookstore.Service.Test](https://github.com/Rhetos/Bookstore/tree/master/test/Bookstore.Service.Test) demo:
  * A typical **integration** test: Test automatic updates of computed data that uses database view:
    BookTest.AutomaticallyUpdateNumberOfChapters.
  * Test data validation that should throw an **exception** in insert:
    BookTest.CommonMisspellingValidation.
  * Test a filer without using the database (**clean unit test**, not an integrations test):
    BookTest.CommonMisspellingValidation_DirectFilter.
  * Standard unit tests for a more **complex data processing**:
    RatingSystemTest.SimpleRatings.
    * Hint: Create string 'report' when testing for multiple values in a list.
      This will provide a simple and complete overview of actual and expected data in case the test fails.
    * Hint: Separate standard unit tests from integration tests with TestCategoryAttribute,
      or by placing them in a separate project.
      This will allow developer to execute fast standard tests more often and slow
      integration test only when need (nightly build, e.g.).
  * Override application components with **fake** implementation
    ([stub or mock](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices))
    to simplify unit tests: BookTest.OverrideSystemComponentsForTesting.
  * Test **parallel requests** when there is a possibility of concurrency issues, such as deadlock,
    unique constraint validations and similar: BookTest.ParallelCodeGeneration.
    * This test also shows how to handle (rare) cases when we need to commit database transaction unit test.
  * Test **row permissions** with fake user: RowPermisionsTest.DocumentRowPermissions.

## Implementing business logic in a separate class or library

Documentation:

* Chapters "How to use another class from an Action" in article
  [Action concept](https://github.com/Rhetos/Rhetos/wiki/Action-concept#how-to-use-another-class-from-an-action-without-dependency-injection).
  The same principles apply to any other concept with a C# code snippet,
  for example ComposableFilterBy or RowPermissions.

Contents:

* Instead of implementing large blocks of C# code in DSL script (in Action or ComposableFilterBy, e.g.),
  you can call some method implemented in a custom class.
* The custom class can be implemented directly in Rhetos application (Rhetos v4 and later). The main benefit for
  writing custom code in a separate class instead of in DSL script is that C# IntelliSense is available.
  IntelliSense even includes the generated code, such as repository classes and Entity Framework model.
  For example see:
  * [ComputeBookRating](https://github.com/Rhetos/Bookstore/blob/master/src/Bookstore.Service/DslScripts/BookRating.rhe)
    in DSL script,
  * class [RatingSystem](https://github.com/Rhetos/Bookstore/blob/master/src/Bookstore.Service/Rating/RatingSystem.cs)
    that implements the feature,
  * and unit test
    [RatingSystemTest](https://github.com/Rhetos/Bookstore/blob/master/test/Bookstore.Service.Test/RatingSystemTest.cs)
    that tests the feature directly without using a database.
* Dependency injection
  * When a feature is implemented in a static method, it can be called directly from a DSL script (see examples above).
    When the feature implementation requires components from the application's dependency injection container,
    such as configuration, database connection and similar, then the custom class that implements
    the feature should also be registered to the DI container.
  * Read [How to use another class from an Action, with dependency injection](https://github.com/Rhetos/Rhetos/wiki/Action-concept#how-to-use-another-class-from-an-action-with-dependency-injection)
    and review linked examples.
  * Read [RepositoryUses](https://github.com/Rhetos/Rhetos/wiki/Low-level-object-model-concepts#repositoryuses)
* Alternatively, the custom class can be implemented in an external library, with one of the following
  design options to prevent circular dependency between the Rhetos application's code and the external library:
  * A) External library does not reference the generated code from the Rhetos application (e.g. the repository classes).
    This is a common solution for generic algorithms implementation.
    C# code in DSL scripts and Rhetos application can directly reference the generated library (old Rhetos apps
    with DeployPackages need to use ExternalReference concept).
    The external library may provided interfaces that can be implemented by entities and other
    data sources in DSL scripts, to simplify reading and writing data without directly referencing the generated code
    (concepts Implements, ImplementsQueryable and RegisteredImplementation, and GenericRepository helper class).
  * B) External library references the Rhetos application (to use repository classes, e.g.),
    but the Rhetos application cannot reference the library to avoid circular reference.
    This can be implemented by using reflection in Rhetos application to dynamically call the external library's methods.
    With Rhetos v4 and later it is simpler to implement the custom classes directly in the Rhetos application instead.

## Temporal data and change history

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/Temporal-data-and-change-history>

## Reporting

Documentation:

* TODO: Overview of reporting approaches on Rhetos apps (issue #315)
* Basic concepts: ReportData and ReportFile
  * Examples in unit tests [Computations.rhe](https://github.com/Rhetos/Rhetos/blob/master/test/CommonConcepts.TestApp/DslScripts/Computations.rhe)
* TODO: TemplaterReport package is not published on GitHub yet, it requires a paid license.

## Full-text search

Documentation:

* [SqlObject](https://github.com/Rhetos/Rhetos/wiki/SqlObject-concept),
  for creating full-text search catalog and index
  * Example in unit test
    [FullTextSearchTest.rhe](https://github.com/Rhetos/Rhetos/blob/master/test/CommonConcepts.TestApp/DslScripts/FullTextSearchTest.rhe)
  * FTS objects cannot be created in a database transaction.
* Rhetos contains EF LINQ extension methods for FullTextSearch
  * See [DatabaseExtensionFunctions.cs](https://github.com/Rhetos/Rhetos/blob/master/src/Rhetos.MsSqlEf6/CommonConcepts/DatabaseExtensionFunctions.cs)
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
