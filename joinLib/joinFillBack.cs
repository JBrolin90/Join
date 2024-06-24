
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
                var rowMap = rowDictionary[joinRow];
                foreach (DataColumn joinColumn in Columns)
                {
                    DataTable sourceTable = GetSourceTable(joinColumn);
                    DataColumn sourceColumn = GetSourceColumn(joinColumn);
                    DataRow sourceRow = rowMap[sourceTable].SourceRow;

                    sourceRow[sourceColumn] = joinRow[joinColumn];
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
                rowDictionary.Add(joinRow, new DataRowMap());
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
                        rowDictionary[joinRow].Add(table, new TableMapper2 { SourceRow = newRow });
                    }
                    mapper.Add(table.Rows.IndexOf(newRow));
                }
                rowMapper.Add(mapper);
            }
        }
    }

    private void RemoveRows(DataTable table, DataColumn column, object value )
    {
        foreach(DataRow joinRow in rowDictionary.Keys){
            DataTable t = rowDictionary[joinRow][table].SourceTable;
            DataRow r = rowDictionary[joinRow][table].SourceRow;
            var x = r[column];
            if(x.Equals(value)) {
                r.Delete();
                rowDictionary[joinRow].Remove(table);
            }
        }
    }

    private void FillBackDeletedRows()
    {
        this.PrintRowStateCount(DataRowState.Deleted);
        for(int iJoinRow = Rows.Count-1; iJoinRow >= 0; iJoinRow-- )
        {
            DataRow joinRow = Rows[iJoinRow];
            if (joinRow.RowState == DataRowState.Deleted)
            {
                object ID = rowDictionary[joinRow][SourcePrimaryKeyTable].SourceRow[SourcePrimaryKey];

                //Enforce referential integrity
                RemoveRows(SourceForeignKeyTable, SourceForeignKey, ID);

                rowDictionary[joinRow][SourcePrimaryKeyTable].SourceRow.Delete();
                rowDictionary.Remove(joinRow);
                rowMapper.RemoveAt(iJoinRow);
                foreach(DataRow row in Rows) {
                    if( row.RowState != DataRowState.Deleted) {
                        var x = row[SourcePrimaryKeyTable.FQColumnName(SourcePrimaryKey)];
                        if(x.Equals(ID)) {
                            rowDictionary.Remove(row);
                            row.Delete();
                        }
                    }
                }
            }
        }
    }
}