using System.Data;
using RowPairs = System.Collections.Generic.IEnumerable<JoinTables.RowPair>;

namespace JoinTables;

public partial class EditableJoin : DataTable
{
    public void _Fill(RowPairs query)
    {
        foreach (RowPair pair in query)
        {
            DataRow r1 = pair.r1 ?? throw new System.InvalidOperationException();
            DataRow r2 = pair.r2 ?? throw new System.InvalidOperationException();
            DataRow row = NewRow();
            foreach (DataColumn c in Columns)
            {
                DataColumn sc = GetSourceColumn(c);
                if (sc.Table == r1.Table)
                    row[c] = r1[sc];
                else if (sc.Table == r2.Table)
                    row[c] = r2[sc];
            }
            Rows.Add(row);

            DataRowMap rowMap = new();
            rowMap.Add(r1.Table, new TableMapper2 { SourceRow = r1 });
            rowMap.Add(r2.Table, new TableMapper2 { SourceRow = r2 });
            rowDictionary.Add(row, rowMap);

            DataRowMap rowsMap = rowDictionary[row];
            DataTable sourceTable = rowsMap[r1.Table].SourceTable;
            DataRow sourceRow = rowsMap[r1.Table].SourceRow;

            TableMapper mapper = new();
            mapper.Add(r1.Table.Rows.IndexOf(r1));
            mapper.Add(r2.Table.Rows.IndexOf(r2));
            rowMapper.Add(mapper);
        }
        AcceptChanges();
    }
}