# Day 6

Topics:

1. [Migrating the application data when changing the database structure](#migrating-the-application-data-when-changing-the-database-structure)
2. [Debugging](#debugging)
3. [System Logging (NLog)](#system-logging-nlog)
4. [Deploying Rhetos apps](#deploying-rhetos-apps)

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
  * **Demonstrate** the step-by-step debugging instructions in
    [Debugging applications built with Rhetos CLI and Rhetos MSBuild integration](https://github.com/Rhetos/Rhetos/wiki/Debugging#debugging-applications-built-with-rhetos-cli-and-rhetos-msbuild-integration)
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
  * **Demonstrate** logging with NLog:
    Follow the instructions in section 
    [Use NLog to write application's system log into a file](https://github.com/Rhetos/Rhetos/wiki/Recommended-application-setup#use-nlog-to-write-applications-system-log-into-a-file).
    Review the nlog configuration file:
    * Targets: log files, Azure Application Insights, other NLog plugins.
    * Rules:
      * Level: Error, Info (warnings), Trace (debugging)
      * Filters by name.
  * **Demonstrate** trace log:
    * Uncomment `<logger name="*" minLevel="Trace" writeTo="TraceLog" />` in nlog configuration.
    * Execute any valid REST request and review RhetosServerTrace.log.
    * Note that you should enable TraceLog in production only for a short period of time,
      because of performance and storage overhead.
* Logging from application code
* Note that system log is a different mechanism from Logging concept and Common.Log table
  * Common.Log contains log of business events from the application usage
    (data changes and auditing), but cannot contain system errors (rollback on error).

Assignment:

* Read the article <https://github.com/Rhetos/Rhetos/wiki/Logging>

## Deploying Rhetos apps

Best practices for deployment process on development, test and production environments.
Additional features of Rhetos CLI utility.

Documentation:

* [Publishing the application to a test environment or production](https://github.com/Rhetos/Rhetos/wiki/Recommended-application-setup#publishing-the-application-to-a-test-environment-or-production)
* [Rhetos CLI](https://github.com/Rhetos/Rhetos/wiki/Rhetos-CLI)
* [Bookstore build script example](https://github.com/Rhetos/Bookstore/blob/master/Build.ps1)
* TODO: Tutorial on "Deploying Rhetos apps" (issue #313), based on contents below.

Contents:

* Deployment on **development environment**
  * Each developer should have a full development environment,
    as we have setup on the first day of this workshop.
    Don't use shared web application for development.
  * Review [Bookstore Build.ps1](https://github.com/Rhetos/Bookstore/blob/master/Build.ps1).
* Initial deployment on **test/production environment**
  * Read the instructions in section
    [Publishing the application to a test environment or production](https://github.com/Rhetos/Rhetos/wiki/Recommended-application-setup#publishing-the-application-to-a-test-environment-or-production).
* Upgrading **test/production environment** (deploying a new version)
  * Copy the applications and run `rhetos dbupdate`.
  * Separate base configuration files (overwrite on each upgrade) from the environment-specific configuration.
    See [Configuration management](https://github.com/Rhetos/Rhetos/wiki/Configuration-management) for details.
  * CI/CD.
* [Rhetos CLI](https://github.com/Rhetos/Rhetos/wiki/Rhetos-CLI) commands and options.
* Troubleshooting deployment
  * Error analysis:
    * Deployment errors and warnings are logged at `Logs\RhetosCli.log`.
    * Uncomment `<logger...TraceLog...>` in `bin\rhetos.exe.config` for more detailed logging.
  * Performance issues with KeepSynchronized on large cache tables
    * See [Automatic recompute on deployment](https://github.com/Rhetos/Rhetos/wiki/Persisting-the-computed-data#automatic-recompute-on-deployment).
      and "Suppressing recompute on deployment".
    * We can manually update the cached data with a custom-made SQL script
      if `rhetos dbupdate` reported warning "Skipped recomputing {Target} from {Source}".
  * Large database transactions (/ShortTransactions)
  * Incorrect drop-script for an SqlObject.
    See [Fixing an incorrect removal script](https://github.com/Rhetos/Rhetos/wiki/SqlObject-concept#troubleshooting-fixing-an-incorrect-removal-script).
* Older Rhetos apps use DeployPackages instead of Rhetos CLI:
  * DeployPackages.exe is similar to running:
    * nuget.exe restore
    * rhetos.exe build
    * csc.exe (C# compiler)
    * rhetos.exe dbupdate.
  * `DeployPackages.exe /DatabaseOnly` is equivalent to `rhetos.exe dbupdate`.
  * Logging is similar: Configure `bin\DeployPackages.exe.config`,
    log is written to `Logs\DeployPackages.log`.

Assignment:

* Publish your Bookstore.Service application on a test environment,
  on a new empty database.

Assignment notes and instructions:

1. The test and production environments are normally placed on a shared server,
   but in this assignment, create a new application instance on your development machine:
   Create a new folder *Bookstore2* for your new application instance.
2. You could simply copy the files from bin folder into *Bookstore2*.
   A better approach is to use *publish* feature instead:
   In Visual Studio right-click the Bookstore.Service project and select "Publish...",
   select target "Folder" then click "Publish".
   The application files are generated in folder `Bookstore.Service\bin\Release\net6.0\publish`.
   You can copy the files from that folder into *Bookstore2*.
3. Create a new empty database Bookstore2.
4. Edit the database connection string in Bookstore2 to reference the new database.
   It is probably located in appsetting.json, or similar.
5. Updated the Bookstore2 database:
   Open command prompt in Bookstore2, and execute `rhetos.exe dbupdate Bookstore.Service.dll`.
6. Test the application by running Bookstore.Service.exe in Bookstore2 folder.
   * Use the following command-line argument to simulate the development environment with Swagger enabled:
     `Bookstore.Service.exe --environment Development`
