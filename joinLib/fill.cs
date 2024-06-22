using System.Data;
using System.Collections;
using DataRowCollection = System.Data.EnumerableRowCollection<System.Data.DataRow>;
using RowPairs = System.Collections.Generic.IEnumerable<JoinTables.RowPair>;    

namespace JoinTables;

public partial class EditableJoin :DataTable
{
    public void _Fill(RowPairs query)
    {
        foreach (RowPair pair in query)
        {
            DataRow r1 = pair.r1?? throw new System.InvalidOperationException();
            DataRow r2 = pair.r2?? throw new System.InvalidOperationException();
            DataRow row = NewRow();
            foreach (DataColumn c in Columns) 
            {
                DataColumn sc = getSourceColumn(c);
                if(sc.Table == r1.Table)
                    row[c] = r1[sc];
                else if(sc.Table == r2.Table)
                    row[c] = r2[sc];
            }
            Rows.Add(row);
        }
        AcceptChanges();
    }
}