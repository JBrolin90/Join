using System.Data;
namespace JoinTables;
public partial class Join :DataTable
{
    private DataSet joinSet = new();
    private void Init(DataSet joinSet)
    {
        this.joinSet = joinSet;
        _SelectAll();
    }



    private void _Fill()
    {
        int iRow;
        for (iRow = 0; iRow < joinSet.Tables[0].Rows.Count; iRow++)
        {
            Rows.Add(NewRow());
            ForEachTable((table) => 
            {
                if(iRow==0) TableName = TableName+table.TableName;
                DataRow row = this.LastRow();
                foreach (DataColumn c in table.Columns)
                {
                    row[table.FQColumnName(c)] = table.Rows[iRow][c.ColumnName];
                }

            });
        }
        AcceptChanges();
    }

    private void ForEachColumn(Action<DataColumn> action)
    {
        foreach (DataColumn c in Columns)
        {
            action(c);
        }
    }

    private void ForEachTable(Action<DataTable> action)
    {
        foreach (DataTable table in joinSet.Tables)
        {
            action(table);
        }
    }


    private DataColumn getSourceColumn(DataColumn c)
    {
        string FQName = c.ColumnName;
        var s = FQName.Split('.');
        DataTable table = joinSet.Tables[s[0]]?? throw new System.Exception("Table not found");
        DataColumn column = table.Columns[s[1]]?? throw new System.Exception("Column not found");
        return column;
    }

 }


