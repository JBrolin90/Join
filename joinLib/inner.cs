using System.Data;
using System.Collections;
using DataRowCollection = System.Data.EnumerableRowCollection<System.Data.DataRow>;
using RowPairs = System.Collections.Generic.IEnumerable<JoinTables.RowPair>;

namespace JoinTables;

public class RowPair : IEnumerator, IEnumerable
{
    public DataRow? r1;
    public DataRow? r2;
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
            if (position == 0) return r1 ?? throw new System.InvalidOperationException();
            if (position == 1) return r2 ?? throw new System.InvalidOperationException();
            throw new System.InvalidOperationException();
        }
    }
}

public partial class EditableJoin : DataTable
{
    //https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/join-operations
    public RowPairs _InnerJoin(DataColumn id1, DataColumn id2)
    {
        RowPairs query = _InnerJoin(joinSet.Tables[0], id1, joinSet.Tables[1], id2);
        return query;
    }
    public RowPairs _InnerJoin(DataTable table1, DataColumn id1, DataTable table2, DataColumn id2)
    {
        if (id1.GetType() != id2.GetType()) throw new System.Exception("Column typea must be the same");
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
        SourcePrimaryKey = table1.Columns["ID"] ?? throw new NullReferenceException();

        SourceForeignKey = table2.Columns[table1.TableName + "ID"] ?? throw new NullReferenceException();
        RowPairs query = _InnerJoin(table1, SourcePrimaryKey, table2, SourceForeignKey);
        return query;
    }

    private RowPairs _InnerJoin(DataRowCollection rc1, DataColumn primaryKey,
                                      DataRowCollection rc2, DataColumn foreignKey)
    {
        SourcePrimaryKey = primaryKey;
        SourceForeignKey = foreignKey;
        RowPairs query = rc1.Join(rc2,
                r1 => r1[primaryKey],
                r2 => r2[foreignKey],
                (r1, r2) => new RowPair { r1 = r1, r2 = r2 });
        _Fill(query);

        return query;
    }

    public void _InnerJoin(List<DataColumn> primaryKeys, List<DataColumn> foreignKeys)
    {
        RowPairs? query = _InnerJoin(joinSet.Tables[0].AsEnumerable(), primaryKeys, joinSet.Tables[1].AsEnumerable(), foreignKeys);
        _Fill(query);
    }
    public bool CompareValues(DataRow r1, DataColumn c1, DataRow r2, DataColumn c2)
    {

        if (c1.DataType == typeof(string) && c2.DataType == typeof(string))
        {
            return string.Compare(r1[c1].ToString(), r2[c2].ToString(), StringComparison.OrdinalIgnoreCase) == 0;
        }
        else
        {
            return r1[c1].Equals(r2[c2]);
        }
    }

    public RowPairs? _InnerJoin(DataRowCollection rc1, List<DataColumn> primaryKeys, DataRowCollection rc2, List<DataColumn> foreignKeys)
    {
        if (primaryKeys.Count != foreignKeys.Count) throw new ArgumentException("Primary and foreign keys must have the same count");
        int count = primaryKeys.Count;
        List<RowPair> result = new List<RowPair>();

        foreach (DataRow row1 in rc1)
        {
            foreach (DataRow row2 in rc2)
            {
                bool match = true;
                for (int i = 0; i < count; i++)
                {
                    match &= CompareValues(row1, primaryKeys[i], row2, foreignKeys[i]);
                }
                if (match)
                {
                    result.Add(new RowPair { r1 = row1, r2 = row2 });
                }
            }
        }
        return result.Count > 0 ? result : null;
    }
}