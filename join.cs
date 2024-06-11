using System.Data;

namespace JoinTables;


class Join :DataTable
{

    public Join(DataSet joinSet) => Init(joinSet);
    public void FillResult() => _FillResult();




    private DataSet joinSet = new();
    private void Init(DataSet joinSet)
    {
        this.joinSet = joinSet;
        BuildResultTable();
    }

    private void BuildResultTable()
    {
        foreach (DataTable table in joinSet.Tables)
        {
            foreach (DataColumn c in table.Columns)
            {
                Columns.Add(table.FQColumnName(c), c.DataType);
            }
        }
    }
    private void _FillResult()
    {
        int iRow;
        for (iRow = 0; iRow < joinSet.Tables[0].Rows.Count; iRow++)
        {
            DataRow newRow = NewRow();
            foreach (DataTable table in joinSet.Tables)
            {
                foreach (DataColumn c in table.Columns)
                {
                    newRow[table.FQColumnName(c)] = table.Rows[iRow][c.ColumnName];
                }
            }
            Rows.Add(newRow);
        }
    }

    private void ForEachTasble(Action action)
    {
        foreach (DataTable table in joinSet.Tables)
        {
            action();
        }
    }

    public void FillBack()
    {
        FillBackDeletedRows();
        FillBackNewRows();
        FillBackChangedRows();
        AcceptChanges();
    }

    private void FillBackChangedRows()
    {
        foreach (DataRow r in Rows)
        {
            if (r.RowState == DataRowState.Modified)
            {
                foreach(DataColumn c in Columns)
                {
                    DataColumn? originalColumn = getOriginalColumn(c);
                    originalColumn!.Table!.Rows[r.Table.Rows.IndexOf(r)][originalColumn!.ColumnName] = r[c.ColumnName];
                }
            }
        }
    }
    private void FillBackNewRows()
    {
        foreach (DataRow r in Rows)
        {
            if (r.RowState == DataRowState.Added)
            {
                foreach(DataTable table in joinSet.Tables)
                {
                    table.Rows.Add();
                }
                foreach (DataColumn c in Columns)
                {
                    DataColumn? originalColumn = getOriginalColumn(c);
                    DataTable table = originalColumn!.Table!;
                    table.LastRow()[originalColumn!.ColumnName] = r[c.ColumnName];
                }
            }
        }
    }

    private void FillBackDeletedRows()
    {
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

    private DataColumn? getOriginalColumn(DataColumn c)
    {
        string FQName = c.ColumnName;
        var s = FQName.Split('.');
        DataTable table = joinSet.Tables[s[0]]?? throw new System.Exception("Table not found");
        DataColumn? column = table.Columns[s[1]];
        return column;
    }

 }


