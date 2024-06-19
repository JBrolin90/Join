using System.Data;
namespace JoinTables;

public struct RowPair
{
    public DataRow r1;
    public DataRow r2;
}
public partial class Join :DataTable
{
    public IEnumerable<RowPair> Inner(DataTable table1, DataTable table2 )
    {
        var query = from r1 in table1.AsEnumerable()
                    join r2 in table2.AsEnumerable()
                    on r1.Field<int>("ID") equals r2.Field<int>(table1.TableName+"ID")
                    select new { r1, r2 };
        return query as IEnumerable<RowPair>?? Enumerable.Empty<RowPair>();
    }
}
