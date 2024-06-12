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
        ForEachTable( (table) => {
            foreach (DataColumn c in table.Columns)
            {
                Columns.Add(table.FQColumnName(c), c.DataType);
            }
        });
    }

    public void LeftOuter(Func<DataSet, DataRow, DataRow?> test)
    {
        foreach(DataRow leftRow in joinSet.Tables[0].Rows)
        {
            DataRow resultRow = NewRow();
            foreach (DataColumn c in joinSet.Tables[0].Columns)
            {
                resultRow[joinSet.Tables[0].FQColumnName(c)] = leftRow[c.ColumnName];
            }
            Rows.Add(resultRow);
            DataRow? rightRow = test(joinSet, leftRow);
            if(rightRow != null)
            {
                foreach (DataColumn c in rightRow.Table.Columns)
                {
                    resultRow[rightRow.Table.FQColumnName(c)] = rightRow[c.ColumnName];
                }
            }
        }
    }
    private void _FillResult()
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


