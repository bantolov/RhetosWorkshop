<Query Kind="Program">
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\EntityFramework.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\EntityFramework.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\EntityFramework.SqlServer.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\EntityFramework.SqlServer.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\NLog.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\NLog.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\Oracle.ManagedDataAccess.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\Oracle.ManagedDataAccess.dll</Reference>
  <Reference>C:\My Projects\Rhetos\Source\Rhetos\bin\Plugins\Rhetos.AspNetFormsAuth.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Configuration.Autofac.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Configuration.Autofac.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\Plugins\Rhetos.Dom.DefaultConcepts.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\Plugins\Rhetos.Dom.DefaultConcepts.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\Plugins\Rhetos.Dom.DefaultConcepts.Interfaces.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\Plugins\Rhetos.Dom.DefaultConcepts.Interfaces.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Dom.Interfaces.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Dom.Interfaces.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\Plugins\Rhetos.Dsl.DefaultConcepts.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\Plugins\Rhetos.Dsl.DefaultConcepts.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Dsl.Interfaces.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Dsl.Interfaces.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Interfaces.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Interfaces.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Logging.Interfaces.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Logging.Interfaces.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Persistence.Interfaces.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Persistence.Interfaces.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\Plugins\Rhetos.Processing.DefaultCommands.Interfaces.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\Plugins\Rhetos.Processing.DefaultCommands.Interfaces.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Processing.Interfaces.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Processing.Interfaces.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Security.Interfaces.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Security.Interfaces.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Utilities.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\Rhetos.Utilities.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\Generated\ServerDom.Model.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\Generated\ServerDom.Model.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\Generated\ServerDom.Orm.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\Generated\ServerDom.Orm.dll</Reference>
  <Reference Relative="..\..\..\Bookstore\dist\BookstoreRhetosServer\bin\Generated\ServerDom.Repositories.dll">C:\Bojan\Bookstore\dist\BookstoreRhetosServer\bin\Generated\ServerDom.Repositories.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.AccountManagement.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.DirectoryServices.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Runtime.Serialization.dll</Reference>
  <Namespace>Oracle.ManagedDataAccess.Client</Namespace>
  <Namespace>Rhetos.Configuration.Autofac</Namespace>
  <Namespace>Rhetos.Dom</Namespace>
  <Namespace>Rhetos.Dom.DefaultConcepts</Namespace>
  <Namespace>Rhetos.Dsl</Namespace>
  <Namespace>Rhetos.Dsl.DefaultConcepts</Namespace>
  <Namespace>Rhetos.Logging</Namespace>
  <Namespace>Rhetos.Persistence</Namespace>
  <Namespace>Rhetos.Security</Namespace>
  <Namespace>Rhetos.Utilities</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.Data.Entity</Namespace>
  <Namespace>System.DirectoryServices</Namespace>
  <Namespace>System.DirectoryServices.AccountManagement</Namespace>
  <Namespace>System.IO</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Reflection</Namespace>
  <Namespace>System.Runtime.Serialization.Json</Namespace>
  <Namespace>System.Text</Namespace>
  <Namespace>System.Xml</Namespace>
  <Namespace>System.Xml.Serialization</Namespace>
</Query>

void Main()
{
    ConsoleLogger.MinLevel = EventType.Info; // Use "Trace" for more details log.
    var rhetosServerPath = Path.GetDirectoryName(Util.CurrentQueryPath);
    Directory.SetCurrentDirectory(rhetosServerPath);
    using (var container = new RhetosTestContainer(commitChanges: false)) // Use this parameter to COMMIT or ROLLBACK the data changes.
    {
        var context = container.Resolve<Common.ExecutionContext>();
        var repository = context.Repository;

        repository.Bookstore.Insert5Books.Execute(null);
        repository.Bookstore.Book.Load().Dump();
        repository.Bookstore.Book.Load(k => k.Title.StartsWith("b")).Dump();
        var filterParameter = new Bookstore.CommonMisspelling();
        repository.Bookstore.Book.Load(filterParameter).Dump();
        Guid id = new Guid("c9860617-7e3e-4158-9d57-bbf4dd1f986e");
        var book1 = repository.Bookstore.Book.Load(new[] { id }).Single().Dump("book1");
        book1.Title += " x";
        
        repository.Bookstore.Book.Update(book1);

        var newBook = new Bookstore.Book { Code = "+++", Title = "My new book" };
        repository.Bookstore.Book.Insert(newBook);
        
        repository.Bookstore.Book.Query().ToList().Dump(1);
        var incorrectBooks = repository.Bookstore.Book.Query(filterParameter);
        incorrectBooks.ToString().Dump();
        incorrectBooks.ToList().Dump(1);
        
        var incorrectBooks2 = repository.Bookstore.Book.Query()
            .Where(item => item.Title.Contains("curiousity"));
        incorrectBooks2.ToString().Dump();
        incorrectBooks2.ToList().Dump(1);
        
        incorrectBooks.Select(k => k.Title).ToString().Dump();
  }
}
