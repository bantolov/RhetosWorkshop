# Day 4

Topics:

1. [Low-level object model concepts](#low-level-object-model-concepts)
2. [Extending DSL - Develop a code generator](#extending-dsl---develop-a-code-generator)
3. [Extending DSL - Develop a macro concept](#extending-dsl---develop-a-macro-concept)
4. [User authentication](#user-authentication)
5. [Authorization - Basic permissions](#authorization---basic-permissions)
6. [Authorization - Row permissions](#authorization---row-permissions)
7. [Follow-up assignment review](#follow-up-assignment-review)

## Low-level object model concepts

Overview of low-level DSL concepts for implementation of specific business rules,
when none of the existing concepts are applicable and there is no incentive to
write a new reusable concept.

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/Low-level-object-model-concepts>

Contents:

* SaveMethod and the related sub-concepts
  * Review the Save method for Book in the generated application source.
    Note the method arguments (insertedNew, updatedNew).
    Note the tags in the Save method for inserting the new code (ArgumentValidation, Initialization).
  * **Demonstrate** SaveMethod and Initialization concepts by updating a DateTime property to
    last modification time.
* RepositoryUses
* RepositoryMember

Assignment:

1. Read the article <https://github.com/Rhetos/Rhetos/wiki/Low-level-object-model-concepts>

## Extending DSL - Develop a code generator

All features available in Rhetos DSL are implemented as plugins with code generators.

While developing a new application, developers will often extends the DSL programming language
with new custom keywords and new code generators, to simplify implementation of patterns
in user requirements or integration with new technologies.

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/Rhetos-concept-development>
* [Bookstore](https://github.com/Rhetos/Bookstore) demo application contains
  an example of code generator concept *DeactivateOnDelete*,
  in project [src\Bookstore.Concepts](https://github.com/Rhetos/Bookstore/tree/master/src/Bookstore.Concepts).
* <https://github.com/Rhetos/Rhetos/wiki/Creating-Rhetos-package>.

Contents:

* In Rhetos framework source code, review the existing CommonConcepts concept definition classes:
  ModuleInfo, EntityInfo (DataStructureInfo) and CreationTimeInfo.
* Creating a new class library project (a new DLL) for custom concepts as a part of your application.
  * **Demonstrate** adding a new Visual Studio project for DLL with custom concepts.
    Add the project with custom DSL concepts (e.g. Bookstore.Concepts) in the same solution
    with the existing Rhetos application (Bookstore.Service).
    The the Rhetos application add the project reference to the Bookstore.Concepts project,
    in order to use the custom concepts during build.
* Rhetos concept development
  * **Demonstrate** development of new concept "LastModifiedTime" with code generator on the
    example from the previous workshop topic "Low-level object model concepts".
    The new concept is similar to the existing concept CreationTime,
    see [definition](https://github.com/Rhetos/Rhetos/blob/master/CommonConcepts/Plugins/Rhetos.Dsl.DefaultConcepts/SimpleBusinessLogic/CreationTimeInfo.cs)
    and [code generator](https://github.com/Rhetos/Rhetos/blob/master/CommonConcepts/Plugins/Rhetos.Dom.DefaultConcepts/SimpleBusinessLogic/CreationTimeCodeGenerator.cs).
* There are other code generators available (Database, REST web API, MVC client model, ...),
  and new custom generated files could be added (for example, a generated TypeScript classes for all entities).
* Testing & debugging custom DSL concepts.
* New concepts library can be released as a NuGet packages to be reused in other Rhetos applications.
  The NuGet package can also contain DSL scripts, data-migration scripts, and other files.
  See <https://github.com/Rhetos/Rhetos/wiki/Creating-Rhetos-package>.

TODO: Overview of additional code generator plugin types (issue #310)

Assignment:

1. Read the article <https://github.com/Rhetos/Rhetos/wiki/Rhetos-concept-development>
2. Read the article <https://github.com/Rhetos/Rhetos/wiki/Creating-Rhetos-package>

## Extending DSL - Develop a macro concept

Macro concepts are simplest and most commonly developed custom concepts in application development.
Instead of generating additional application code, they simply create new concepts.

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/Rhetos-concept-development>
* [Bookstore](https://github.com/Rhetos/Bookstore) demo application contains
  an example of macro concept *PhoneNumber*,
  in [src\Bookstore.Concepts](https://github.com/Rhetos/Bookstore/tree/master/src/Bookstore.Concepts).

Contents:

* Macro concepts
  * **Demonstrate** development of new macro concept "CodeTable" that can be applied to an entity
    and generates: `ShortString Code { AutoCode; }` and `ShortString Name { Required; }`.
* Macro concept that inherits EntityInfo
  * **Demonstrate** on CodeTable.
  * Note that "inheritance" approach is not a best practice for this example,
    because it prevent you from applying multiple such feature the same entity.
* Macro concept can use *IDslModel*, for example to add some feature to each entity
  in a given module.

Assignment:

1. Implement a new macro concept "MonitoredRecord” as a part of your Bookstore application.
   The concept will be used on an entity, and it should automatically add the following features
   to that entity (also see the DSL usage example below):
   1. logging of all data changes on all properties
   2. a new property "CreatedAt" with each record's time of creation
2. The concept definition classes for the created concepts are:
   DateTimePropertyInfo, CreationTimeInfo, EntityLoggingInfo,
   AllPropertiesLoggingInfo, DenyUserEditPropertyInfo.
3. Usage example:
    ```c
    Module Bookstore
    {
        Entity Book
        {
            MonitoredRecord;

            // The "MonitoredRecord”" concept will generate the following features:
            // DateTime CreatedAt { CreationTime; DenyUserEdit; }
            // Logging { AllProperties; }
        }
    }
    ```

## User authentication

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/User-authentication-and-authorization>

Assignment:

1. Read the article <https://github.com/Rhetos/Rhetos/wiki/User-authentication-and-authorization>.

## Authorization - Basic permissions

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/Basic-permissions>

Assignment:

1. Read the article <https://github.com/Rhetos/Rhetos/wiki/Basic-permissions>.
2. In order to test the basic permissions, *disable*
   [suppressed permissions](https://github.com/Rhetos/Rhetos/wiki/Basic-permissions#suppressing-permissions-in-a-development-environment)
   in development environment:
   * In your application's *Web.config* file make sure that "Rhetos:AppSecurity:AllClaimsForUsers"
     is set to empty string "", and "BuiltinAdminOverride" to "False".
3. Open a web browser, and read the data from any entity in your application through the
   REST web API (for example <http://localhost/Bookstore.Service/rest/Bookstore/Book/>)
   * The request should fail and display an error "Your account '***' is not registered in the system...".
     This is happening because we have disabled the override, and the basic permissions are now active.
4. Open the Rhetos application homepage (for example <http://localhost/Bookstore.Service/>),
   and find your **account name** under the server status "User identity: ***".
5. Add a new user directly in the database table (`Common.Principle`),
   set the Name to your account name
   (see [Common administration activities](https://github.com/Rhetos/Rhetos/wiki/Basic-permissions#common-administration-activities)).
   Make sure to include the domain name prefix, if it's showing in the previous step after "User identity".
6. Test the REST request again, the message should now be different: "You are not authorized for action...".
   * Find the claim that is required for this operation in the table *Common.Claim*.
7. Add a new role in the database (`Common.Role`). Add the role permission
   (`Common.RolePermission`) to connect this role with the claim that is required.
8. Assign this role to the previously created principal (`Common.PrincipalHasRole`).
9. Test the REST request again. It should returns the entity's data.

## Authorization - Row permissions

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/RowPermissions-concept>

Assignment:

1. Read the article <https://github.com/Rhetos/Rhetos/wiki/RowPermissions-concept>.
2. Implement the following row permissions in you Bookstore application with the following requirements:
   * Each book has an employee assigned to it.
   * Every employee can read all book records, but only the employee assigned to the book
     can edit that record in Book entity.
   * Only the employee assigned to the book can edit the book's comments (see entity Comment).

Assignment notes and common issues:

* When testing row permissions, you can return the Web.config to the old configuration:
  `<add key="BuiltinAdminOverride" value="True" />`, if you have changed it previously
  while testing Basic permissions.
* Basic permissions (claims) and row permissions are working at the same time.
  For example, for a user to read an entity, he or she should have both:
    1. basic permission assigned (Read claim)
    2. and a row permission rule that allows the operation (if the row permissions are used on this entity).
* If you are not using row permissions on an entity, the permissions will not be checked,
  but when you add the RowPermissions keyword to an entity, then all read and write
  operations in this entity are *denied by default*, until explicitly allowed
  by Allow, AllowRead or AllowWrite.
* You can connect the user account to the employee by simply
  adding `ShortString UserName;` property to the `Entity Employee`.
  To test the application, enter your full account name in this field
  directly in the database.

## Follow-up assignment review

```c
Entity Employee
{
    ShortString UserName;
}

Entity Book
{
    Reference AssignedTo Bookstore.Employee; // Only the assigned person should be allowed to edit the book data.

    RowPermissions
    {

        AllowRead EveryoneCanRead 'context =>
            {
                return book => book.AssignedTo.ID != null;
            }';

        // Better:
        AllowRead EveryoneCanRead2 'context =>
            {
                return book => true;
            }';

        // Shorter:
        AllowRead EveryoneCanRead3 'context => book => true';

        //====================

        Allow OwnerCanWrite 'context =>
            {
                Guid employeeId = context.Repository
                    .Bookstore.Employee.Query()
                    .Where(e => e.UserName == context.UserInfo.UserName)
                    .Select( e => e.ID)
                    .SingleOrDefault();
                return book => book.AssignedTo.ID == employeeId;
            }';

        // Better:
        AllowWrite OwnerCanWrite2 'context =>
            {
                return book => book.AssignedTo.UserName == context.UserInfo.UserName;
            }';
    }
}

Entity Comment
{
    LongString Text;
    Reference Book { Detail; }

    RowPermissions
    {
        AllowRead EveryoneCanRead 'context =>
            {
                return comment => comment.Book.AssignedTo.ID != null;
            }';

        Allow OwnerCanWrite 'context =>
            {
                Guid employeeId = context.Repository
                    .Bookstore.Employee.Query()
                    .Where(e => e.UserName == context.UserInfo.UserName)
                    .Select(e => e.ID)
                    .SingleOrDefault();

                return comment => comment.Book.AssignedTo.ID == employeeId;
            }';

        // Better:
        InheritFrom Bookstore.Comment.Book;
    }
}

// Or:
AutoInheritRowPermissions;
```
