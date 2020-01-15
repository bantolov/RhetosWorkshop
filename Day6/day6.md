# Day 6

Topics:

1. [Migrating the application data when changing the database structure](#migrating-the-application-data-when-changing-the-database-structure)
2. [Debugging](#debugging)
3. [System Logging (NLog)](#system-logging-nlog)
4. [Deploying Rhetos applications](#deploying-rhetos-applications)

## Migrating the application data when changing the database structure

When a developer makes changes in DSL script that affect the tables and columns
in the database (renaming an entity, for example),
there is often a need to migrate the data from the old data structure to the new one.

We will show how to develop and deploy data migration scripts,
to migrate the data when upgrading your Rhetos application.

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/Data-migration>
* <https://github.com/Rhetos/AfterDeploy/blob/master/Readme.md>

Contents:

* Developing data-migration scripts
  * SQL scripts - folder, purpose, ordering
  * Formatting
  * Rules for writing data-migration scripts
* Examples
  * **Demonstrate** example "Renaming a property".
  * Note that data-migration scripts are often used for initialization of the hardcoded data.
  * Other examples are available in the article
    [Data-migration](https://github.com/Rhetos/Rhetos/wiki/Data-migration).
* Advanced Topics
  * Deploying migration scripts
  * Automatic use of the migration tables when dropping and creating columns
    * Note that this feature is commonly used on downgrade and later upgrade,
      for example when switching between development branches.
  * Database structure independence
    * Note that this is a consequence of synergic work by two mechanisms:
      automatic backup/restore of column data and use/apply in data migration scripts.
  * Cleanup
    * You can **skip** this chapter for now, it is rarely used in practice.
  * Database constraint blocking the data migration
    * You can **skip** this chapter for now, it is rarely used in practice.
* AfterDeploy scripts as an alternative to DataMigration
  * AfterDeploy scripts are useful for data that is not an integral part of the business application
    (is not referenced by other entities) and should be refreshed on each deployment
    (data-migration scripts are usually executed only once).
  * AfterDeploy scripts can use database objects such as view and stored procedure,
    and other system data in the database (for example, automatically generated claims).
    This is why they are useful for predefined roles and permissions.
  * Hard-coded data (lookup tables, code tables) should be populated by DataMigration scripts,
    to allow using this data in other data-migrations scripts.

Assignment:

* Read the article <https://github.com/Rhetos/Rhetos/wiki/Data-migration>
* Read the article <https://github.com/Rhetos/AfterDeploy/blob/master/Readme.md>

## Debugging

When we need to debug the generated application, we can simply start Visual Studio
and debug the Rhetos application like any other application that has been written in C#.

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/Debugging>

Contents:

* Debugging
  * We can use Visual Studio debugger on the generated application as any other .NET web application.
* Example
  * **Demonstrate** the step-by-step debugging instructions.
* Tips and Tricks

Assignment:

* Read the chapter "Tips and tricks" in the article <https://github.com/Rhetos/Rhetos/wiki/Debugging#tips-and-tricks>

## System Logging (NLog)

The generated Rhetos application has system logging integrated in many internal components.
It provides global error reporting, performance diagnostics, and a detailed trace log for debugging.

Documentation:

* <https://github.com/Rhetos/Rhetos/wiki/Logging#system-log>,
  chapter "System log".

Contents:

* Configuring the integrated logging
  * Review the `nlog` section in web.config:
    * Targets: log files, Azure Application Insights, other NLog plugins.
    * Rules
      * Level: Error, Info (warnings), Trace (debugging)
      * Filters by name.
  * **Demonstrate** trace log:
    * Uncomment `<logger name="*" minLevel="Trace" writeTo="TraceLog" />` in web.config.
    * Execute any valid REST request and review RhetosServerTrace.log.
    * Note that you should enable TraceLog in production only for a short period of time,
      because of performance and storage overhead.
* Logging from application code
* Note that system log is a different mechanism from Logging concept and Common.Log table
  * Common.Log contains log of business events from the application usage
    (data changes and auditing), but cannot contain system errors (rollback on error).

Assignment:

* Read the article <https://github.com/Rhetos/Rhetos/wiki/Logging>

## Deploying Rhetos applications

Best practices for deployment process on development, test and production environments.
Additional features of DeployPackages utility.

Documentation:

* Bookstore build script example <https://github.com/Rhetos/Bookstore/blob/master/Build.ps1>
* Command-line switches (`DeployPackages.exe /?`)
* TODO: Tutorial

Contents:

* Deployment on **development environment**
  * Each developer should have a full development environment,
    as we have setup on the first day of this workshop.
    Don't use shared web application for development.
  * Review [Bookstore Build.ps1](https://github.com/Rhetos/Bookstore/blob/master/Build.ps1).
* Initial deployment on **test/production environment**
  * Copy the  web application (folder `dist\BookstoreRhetosServer`) to the server.
  * Create an empty database.
  * Setup IIS and .config files.
  * Run `DeployPackages /DatabaseOnly` to upgrade the database (this will skip the steps that
    download NuGet packages and generate the application binaries).
    * DeployPackages (currently) requires sources of all packages. This can complicate deployment
      if we were deploying some packages directly from source folder.
      The solution is to deploy all Rhetos packages as NuGet packages, instead of deploying
      directly from source folder (see RhetosPackages.config).
      See how is this solved in [this commit](https://github.com/Rhetos/Bookstore/commit/f8ae2d33d8928e022107405ce5da22d04704785d)
      for Bookstore demo application.
      Now when Rhetos application is copied to production, the PackagesCache subfolder will
      contain everything need for DeployPackages with DatabaseOnly.
* Upgrading **test/production environment** (deploying a new version)
  * Copy the web application (folder `dist\BookstoreRhetosServer`) over the existing one,
    while keeping the existing config files.
  * Run `DeployPackages /DatabaseOnly` to upgrade the database.
  * CI/CD
* Understanding DeployPackages.exe
  * Review the console output of DeployPackages.exe, for an overview of deployment steps.
  * Review all command-line switches (`DeployPackages.exe /?`)
* Troubleshooting deployment
  * Error reporting: Errors in DSL, C# and SQL code.
    DeployPackages.log for more details.
    Uncomment TraceLog in `bin\DeployPackages.exe.config` for more details.
  * Performance issues with KeepSynchronized on large cache tables
    * See [Automatic recompute on deployment](https://github.com/Rhetos/Rhetos/wiki/Persisting-the-computed-data#automatic-recompute-on-deployment).
      and "Suppressing recompute on deployment".
    * Usually we will manually update the cached data with a custom-made SQL script
      if DeployPackages reported "Skipped recomputing {Target} from {Source}".
  * Large database transactions (/ShortTransactions)
  * Incorrect drop-script for an SqlObject.
    See [Fixing an incorrect removal script](https://github.com/Rhetos/Rhetos/wiki/SqlObject-concept#troubleshooting-fixing-an-incorrect-removal-script).

Assignment:

* Publish your BookstoreRhetosServer application on a test environment,
  on a new empty database.
  Make sure to run DeployPackages with `/DatabaseOnly` switch,
  to avoid rebuilding the whole application.
