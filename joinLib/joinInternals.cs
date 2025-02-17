using System.Data;
namespace JoinTables;
public partial class EditableJoin :DataTable
{
    private RowMapper rowMapper = new();

    private MapperDictionary rowDictionary = new();
    private DataSet joinSet = new();
    private DataColumn? _primaryKey;
    private DataColumn? _foreignKey;

    private DataColumn SourcePrimaryKey { 
        get => _primaryKey?? throw new Exception("Primary key not found"); 
        set => _primaryKey = value; }
    private DataColumn SourceForeignKey { 
        get => _foreignKey?? throw new Exception("Foreign key not found"); 
        set => _foreignKey = value; }

    private DataTable SourcePrimaryKeyTable { get => SourcePrimaryKey.Table?? throw new Exception("Table not found"); }
    private DataTable SourceForeignKeyTable { get => SourceForeignKey.Table?? throw new Exception("Table not found"); }

    private void Init(DataSet joinSet)
    {
        if(joinSet.Tables.Count != 2) throw new System.Exception("Must be exactly two tables");
        this.joinSet = joinSet;
        TableName = joinSet.Tables[0].TableName + joinSet.Tables[1].TableName;

    }



    private void ForEachTable(Action<DataTable> action)
    {
        foreach (DataTable table in joinSet.Tables)
        {
            action(table);
        }
    }

    private DataTable GetSourceTable(DataColumn c)
    {
        return GetSourceColumn(c).Table?? throw new System.Exception("Table not found");
    }
    
    private DataColumn GetSourceColumn(DataColumn c)
    {
        string FQName = c.ColumnName;
        var s = FQName.Split('.');
        DataTable table = joinSet.Tables[s[0]]?? throw new System.Exception("Table not found");
        DataColumn column = table.Columns[s[1]]?? throw new System.Exception("Column not found");
        return column;
    }

 }


