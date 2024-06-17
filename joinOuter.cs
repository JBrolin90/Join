using System.Data;
namespace JoinTables;
public partial class Join :DataTable
{
    // Method that uses Linq to join table
    public void JoinOuter(DataTable table1, DataTable table2 )
    {
        var query = from t1 in table1.AsEnumerable()
                    join t2 in table2.AsEnumerable()
                    on t1.Field<int>("ID") equals t2.Field<int>("ID") into temp
        foreach (var item in query)
        {
            Console.WriteLine($"ID: {item.ID}, Name: {item.Name}, Age: {item.Age}, Address: {item.Address}");
        }
   }
    public void XJoinOuter(DataTable table1, DataTable table2 )
    {
        var query = from t1 in table1.AsEnumerable()
                    join t2 in table2.AsEnumerable()
                    on t1.Field<int>("ID") equals t2.Field<int>("ID") into temp
                    from t2 in temp.DefaultIfEmpty()
                    select new
                    {
                        ID = t1.Field<int>("ID"),
                        Name = t1.Field<string>("Name"),
                        Age = t1.Field<int>("Age"),
                        Address = t2 == null ? "No Address" : t2.Field<string>("Address")
                    };
        foreach (var item in query)
        {
            Console.WriteLine($"ID: {item.ID}, Name: {item.Name}, Age: {item.Age}, Address: {item.Address}");
        }
   }

}