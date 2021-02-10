# Day 2

Topics:

1. [Simple business rules](#simple-business-rules)
2. [Read-only data structures](#read-only-data-structures)
3. [Simple read-only entities and code tables](#simple-read-only-entities-and-code-tables)
4. [Domain Object Model](#domain-object-model)

## Simple business rules

Rhetos CommonConcepts package contains some simple business rules that are
common in many applications.
Then can often be used by simply declaring them on an entity or a property.

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/Implementing-simple-business-rules>

Contents:

* Property value constraints (Required, MaxValue, RegExMatch, ...)
  * **Demonstrate** Required and MaxValue concept with POST requests.
* Deny data modifications (Lock, DenyUserEdit, ...)
* Automatically generated data (DefaultValue, AutoCode, ...)
  * **Demonstrate** AutoCode and CreationTime concept with POST requests.
* Logging
* Other features: Deactivatable, PessimisticLocking

Assignment:

1. Read the article <https://github.com/Rhetos/Rhetos/wiki/Implementing-simple-business-rules>
2. Use the following concepts from the article at least once in your bookstore application DSL scripts:
   Lock, DenyUserEdit, DefaultValue, AutoCode, CreationTime, ModificationTimeOf,
   Logging, AllProperties, Deactivatable.
3. Test the concepts Lock, AutoCode and Logging by inserting data by POST request,
   and check the results in the database.
   Instructions for sending the POST request are available at
   <https://github.com/Rhetos/Rhetos/wiki/Creating-new-WCF-Rhetos-application#test-and-review-the-application>.

## Read-only data structures

Basic DSL concepts that encapsulate loading or querying the data.

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/Read-only-data-structures>

Contents:

* Browse
  * **Demonstrate** the example from the wiki article.
* SqlQueryable
  * **Demonstrate** the example from the wiki article.
* External SQL scripts
  * **Demonstrate** the example from the wiki article.
* Using the computed data in Browse
  * **Demonstrate** the example from the wiki article.
* Computed
  * Similar to SqlQueryable, but implemented in C# instead of SQL.

Assignment:

1. Read the article <https://github.com/Rhetos/Rhetos/wiki/Read-only-data-structures>
2. The bookstore application should compute the total number of topics
   for each book (write an new SqlQueryable that Extends the Book entity).
   See the Entity BookTopic from article
   [Data model and relationships](https://github.com/Rhetos/Rhetos/wiki/Data-model-and-relationships).
3. The bookstore application should contain a grid that displays a list of books,
   with three columns: BookName, AuthorName and NumberOfTopics.
   Create a data source for the grid (write a new Browse that takes data from
   entities Book, Person and the previously created SqlQueryable).

## Simple read-only entities and code tables

How to develop a simple entity with hard-coded data.

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/Simple-read-only-entities-and-codetables>

Contents:

* Hardcoded concept
  * **Demonstrate** adding a Genre entity with two entries, review the generated records in database.
  * We will cover data-migration and after-deploy scripts later in workshop.
    They can be used for customized and more complex data initialization.
* Usage in the object model
* Usage in the database
* Usage in polymorphic implementation
  * **Skip** this example for now. Polymorphic concept will be covered later in the workshop.

## Domain Object Model

The **Domain Object Model** (DOM) is a set of C# classes that implement
the application's business logic and control the application's data.

When building the application, Rhetos generates the object model in the additional C# files,
based on the application's DSL model (from the *.rhe* scripts).
These additional files are automatically compiled as part of the application.

Application developers need to understand the generated classes and methods,
because they will be used in C# code snippets when implementing new business features,
such as filters, validations, computations and server actions.

Prerequisites:

* For this workshop, and development of Rhetos applications in general,
  it is important to understand basics of C# LINQ and how
  Entity Framework works with the LINQ queries,
  because many features such as filters and data processing will be
  implemented with LINQ queries.
  * Understanding simple methods on queries: Select, Where and ToList.
  * Understanding difference between applying Select(), Where() or Take() methods
    to IQueryable (applied to generated SQL)
    vs. using it on IEnumerable (executed on loaded data).

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/Using-the-Domain-Object-Model>
  * You can **skip** the chapters "Execute recompute (ComputedFrom)"
    and "Helpers for writing code snippets",
    we can come back to it later after we cover some other topics (Filters).
* <https://github.com/Rhetos/Rhetos/wiki/Action-concept>
  * You can **skip** the chapters "Using external code when developing actions"
    and "Example for the ExtAction concept".
    Using external classes is an advanced topic, we can return to it later.
    ExtAction is not included in CommonConcepts.

Contents:

* Understanding the generated object model
  * Review the generated C# application source code in `obj\Rhetos\RhetosSource`.
  * Simple class (POCO).
  * Queryable class (inherits simple, adds navigation properties for EF LINQ).
  * Repository class with methods that implement business rules and data access.
* How to execute the examples
  * Install and test LINQPad (requires paid license for IntelliSense).
  * Create a "playground" console app in Visual Studio.
* Reading the data (Load, Query, Filters, subqueries)
  * Test the examples from the article.
* Modifying the data, Action concept
  * You can **skip** the chapter "Execute recompute (ComputedFrom)" for now.
  * Test the examples from the article.
  * Note that the database transaction for each scope is rolled back by default.
    Call `scope.CommitChanges()` to commit the transaction.
* Analyze the DSL model.
* You can **skip** the chapter "Helpers for writing code snippets" for now.

Assignment:

1. Read the article <https://github.com/Rhetos/Rhetos/wiki/Using-the-Domain-Object-Model>,
   except the chapters "Execute recompute (ComputedFrom)" and "Helpers for writing code snippets".
2. Use a LINQPad script, or a playground application,
   for the bookstore application to solve the following tasks:
3. By using only Load() methods from the repositories,
   for each book print the book title and the name of its author.
   * Notes: You can use the `Load()` method without parameters to read all the books,
     and use the `Load(Guid[])` method with ID parameter to read the author for each book.
   * This approach is not efficient.
4. By using a Query() method, for all books print the book title number and the name of its author.
   * Notes: This will require only one Query() with a Select() method.
     Use the Dump() method to print all required data at once.
5. Use the ToString() method on the LINQ query from the previous task
   to print the generated SQL query.
6. By using the Action concept, write a server action for generating multiple books from one request,
   and execute the Action from the LINQPad or the playground app.
   * Parameters for the action are: NumberOfBooks (how many needs to be generated)
     and Title (the generated books can have the same title).
