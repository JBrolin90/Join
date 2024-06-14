using System.Data;

namespace JoinTables;


public partial class Join :DataTable
{

    private void _SelectAll()
    {
        Columns.Clear();
        ForEachTable( (table) => {
            foreach (DataColumn c in table.Columns)
            {
                Columns.Add(table.FQColumnName(c), c.DataType);
            }
        });
    }

    private void _Select(List<DataColumn> columns)
    {
        Columns.Clear();
        foreach (DataColumn c in columns)
        {
            DataTable t = c.Table?? throw new System.Exception("Table not found");
            Columns.Add(t.FQColumnName(c), c.DataType);
        }
    }


}