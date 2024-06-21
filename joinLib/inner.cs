using System.Data;
using System.Collections;
using DataRowCollection = System.Data.EnumerableRowCollection<System.Data.DataRow>;
using RowPairs = System.Collections.Generic.IEnumerable<JoinTables.RowPair>;    

namespace JoinTables;

public class RowPair : IEnumerator,IEnumerable
{
    internal  DataRow? r1;
    internal  DataRow? r2;
    private int position = -1;

    public IEnumerator GetEnumerator() => (IEnumerator)this;
    public bool MoveNext()
    {
        position++;
        return (position < 2);
    }
    public void Reset() => position = -1;

    public object Current
    {
        get
        {
            if (position == 0) return r1?? throw new System.InvalidOperationException();
            if (position == 1) return r2?? throw new System.InvalidOperationException();
            throw new System.InvalidOperationException();
        }
    }
}

public partial class EditableJoin :DataTable
{
    //https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/join-operations
    public RowPairs _InnerJoin(DataColumn id1, DataColumn id2)
    {
        RowPairs query = _InnerJoin(joinSet.Tables[0], id1, joinSet.Tables[1], id2);
        return query;
    }
    public RowPairs _InnerJoin(DataTable table1, DataColumn id1, DataTable table2, DataColumn id2)
    {
        if(id1.GetType() != id2.GetType() ) throw new System.Exception("Column typea must be the same");
        DataRowCollection rc1 = table1.AsEnumerable();
        DataRowCollection rc2 = table2.AsEnumerable();
        RowPairs query = _InnerJoin(rc1, id1, rc2, id2); 

        return query;
    }

    public RowPairs _InnerJoin()
    {
        RowPairs query = _InnerJoin(joinSet.Tables[0], joinSet.Tables[1]);
        return query;
    }
    public RowPairs _InnerJoin(DataTable table1, DataTable table2)
    {
        DataColumn id1 = table1.Columns["ID"]??throw new NullReferenceException();
        DataColumn id2 = table2.Columns[table1.TableName + "ID"]??throw new NullReferenceException();
        RowPairs query = _InnerJoin(table1, id1, table2, id2);
        return query;
    }

    private RowPairs _InnerJoin(DataRowCollection rc1, DataColumn id1, 
                                      DataRowCollection rc2, DataColumn id2)
    {
        RowPairs query = rc1.Join(rc2,
                r1 => r1[id1],
                r2 => r2[id2],
                (r1, r2) => new RowPair { r1 = r1, r2 = r2 });
        _Fill(query);

        return query;
    }
}
