using System.Data;
namespace JoinTables;
public partial class Join :DataTable
{
    public void LeftOuter(Func<DataSet, DataRow, DataRow?> test)
    {
        foreach(DataRow leftRow in joinSet.Tables[0].Rows)
        {
            DataRow resultRow = NewRow();
            foreach (DataColumn c in joinSet.Tables[0].Columns)
            {
                resultRow[joinSet.Tables[0].FQColumnName(c)] = leftRow[c.ColumnName];
            }
            Rows.Add(resultRow);
            DataRow? rightRow = test(joinSet, leftRow);
            if(rightRow != null)
            {
                foreach (DataColumn c in rightRow.Table.Columns)
                {
                    resultRow[rightRow.Table.FQColumnName(c)] = rightRow[c.ColumnName];
                }
            }
        }
    }
}