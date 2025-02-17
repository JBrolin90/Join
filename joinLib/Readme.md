joinLib
joinLib is a .NET library that provides functionality for joining two DataTable objects contained in a DataSet. Its primary goal is to offer a joined view of the data, allowing you to perform modifications and then propagate those changes back to the original tables.

Features
SelectAll(): Automatically add all columns from the joined tables using fully qualified names.
InnerJoin(): Performs an inner join between two tables based on a primary–foreign key pair. By convention, the first table must have an "ID" column and the second table a corresponding column named "[FirstTableName]ID".
FillBack(): Pushes changes made to the join view (modifications, additions, or deletions) back to the source DataTables.
PrintTable(): Utility method to print the contents of a DataTable to the console for debugging and verification purposes.
Usage
To use joinLib, create a DataSet with exactly two DataTables that follow the naming conventions. Then, create an instance of JoinTables.EditableJoin and call the appropriate methods to perform your operations.

A full usage example is provided in the Usage.md as well as in the joinTest.cs file.

Example
```
using System.Data;
using JoinTables;

// Create a DataSet with two DataTables (t1 and t2)
// t1 must include an "ID" column and t2 a "[t1]ID" column.
DataSet ds = new DataSet();
// Populate ds with two appropriate DataTables...

EditableJoin join = new EditableJoin(ds);
join.SelectAll();
join.InnerJoin();

// Work with the join data (e.g., modify rows, add new rows, delete rows).
join.PrintTable();

// Propagate the changes back to the original DataTables.
join.FillBack();
```

Project Structure
joinLib/: Contains the library source code.
join.cs – The main entry point class for EditableJoin.
Other files like joinSelect.cs, inner.cs, and joinFillBack.cs provide implementations for the join operations.
Usage.md – Detailed description and example of how to use the library.
console/: Hosts an example application that demonstrates how to use joinLib (see joinTest.cs and Program.cs).
NUnitTests/ and XUnitTests/: Test projects for validating joinLib functionality.
Building and Testing
To build the project, use the provided VS Code tasks or run the following command:

```
dotnet build joinLib/joinLib.csproj
```
To run the console example, launch the join Debug configuration from Visual Studio Code. 