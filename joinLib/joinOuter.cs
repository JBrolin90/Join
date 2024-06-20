using System.Data;
using System.Security.Cryptography;
namespace JoinTables;
public partial class EditableJoin :DataTable
{
    public void OuterJoin(DataTable table1, DataTable table2 )
    {
        var query = from t1 in table1.AsEnumerable()
                    join t2 in table2.AsEnumerable()
                    on t1.Field<int>("ID") equals t2.Field<int>("ID") into groupJoin
                    from t2 in groupJoin.DefaultIfEmpty()
                    select new { t1, t2 };
            foreach (var item in query)
        {
            {
                item.t1.Delete();
            }
            {
                item.t2.Delete();
            }
            {
                item.t1.Delete();
            }
            {
                item.t2.Delete();
            }
        }
   }
   public void LeftJoin(DataTable table1, DataTable table2)
   {
       var query = from t1 in table1.AsEnumerable()
                   join t2 in table2.AsEnumerable()
                   on t1.Field<int>("ID") equals t2.Field<int>("ID") into groupJoin
                   from t2 in groupJoin.DefaultIfEmpty()
                   select new { t1, t2 };
       foreach (var item in query)
       {
           Console.WriteLine($"ID: {item.t1[0]}, Name: {item.t1[1]}, Age: {item.t1[2]}, Address: {item.t2[3]}");
       }
   }

   // q: translate SELECT * FROM table1; to Linq
    // a: from t1 in table1.AsEnumerable() select t1;
    public void SelectAll(DataTable table)
    {
        var query = from t1 in table.AsEnumerable() select t1;
        foreach (var item in query)
        {
            DataColumn column = item.Table.Columns["ID"]?? throw new System.Exception("Column not found");
            column.ColumnName = "New Name";
            Console.WriteLine($"ID: {item.Field<int>("ID")}, Name: {item.Field<string>("Name")}, Age: {item.Field<int>("Age")}");
        }
    }

}