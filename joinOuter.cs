using System.Data;
namespace JoinTables;
public partial class Join :DataTable
{
    // Method that uses Linq to join table
    public void InnerJoin(DataTable table1, DataTable table2 )
    {
        var query = Inner(table1, table2);
        var Xquery = from t1 in table1.AsEnumerable()
                    join t2 in table2.AsEnumerable()
                    on t1.Field<int>("ID") equals t2.Field<int>(table1.TableName+"ID")
                    select new { t1, t2 };
        foreach (var item in query)
        {
            item.t1.Print(false); item.t2.Print();
            DataRow r = NewRow();
            foreach (DataColumn c in item.t1.Table.Columns)
            {
                string name = item.t1.Table.FQColumnName(c);
                if(Columns.Contains(name))
                {
                    r[name] = item.t1[c.ColumnName];
                }
            }
            foreach (DataColumn c in item.t2.Table.Columns)
            {
                string name = item.t2.Table.FQColumnName(c);
                if(Columns.Contains(name))
                {
                    r[name] = item.t2[c.ColumnName];
                }
            }
        }
   }
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
            item.Table.Columns["ID"].ColumnName = "New Name";
            Console.WriteLine($"ID: {item.Field<int>("ID")}, Name: {item.Field<string>("Name")}, Age: {item.Field<int>("Age")}");
        }
    }

}