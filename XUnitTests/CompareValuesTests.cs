using System;
using System.Data;
using Xunit;
using System;
using JoinTables;

namespace XUnitTests;


//namespace JoinTables.Tests;

public class EditableJoinTests
{
    private EditableJoin editableJoin;
    private DataSet joinSet = new DataSet();
    private DataTable table1 = new();
    private DataTable table2 = new();

    public EditableJoinTests()
    {
        joinSet.Tables.Add(table1);
        joinSet.Tables.Add(table2);
        editableJoin = new EditableJoin(joinSet);
    }

    [Fact]
    public void CompareValues_StringColumns_CaseInsensitiveMatch_ReturnsTrue()
    {
        // Arrange
        var table1 = new DataTable();
        table1.Columns.Add("Name", typeof(string));
        var row1 = table1.NewRow();
        row1["Name"] = "John";
        table1.Rows.Add(row1);

        var table2 = new DataTable();
        table2.Columns.Add("Name", typeof(string));
        var row2 = table2.NewRow();
        row2["Name"] = "JOHN";
        table2.Rows.Add(row2);

        // Act
        bool result = editableJoin.CompareValues(row1, table1.Columns["Name"]!, row2, table2.Columns["Name"]!);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CompareValues_StringColumns_DifferentValues_ReturnsFalse()
    {
        // Arrange
        var table1 = new DataTable();
        table1.Columns.Add("Name", typeof(string));
        var row1 = table1.NewRow();
        row1["Name"] = "John";
        table1.Rows.Add(row1);

        var table2 = new DataTable();
        table2.Columns.Add("Name", typeof(string));
        var row2 = table2.NewRow();
        row2["Name"] = "Jane";
        table2.Rows.Add(row2);

        // Act
        bool result = editableJoin.CompareValues(row1, table1.Columns["Name"]!, row2, table2.Columns["Name"]!);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CompareValues_IntegerColumns_EqualValues_ReturnsTrue()
    {
        // Arrange
        var table1 = new DataTable();
        table1.Columns.Add("ID", typeof(int));
        var row1 = table1.NewRow();
        row1["ID"] = 1;
        table1.Rows.Add(row1);

        var table2 = new DataTable();
        table2.Columns.Add("ID", typeof(int));
        var row2 = table2.NewRow();
        row2["ID"] = 1;
        table2.Rows.Add(row2);

        // Act
        bool result = editableJoin.CompareValues(row1, table1.Columns["ID"]!, row2, table2.Columns["ID"]!);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CompareValues_IntegerColumns_DifferentValues_ReturnsFalse()
    {
        // Arrange
        var table1 = new DataTable();
        table1.Columns.Add("ID", typeof(int));
        var row1 = table1.NewRow();
        row1["ID"] = 1;
        table1.Rows.Add(row1);

        var table2 = new DataTable();
        table2.Columns.Add("ID", typeof(int));
        var row2 = table2.NewRow();
        row2["ID"] = 2;
        table2.Rows.Add(row2);

        // Act
        bool result = editableJoin.CompareValues(row1, table1.Columns["ID"]!, row2, table2.Columns["ID"]!);

        // Assert
        Assert.False(result);
    }
}
