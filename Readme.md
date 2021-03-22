# Overview

Notes for self-education:

* The workshop is divided into days, each day containing several topics.
* For each topic, follow the instruction under chapters "Contents" and "Assignment".
* The "Contents" section instructs what to read and try in practice.
  It usually contains bullets with instructions what to do for each section of the referenced documentation.
  Look for the following marks:
  * **Read** - Read the referenced documentation section, no need to implement it.
  * **Demonstrate** - Implement the referenced example in your development environment, and test it.
  * **Skip** - The referenced documentation section is not part of the current workshop topic.
* The "Assignment" section contains a homework assignment.
  Implement it in you development environment.
* The "Documentation" section is just a list of references.
* Some topics currently have incomplete documentation (see TODO tags in other files).

## [Day 1](Day1/day1.md)

*(3 hours lecture)*

1. Prerequisites for day 1 *(assignment)*
2. What is Rhetos *(theory)*
3. What is available on GitHub *(theory)*
4. Development environment setup and the first application
5. Rhetos DSL syntax
   * Declarative programming *(theory)*
6. Data model and relationships
7. From LINQ to SQL *(assignment)*

Comprehension review:

* What is Rhetos / Developing generated application without Rhetos
* Build in Visual Studio: rhetos build, csc, rhetos dbupdate
* When to use the Extends keyword and 1:1 relationships

## [Day 2](Day2/day2.md)

*(3 hours lecture)*

1. Simple business rules
2. Read-only data structures
3. Simple read-only entities and code tables *(new)*
4. Domain Object Model
   * Understanding the generated object model *(theory)*

## [Day 3](Day3/day3.md)

*(2 hours lecture, Authentication and Authorization could be moved from day 4 to day 3 to reduce day 4)*

1. Filters
2. Validations
3. REST Web API
4. Low-level database concepts

Comprehension review:

* Filters / When to use FilterBy, ItemFilter or ComposableFilterBy
* Filters / When not to write filters
* Data validations / Understanding data validations in Rhetos
* Low-level database concepts / Rule of thumb
* Low-level database concepts / AutoDetectSqlDependencies
* Low-level database concepts / What is a difference between SqlQueryable and SqlView?

## [Day 4](Day4/day4.md)

*(3 hours lecture)*

1. Low-level object model concepts
   * When to use them *(theory)*
2. Extending DSL - Develop a code generator *(incomplete documentation)*
3. Extending DSL - Develop a macro concept *(incomplete documentation)*
4. User authentication *(theory)*
5. Authorization - Basic permissions *(theory)*
6. Authorization - Row permissions *(theory)*

## [Day 5](Day5/day5.md)

*(3 hours lecture)*

1. Entity inheritance *(theory)*
2. Event sourcing *(theory)*
3. Business process modeling *(theory, incomplete documentation)*
4. Computations, persisting computed data *(theory, incomplete documentation)*

## [Day 6](Day6/day6.md)

*(3 hours lecture)*

1. Migrating the application data when changing the database structure
   * Database structure independence *(theory)*
2. Debugging
3. System Logging - NLog
4. Deploying Rhetos applications

## [Day 7](Day7/day7.md)

*(2 hours lecture)*

1. Unit testing
2. Implementing business logic in a separate library *(theory)*
3. Temporal data and change history *(theory)*
4. Reporting - TemplaterReport *(theory)*
5. Full-text search *(theory)*
6. Rhetos on GitHub *(theory)*
