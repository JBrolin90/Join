
using System.Data;

namespace JoinTables;


public partial class EditableJoin :DataTable
{

    public void FillBack()
    {
        FillBackDeletedRows();
        FillBackNewRows();
        FillBackChangedRows();
        AcceptChanges();
    }

    private void FillBackChangedRows()
    {
        PrintRowStateCount(DataRowState.Modified);
        foreach (DataRow r in Rows)
        {
            if (r.RowState == DataRowState.Modified)
            {
                foreach (DataColumn c in Columns)
                {
                    DataColumn? originalColumn = getSourceColumn(c);
                    originalColumn!.Table!.Rows[r.Table.Rows.IndexOf(r)][originalColumn!.ColumnName] = r[c.ColumnName];
                }
            }
        }
    }

    private void PrintRowStateCount(DataRowState rowState)
    {
        int ModifiedRowsCount = 0;
        foreach (DataRow r in Rows)
        {
            if (r.RowState == rowState)
            {
                ModifiedRowsCount++;
            }
        }
        Console.WriteLine($"{rowState.ToString()} rows: {ModifiedRowsCount}");
    }

    private void FillBackNewRows()
    {
        PrintRowStateCount(DataRowState.Added);
        foreach (DataRow r in Rows)
        {
            if (r.RowState == DataRowState.Added)
            {
                foreach(DataTable table in joinSet.Tables)
                {
                    DataRow newRow = table.NewRow();
                    foreach (DataColumn c in table.Columns)
                    {
                        newRow[c.ColumnName] = r[table.FQColumnName(c)];
                    }
                    table.Rows.Add(newRow);
                }
            }
        }
    }

    private void FillBackDeletedRows()
    {
        PrintRowStateCount(DataRowState.Deleted);
        for(int i = 0; i < Rows.Count; i++ )
        {
            if (Rows[i].RowState == DataRowState.Deleted)
            {
                foreach(DataTable table in joinSet.Tables)
                {
                    table.Rows[i].Delete();
                }
            }
        }
    }
}