# Day 5

Topics:

1. [Entity inheritance](#entity-inheritance)
2. [Event sourcing](#event-sourcing)
3. [Business process modeling](#business-process-modeling)
4. [Computations, persisting computed data](#computations-persisting-computed-data)

## Entity inheritance

Design patterns and best practices for modeling class inheritance in database.
Polymorphic concept in Rhetos DSL can simplify implementation of some data structures,
such as entity inheritance or events in business process model.

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/Polymorphic-concept>
* Example with comments: <https://github.com/Rhetos/Bookstore/blob/master/src/Bookstore.Service/DslScripts/SalesItem.rhe>

Contents:

* Three standard models of implementing entity inheritance in database
  * <https://www.entityframeworktutorial.net/code-first/inheritance-strategy-in-code-first.aspx>
  * Review three `CREATE TABLE` examples from: [Inheritance strategy.md](Inheritance%20strategy.md)
    * Review trade-offs between redundancy in data structure vs. data consistency.
    * Maintainability is a priority in Enterprise Application Development.
* Polymorphic concept
  * Basic usage
    * **Demonstrate** Polymorphic to implement previous example for "Table per Concrete Class".
    * **Demonstrate** custom property implementation for Book.Description, instead of adding a new column to the Book entity. It should return Title + ' ' + Author's name.
      Use *Implements* concept, see the [SalesItem example](https://github.com/Rhetos/Bookstore/blob/master/src/Bookstore.Service/DslScripts/SalesItem.rhe).
      * Hint: To develop the SQL code snippet for the Implements parameter, see where this code will end up in database:
      See the generated view Book_As_SalesItem, copy the select query and modify the Description part until the query works as expected, then copy the Description part to the DSL script.
  * **Review basic principles** of polymorphic concept
    * Polymorphic is similar to an interface in C#:
      Any data structures can implement a given polymorphic by providing a *mapping*
      between its records and the polymorphic structure.
    * Polymorphic is similar to a base class for inheritance in C#:
      it represents a base object as a union of all specific implementations.
    * Polymorphic is more flexible then inheritance:
      * Allows independent feature implementation for each subtype (e.g., AutoCode format, Description).
        Mixing multiple type in same entity/table results with many "if" conditions in business
        rules (e.g., property is requires except for certain subtypes).
        Many simple features are easier to maintain than few complex ones.
      * Entity can implement multiple polymorphic types (or the same one multiple times).
      * Entity can provide complex custom mapping, it does not need to have same properties.
  * Multiple interface implementations
  * Property implementation with subquery
  * Limit the implementation with filter (where)
    * **Demonstrate** `Where` (price is not null).
  * Referencing or extending a polymorphic data structure
    * Review Browse `SalesItemGrid`.
    * Review detail entity `SalesItemComment`.
    * Note that materialization can occur manually or automatically,
      for performance and consistency.
    * Materialized entity can be extended with additional properties.
  * Subtype implementation using SQL query
  * Efficient queries from client application
    * **Demonstrate** SQL Server execution plan when filtering the polymorphic view
      by subtype. Note that only one implementation table will be considered.

Assignment:

* Read the article <https://github.com/Rhetos/Rhetos/wiki/Polymorphic-concept>

## Event sourcing

Event sourcing is one of most important design patterns in business applications,
often applied to business process modeling.

It is not directly related to Rhetos, but is commonly used here
because it correlates well with principles and goals of Rhetos platform.

Documentation:

* [Greg Young - CQRS and Event Sourcing - Code on the Beach 2014](https://www.youtube.com/watch?v=JHGkaShoyNs)
* <https://martinfowler.com/eaaDev/EventSourcing.html>

Contents:

* Event Sourcing ensures that all changes to application state are stored as a sequence of events.
  Not just can we query these events, we can also use the event log to reconstruct past states,
  and as a foundation to automatically adjust the state to cope with retroactive changes.
  [Fowler](https://martinfowler.com/eaaDev/EventSourcing.html)
* Not allowing update or delete commands (not so strictly).
  * Example: Approving a request should be insert, not and update.
    Current request state should be computed by the system.
  * Client does not want to lose data with update and delete.
* "Event sourcing" - Event is the source.
* Often coupled with CQRS
* Advantages
  * Complete rebuild from scratch
  * Temporal query
  * Event replay on retroactive correction
  * Feature decoupling makes it easier to change or extend (customize) business process.
    For example, adding a `Where`, or changing a new status for an event will
    automatically update effected current states.

Assignment:

1. Watch [Greg Young - CQRS and Event Sourcing - Code on the Beach 2014](https://www.youtube.com/watch?v=JHGkaShoyNs)

## Business process modeling

Best practices for implementing business processes in Rhetos apps.

Documentation:

* TODO: Business process modeling tutorial (issue #311) on implementing business processes, with 2 typical examples:
  updating latest status and accumulating amounts.
* An example of a business process with statuses:
  See Shipment.rhe, ShipmentEvents.rhe and ShipmentState.rhe
  in <https://github.com/Rhetos/Bookstore/tree/master/src/Bookstore.Service/DslScripts>.
* TODO: Implement an example of a business process with aggregated quantities in Bookstore (issue #289)
  * WarehouseState process in module Warehousing1 - same implementation as Shipment process
  * WarehouseState process in module Warehousing2 - optimized implementation in C#
    instead of database view with cached state for each event (not just the current state).

Contents:

* Example diagram for shipment management: Approved => DeliveryInProgress => Delivered.
  * Events and statuses are not always related 1:1, multiplicity can go in both ways.
  * Process state is often not a single value.
* Event is short for "record of a business event" in the application.
  * New entity for each event type.
  * **Demonstrate** events from Bookstore shipment management. Use ShortString as a status for now.
* Process state is a broader computed information that includes the process status
  (our naming convention).
  * SqlQueryable. We will often cache the result (next workshop topic).
  * **Demonstrate** computed shipment state.
* Use Polymorphic as a common event interface for computing state.
  * **Demonstrate** the change in SqlQueryable.
  * Less coupled features (no need to change SqlQueryable when adding new events).
  * Easier customization.
  * Event effects are encapsulated in the event entity.
* Base process entity can participate as a creation event
  * **Demonstrate** creation event on shipment with status "Preparing".
* Two most common process types
  * Process state is a status from the *latest event* (the shipment example).
  * Process state is an aggregation of *amounts* (amount of goods in warehouse management,
    or financial account balance in bookkeeping process).
  * Implementation is basically the same, based on event sourcing, polymorphic event entities,
    and computed process state.
  * Aggregation of amounts is often additionally optimized to avoid scanning all records in one
    group when updating state (ChangesOnChangedItems with custom FilterBy).
  * Review an example of process that aggregate amounts:
    * [Skladiste.txt](Skladiste.txt). TODO: Implement WarehouseState process in Bookstore demo application (issue #289)

Assignment:

1. Implement a business process of your choice (similar to the Shipment example) in the Bookstore application.

## Computations, persisting computed data

Managing the computed data in your application.
Switching between on-the-fly computations and automated caching.

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/Persisting-the-computed-data>
  * TODO: Detailed explanation of ComputedFrom, KeyProperties and ChangesOn concepts (issue #312),
    with examples from Bookstore and how to manually execute Recompute method on generated object model.
* Examples are available in <https://github.com/Rhetos/Bookstore/blob/master/src/Bookstore.Service/DslScripts>
  * ShipmentState.rhe
  * BookInfo.rhe (a similar example)
  * BookRating.rhe (data source is not a view, but an external C# algorithm).
* You can also find a short explanation of how the ChangesOnChangedItems works in LINQPad script
  <https://github.com/Rhetos/Bookstore/blob/master/src/Bookstore.Service/LinqPad/Shipment%20state.linq>

Contents:

* Understanding the computed data
  * Clean data model: user-entered data and system-computed data.
  * Computed on-the-fly vs. persisted (cache entity).
  * How to compute data: Filters, SqlQueryable, Computed, ...
* ComputedFrom concept
  * Source (ComputedFrom), mapping (AllProperties), updating (KeepSynchronized).
  * **Demonstrate** ComputedFrom on SqlQueryable BookInfo with NumberOfComments,
    from <https://github.com/Rhetos/Rhetos/wiki/Read-only-data-structures#sqlqueryable-concept>.
    * Rename SqlQueryable to ComputeBookInfo.
    * **Demonstrate** inserting comment in LINQPad without ChangesOn* concept.
* Automatically updating cached data
  * ChangesOnChangedItems
  * Helpers:
    * ChangesOnReferenced - the result depends on a referenced entity
    * ChangesOnLinkedItems - the result depends on a detail entity
    * ChangesOnBaseItem
  * KeyProperties - By default, the computed data and cached data is matched by ID.
* Automatic recompute on deployment
  * By default, cached data will be automatically updated on deployment if Rhetos detects
    modification in the direct source object.
  * Forcing recompute on deployment (the automatic detection does not track indirect modifications)
  * Suppressing recompute on deployment (custom manual cache update,
    for deployment performance on very large cache tables)
* Maintainability
  * You can create a report that will recompute all data with ComputedFrom KeepSynchronized,
    compare it to the existing cache tables,
    and send the result to administrators or developers.
    * For example, we can detect manual corrections of data in the database (client support),
      where developer forgot to update the dependent data.

Assignment:

* Read the article <https://github.com/Rhetos/Rhetos/wiki/Persisting-the-computed-data>
