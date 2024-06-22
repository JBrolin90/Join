using System.Data;
namespace JoinTables;
public static class DataTableExtensions
{
    public static string FQColumnName(this DataTable table, DataColumn c)
    {
        return table.TableName + "." + c.ColumnName;
    }
    public static DataRow LastRow(this DataTable table)
    {
        return table.Rows[table.Rows.Count - 1];
    }
   public static void PrintTable(this DataTable table)
    {
        Console.WriteLine($"Table: {table.TableName} ({table.Rows.Count} rows)");
        foreach (DataRow r in table.Rows)
        {
            foreach (DataColumn c in table.Columns)
            {
                System.Console.Write(c.ColumnName + ": " + r[c.ColumnName] + ", ");
            }
            System.Console.WriteLine();
        }
    }
}

public static class DataRowExtensions
{
    public static void Delete(this DataRow row)
    {
        row.Table.Rows.Remove(row);
    }
    public static void Print(this DataRow row, bool lf = true)
    {
        foreach (DataColumn c in row.Table.Columns)
        {
            System.Console.Write(c.ColumnName + ": " + row[c.ColumnName] + ", ");
        }
        if (lf)
        {
            System.Console.WriteLine();
        }
    }
}   
