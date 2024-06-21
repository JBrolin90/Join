using System.Data;
using DataRowCollection = System.Data.EnumerableRowCollection<System.Data.DataRow>;
using RowPairs = System.Collections.Generic.IEnumerable<JoinTables.RowPair>;    

namespace JoinTables;
public partial class EditableJoin :DataTable
{

    private void _Fill()
    {
        int iRow;
        for (iRow = 0; iRow < joinSet.Tables[0].Rows.Count; iRow++)
        {
            Rows.Add(NewRow());
            ForEachTable((table) => 
            {
                if(iRow==0) TableName = TableName+table.TableName;
                DataRow row = this.LastRow();
                foreach (DataColumn c in table.Columns)
                {
                    row[table.FQColumnName(c)] = table.Rows[iRow][c.ColumnName];
                }

            });
        }
        AcceptChanges();
    }


    private void _Fill(RowPairs rowPairs)
    {
        this.Clear();
        foreach (RowPair pair in rowPairs)
        {
            DataRow newRow =  (NewRow());
            foreach(DataRow rRow in pair)
            {
                foreach (DataColumn c in rRow.Table.Columns)
                {
                    string name = rRow.Table.FQColumnName(c);
                    if(Columns.Contains(name))
                        newRow[name] = rRow[c.ColumnName];
                }
            }
            Rows.Add(newRow);
        }
        AcceptChanges();
    }
}