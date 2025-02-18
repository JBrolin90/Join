using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JoinTables;
using Xunit;

namespace XUnitTests;


public class MultipleKeysJoinTests
{

    public MultipleKeysJoinTests()
    {
    }

    [Fact]
    public void InnerJoin_WithMatchingRows_ReturnsCorrectResult()
    {
        // Arrange
        var table1 = new DataTable("Table1");
        table1.Columns.Add("ID", typeof(int));
        table1.Columns.Add("Name", typeof(string));
        table1.Rows.Add(1, "John");
        table1.Rows.Add(2, "Jane");

        var table2 = new DataTable("Table2");
        table2.Columns.Add("ForeignID", typeof(int));
        table2.Columns.Add("Department", typeof(string));
        table2.Rows.Add(1, "IT");
        table2.Rows.Add(2, "HR");

        DataSet joinSet = new DataSet();
        joinSet.Tables.Add(table1);
        joinSet.Tables.Add(table2);
        EditableJoin editableJoin = new(joinSet); ;


        var primaryKeys = new List<DataColumn> { table1.Columns["ID"]! };
        var foreignKeys = new List<DataColumn> { table2.Columns["ForeignID"]! };

        // Act
        var result = editableJoin._InnerJoin(table1.AsEnumerable(), primaryKeys, table2.AsEnumerable(), foreignKeys);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, pair => Assert.Equal(pair.r1!["ID"]!, pair.r2!["ForeignID"]!));
    }

    [Fact]
    public void InnerJoin_WithNoMatchingRows_ReturnsNull()
    {
        // Arrange
        var table1 = new DataTable("Table1");
        table1.Columns.Add("ID", typeof(int));
        table1.Rows.Add(1);
        table1.Rows.Add(2);

        var table2 = new DataTable("Table2");
        table2.Columns.Add("ForeignID", typeof(int));
        table2.Rows.Add(3);
        table2.Rows.Add(4);

        DataSet joinSet = new DataSet();
        joinSet.Tables.Add(table1);
        joinSet.Tables.Add(table2);
        EditableJoin editableJoin = new(joinSet); ;


        var primaryKeys = new List<DataColumn> { table1.Columns["ID"]! };
        var foreignKeys = new List<DataColumn> { table2.Columns["ForeignID"]! };

        // Act
        var result = editableJoin._InnerJoin(table1.AsEnumerable(), primaryKeys, table2.AsEnumerable(), foreignKeys);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void InnerJoin_WithMultipleKeys_ReturnsCorrectResult()
    {
        // Arrange
        var table1 = new DataTable("Table1");
        table1.Columns.Add("ID", typeof(int));
        table1.Columns.Add("Name", typeof(string));
        table1.Rows.Add(1, "John");
        table1.Rows.Add(2, "Jane");
        table1.Rows.Add(1, "Jack");

        var table2 = new DataTable("Table2");
        table2.Columns.Add("ForeignID", typeof(int));
        table2.Columns.Add("ForeignName", typeof(string));
        table2.Columns.Add("Department", typeof(string));
        table2.Rows.Add(1, "John", "IT");
        table2.Rows.Add(2, "Jane", "HR");
        table2.Rows.Add(1, "Jack", "Sales");

        DataSet joinSet = new DataSet();
        joinSet.Tables.Add(table1);
        joinSet.Tables.Add(table2);
        EditableJoin editableJoin = new(joinSet); ;


        var primaryKeys = new List<DataColumn> { table1.Columns["ID"]!, table1.Columns["Name"]! };
        var foreignKeys = new List<DataColumn> { table2.Columns["ForeignID"]!, table2.Columns["ForeignName"]! };

        // Act
        var result = editableJoin._InnerJoin(table1.AsEnumerable(), primaryKeys, table2.AsEnumerable(), foreignKeys);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.All(result, pair =>
        {
            Assert.Equal(pair.r1!["ID"], pair.r2!["ForeignID"]);
            Assert.Equal(pair.r1["Name"], pair.r2["ForeignName"]);
        });
    }

    [Fact]
    public void InnerJoin_WithMismatchedKeyCount_ThrowsArgumentException()
    {
        // Arrange
        var table1 = new DataTable("Table1");
        table1.Columns.Add("ID", typeof(int));
        table1.Rows.Add(1);

        var table2 = new DataTable("Table2");
        table2.Columns.Add("ForeignID", typeof(int));
        table2.Columns.Add("Name", typeof(string));
        table2.Rows.Add(1, "John");

        DataSet joinSet = new DataSet();
        joinSet.Tables.Add(table1);
        joinSet.Tables.Add(table2);
        EditableJoin editableJoin = new(joinSet); ;

        var primaryKeys = new List<DataColumn> { table1.Columns["ID"]! };
        var foreignKeys = new List<DataColumn> { table2.Columns["ForeignID"]!, table2.Columns["Name"]! };

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            editableJoin._InnerJoin(table1.AsEnumerable(), primaryKeys, table2.AsEnumerable(), foreignKeys));
    }
}
