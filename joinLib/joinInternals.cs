using System.Data;
namespace JoinTables;
public partial class EditableJoin :DataTable
{
    private DataSet joinSet = new();
    private void Init(DataSet joinSet)
    {
        if(joinSet.Tables.Count != 2) throw new System.Exception("Must be exactly two tables");
        this.joinSet = joinSet;
        TableName = joinSet.Tables[0].TableName + joinSet.Tables[1].TableName;
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


