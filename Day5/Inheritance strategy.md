# Inheritance strategy

## Example of inheritance hierarchy

(this is not an actual Rhetos object model)

```cs
public class SalesItem
{
    public string Code;
    public decimal Price;
}
public class Book : SalesItem
{
    public int NumberOfPages;
}
public class Food : SalesItem
{
    public int Calories;
}
```

## Standard options for database implementation

OPTION 1: Table per Hierarchy

```sql
CREATE TABLE SalesItem (ObjectType, Code, Price, NumberOfPages, Calories)

-- In Rhetos DSL: Single Entity + validations for ObjectType.
```

OPTION 2: Table per Type

```sql
CREATE TABLE SalesItem (Code, Price)
CREATE TABLE Book (NumberOfPages), FOREIGN KEY (ID → SalesItem.ID)
CREATE TABLE Food (Calories), FOREIGN KEY (ID → SalesItem.ID)

-- In Rhetos DSL: use "Extends"
```

OPTION 3: Table per Concrete Class

```sql
CREATE TABLE Book (Code, Price, NumberOfPages)
CREATE TABLE Food (Code, Price, Calories)
CREATE VIEW SalesItem
    SELECT Code, Price FROM Book
    UNION ALL
    SELECT Code, Price FROM Food

-- In Rhetos DSL: use "Polymorphic"
```
