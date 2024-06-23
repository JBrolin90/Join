using System.Data;
namespace JoinTables;

class MyList: List<int>
{}

public class JoinTest
{
    public DataTable t1 = new ();
    public DataTable t2 = new ();
    public DataSet joinSet = new DataSet();
    public JoinTest()
    {
        Setup();
    }

    public void Setup()
    {
        t1.TableName = "t1";
        t1.Columns.Add("ID", typeof(int));
        t1.Columns.Add("Name", typeof(string));
        t1.PrimaryKey = new DataColumn[] { t1.Columns[0] };

        t2.TableName = "t2";
        t2.Columns.Add("ID", typeof(int));
        t2.Columns.Add("t1ID", typeof(int));
        t2.Columns.Add("Name", typeof(string));
        t2.PrimaryKey = new DataColumn[] { t2.Columns[0] };

        t1.Rows.Add(1, "A");
        t1.Rows.Add(2, "B");

        t2.Rows.Add(1, 1, "A1");
        t2.Rows.Add(2, 2, "B2");
        t2.Rows.Add(3, 1, "A3");
        t2.Rows.Add(4, 1, "A4");
        joinSet.Tables.Add(t1);
        joinSet.Tables.Add(t2);
    }
    public void TestModify()
    {
        Console.WriteLine("Hello, Joiners!, we'll test modifying data");

        t1.PrintTable();
        t2.PrintTable();

        EditableJoin join = new EditableJoin(joinSet);
        join.SelectAll();
        join.InnerJoin();
        join.PrintTable();
        Console.WriteLine();

        Console.WriteLine("Updating data");

        join.Rows[3][1] = "AA";
        join.Rows[2][3] = 2; join.Rows[2][4] = "BB";
        join.FillBack();
        Console.WriteLine("After update");
        join.PrintTable();
        t1.PrintTable();
        t2.PrintTable();
        Console.WriteLine();
    }

    public void TestAdd()
    {
        Console.WriteLine("Hello, Joiners!, we'll test adding data");

        t1.PrintTable();
        t2.PrintTable();

        EditableJoin join = new EditableJoin(joinSet);
        join.SelectAll();
        join.InnerJoin();
        join.PrintTable();
        Console.WriteLine();

        DataRow r = join.NewRow();
        r[0] = 3;
        r[1] = "t1.C";
        r[2] = 5;
        r[3] = 3;
        r[4] = "t2.C";
        join.Rows.Add(r);

        r = join.NewRow();
        r[0] = 4;
        r[1] = "t1.D";
        join.Rows.Add(r);

        join.PrintTable();
        Console.WriteLine();

        join.FillBack();
        t1.PrintTable();
        t2.PrintTable();
    }
    public void TestDelete()
    {
        Console.WriteLine("Hello, Joiners!, we'll test deleting data");

        t1.PrintTable();
        t2.PrintTable();

        EditableJoin join = new EditableJoin(joinSet);
        join.SelectAll();
        join.InnerJoin();
        join.PrintTable();
        Console.WriteLine();

        join.Rows[0].Delete();
        join.FillBack();
        Console.WriteLine("After delete");
        join.PrintTable();
        t1.PrintTable();
        t2.PrintTable();
        Console.WriteLine();
    }   
}


