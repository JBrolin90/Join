using System;
using System.Data;
using Xunit;
using JoinTables;

namespace JoinLibTests
{
    public class EditableJoinTests
    {
        // Helper method to create a test DataSet with two DataTables.
        private DataSet CreateTestDataSet()
        {
            DataSet ds = new DataSet();

            // Create first table (t1) with "ID" and "Name".
            DataTable dt1 = new DataTable("t1");
            dt1.Columns.Add("ID", typeof(int));
            dt1.Columns.Add("Name", typeof(string));
            dt1.PrimaryKey = [dt1.Columns["ID"]];
            dt1.Rows.Add(1, "Alice");
            dt1.Rows.Add(2, "Bob");
            ds.Tables.Add(dt1);

            // Create second table (t2) with "t1ID" and "Detail".
            DataTable dt2 = new DataTable("t2");
            dt2.Columns.Add("ID", typeof(int));
            dt2.Columns.Add("t1ID", typeof(int));
            dt2.Columns.Add("Detail", typeof(string));
            dt2.Rows.Add(1, 1, "Detail A");
            dt2.Rows.Add(2, 2, "Detail B");
            dt2.Rows.Add(3, 5, "Detail C"); // This row should not be joined.
            ds.Tables.Add(dt2);

            return ds;
        }

        [Fact]
        public void InnerJoin_Should_ReturnOnlyMatchingRows()
        {
            // Arrange
            DataSet ds = CreateTestDataSet();
            EditableJoin join = new EditableJoin(ds);

            // Act
            join.SelectAll();
            join.InnerJoin();
            DataTable joined = join;

            // Assert: Only rows with matching keys (ID 1 and 2) should be joined.
            Assert.NotNull(joined);
            Assert.Equal(2, joined.Rows.Count);
        }

        [Fact]
        public void FillBack_Should_UpdateOriginalTable()
        {
            // Arrange
            DataSet ds = CreateTestDataSet();
            EditableJoin join = new EditableJoin(ds);
            join.SelectAll();
            join.InnerJoin();
            DataTable joined = join;

            // Modify the joined table, e.g., update the "Name" for ID = 1.
            DataRow row = joined.Select("t1.ID = 1")[0];
            row["t1.Name"] = "Updated Alice";

            // Act: Propagate changes back to original tables.
            join.FillBack();

            // Assert: Verify the change is reflected in the original DataTable.
            DataTable dt1 = ds.Tables["t1"];
            DataRow original = dt1.Rows.Find(1);
            Assert.Equal("Updated Alice", original["Name"]);
        }

        [Fact]
        public void PrintTable_Should_ExecuteWithoutException()
        {
            // Arrange
            DataSet ds = CreateTestDataSet();
            EditableJoin join = new EditableJoin(ds);
            join.SelectAll();
            join.InnerJoin();

            // Act & Assert: Ensure PrintTable runs without throwing exceptions.
            var exception = Record.Exception(() => join.PrintTable());
            Assert.Null(exception);
        }
    }
}