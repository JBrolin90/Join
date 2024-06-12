using System.Data;
namespace JoinTables;
public class FewTables
{
    public DataTable t1 = new ();
    public DataTable t2 = new ();
    public DataSet joinSet = new DataSet();
    public FewTables()
    {
        Setup();
    }

    public void Setup()
    {
        t1.TableName = "t1";
        t1.Columns.Add("ID", typeof(int));
        t1.Columns.Add("Name", typeof(string));

        t2.TableName = "t2";
        t2.Columns.Add("ID", typeof(int));
        t2.Columns.Add("t1ID", typeof(int));
        t2.Columns.Add("Name", typeof(string));

        t1.Rows.Add(1, "A");
        t1.Rows.Add(2, "B");

        t2.Rows.Add(1, 1, "A1");
        t2.Rows.Add(2, 2, "A2");
        joinSet.Tables.Add(t1);
        joinSet.Tables.Add(t2);



    }
}


