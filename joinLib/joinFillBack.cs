
using System.Data;
using System.Data.Common;

namespace JoinTables;


public partial class EditableJoin :DataTable
{

    public void FillBack()
    {
        FillBackDeletedRows();
        FillBackNewRows();
        FillBackChangedRows();
        AcceptChanges();
        if(rowMapper.Count != Rows.Count) throw new System.Exception("RowMapper count mismatch");

    }

    private DataRow GetSourceRow(DataRow joinRow, DataColumn sourceColumn)
    {
        DataTable sourceTable = sourceColumn.Table ?? throw new System.Exception("Table not found");
        int iRowMap = Rows.IndexOf(joinRow);
        int iSourceTable = joinSet.Tables.IndexOf(sourceTable); 
        int sourceRow = rowMapper[iRowMap][iSourceTable];
        return sourceTable.Rows[sourceRow];
    }

    private void FillBackChangedRows()
    {
        this.PrintRowStateCount( DataRowState.Modified);
        foreach (DataRow joinRow in Rows)
        {
            if (joinRow.RowState == DataRowState.Modified)
            {
                int iRowMap = Rows.IndexOf(joinRow);
                foreach (DataColumn joinColumn in Columns)
                {
                    DataColumn sourceColumn = GetSourceColumn(joinColumn);
                    GetSourceRow(joinRow, sourceColumn)[sourceColumn] = joinRow[joinColumn];
                }
            }
        }
    }


    private void FillBackNewRows()
    {
        this.PrintRowStateCount(DataRowState.Added);
        foreach (DataRow joinRow in Rows)
        {
            if (joinRow.RowState == DataRowState.Added)
            {
                TableMapper mapper = new();
                foreach(DataTable table in joinSet.Tables)
                {
                    bool hasData = false;
                    DataRow newRow = table.NewRow();
                    foreach (DataColumn joinColumn in Columns)
                    {
                        DataColumn sourceColumn = GetSourceColumn(joinColumn);
                        if(sourceColumn.Table == table) {
                            if(joinRow[joinColumn] != DBNull.Value) {
                                newRow[sourceColumn] = joinRow[joinColumn];
                                hasData = true;
                            }
                        }
                    }
                    if(hasData==true) {
                        table.Rows.Add(newRow);
                    }
                    mapper.Add(table.Rows.IndexOf(newRow));
                }
                rowMapper.Add(mapper);
            }
        }
    }

    private void FillBackDeletedRows()
    {
        this.PrintRowStateCount(DataRowState.Deleted);
        for(int iJoinRow = Rows.Count-1; iJoinRow >= 0; iJoinRow-- )
        {
            DataRow? joinRow = Rows[iJoinRow];
            if (joinRow.RowState == DataRowState.Deleted)
            {
                foreach(DataTable table in joinSet.Tables)
                {
                    int iSourceTable = joinSet.Tables.IndexOf(table);
                    DataRow sourceRow = table.Rows[rowMapper[iJoinRow][iSourceTable]];
                    sourceRow.Delete();
                }
                rowMapper.RemoveAt(iJoinRow);
            }
        }
    }
}