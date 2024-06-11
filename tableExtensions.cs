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
