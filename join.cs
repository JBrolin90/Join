using System.Data;


class Join
{
    public DataSet joinSet;
    public DataTable result = new DataTable();
    public Join(DataSet joinSet)
    {
        this.joinSet = joinSet;
    }
    public void BuildResultTable()
    {
        result.TableName = "result";
        foreach (DataTable table in joinSet.Tables)
        {
            foreach (DataColumn c in table.Columns)
            {
                result.Columns.Add(table.FQColumnName(c), c.DataType);
            }
        }
    }
    public void FillResult()
    {
        int iRow;
        for (iRow = 0; iRow < joinSet.Tables[0].Rows.Count; iRow++)
        {
            DataRow newRow = result.NewRow();
            foreach (DataTable table in joinSet.Tables)
            {
                foreach (DataColumn c in table.Columns)
                {
                    newRow[table.FQColumnName(c)] = table.Rows[iRow][c.ColumnName];
                }
            }
            result.Rows.Add(newRow);
        }
    }

    public void PrintResult()
    {
        foreach (DataRow r in result.Rows)
        {
            foreach (DataColumn c in result.Columns)
            {
                System.Console.Write(c.ColumnName + ": " + r[c.ColumnName] + ", ");
            }
            System.Console.WriteLine();
        }
    }
}


public static class DataTableExtensions
{
    public static string FQColumnName(this DataTable table, DataColumn c)
    {
        return table.TableName + "." + c.ColumnName;
    }
}

public class FewTables
{
    public DataTable t1 = new ();
    public DataTable t2 = new ();
    public DataSet joinSet = new DataSet();
    public FewTables()
    {
        Setup();
    }

    public void PrintTable(DataTable t)
    {
        foreach (DataRow r in t.Rows)
        {
            foreach (DataColumn c in t.Columns)
            {
                System.Console.Write(c.ColumnName + ": " + r[c.ColumnName] + ", ");
            }
            System.Console.WriteLine();
        }
    }

    public void Setup()
    {
        t1.TableName = "t1";
        t1.Columns.Add("ID", typeof(int));
        t1.Columns.Add("Name", typeof(string));

        t2.TableName = "t2";
        t2.Columns.Add("ID", typeof(int));
        t2.Columns.Add("t1ID", typeof(int));
        t2.Columns.Add("Name", typeof(string));

        t1.Rows.Add(1, "A");
        t1.Rows.Add(2, "B");

        t2.Rows.Add(1, 1, "A1");
        t2.Rows.Add(2, 1, "A2");
        joinSet.Tables.Add(t1);
        joinSet.Tables.Add(t2);

    }
}


