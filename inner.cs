using System.Data;
namespace JoinTables;

public class RowPair
{
    public DataRow t1;
    public DataRow t2;
}
public partial class Join :DataTable
{
    public IEnumerable<RowPair>? Inner(DataTable table1, DataTable table2 )
    {
        var query = from t1 in table1.AsEnumerable()
                    join t2 in table2.AsEnumerable()
                    on t1.Field<int>("ID") equals t2.Field<int>(table1.TableName+"ID")
                    select new { t1, t2 };
        return query as IEnumerable<RowPair>;

    }
}
