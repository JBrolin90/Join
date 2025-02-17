You can use joinLib as a library to perform join operations between two DataTables contained in a DataSet. For example, the library exposes a partial class JoinTables.EditableJoin that lets you generate a join view of two tables using methods such as:

SelectAll() – to automatically add all columns (using the fully qualified names from each source table).
InnerJoin() – to perform an inner join between the two tables based on a primary–foreign key pair (the default convention is that the first table has an "ID" column and the second table has a column named "[FirstTableName]ID").
FillBack() – to propagate any changes, additions, or deletions made in the joined table back to the original tables.
A full usage example is demonstrated in joinTest.cs. In that file, a DataSet with two tables is created, and then an instance of JoinTables.EditableJoin is created with the DataSet. After calling SelectAll() and InnerJoin(), the joined table can be modified and then pushed back to the original data via FillBack().

Here’s a short example:

```
using System.Data;
using JoinTables;

// Assume ds is a DataSet with exactly two tables, following
// the naming conventions required (for instance, t1 with column "ID"
// and t2 with a column "t1ID") 
DataSet ds = new DataSet();  
// populate ds with two appropriate DataTables

EditableJoin join = new EditableJoin(ds);
join.SelectAll();
join.InnerJoin();

// Work with the join data (modify, add, delete rows)
join.PrintTable();

// Propagate the changes back to the source tables.
join.FillBack();
```