
using System.Data;

namespace JoinTables;

internal class TableMapper2
{
    private DataRow? soureRow;

    internal DataTable SourceTable { 
        get => SourceRow.Table?? throw new System.Exception("Table not found"); 
        private set{} }
    internal DataRow SourceRow { 
        get => soureRow?? throw new System.Exception("Row not found");
        set => soureRow = value; }
}

class DataRowMap: Dictionary<DataTable, TableMapper2>
{
}

class MapperDictionary: Dictionary<DataRow, DataRowMap>
{
}

class TableMapper: List<int>
{
}
class RowMapper: List<TableMapper>
{
}
