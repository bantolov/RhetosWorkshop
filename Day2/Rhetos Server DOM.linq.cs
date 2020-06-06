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
