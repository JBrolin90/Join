using System.Data;

namespace JoinTables;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Joiners!");

        FewTables tables = new();
        tables.t1.PrintTable();
        tables.t2.PrintTable();

        EditableJoin join = new EditableJoin(tables.joinSet);
        //join.Select(new List<DataColumn> { tables.t1.Columns[0], tables.t1.Columns[1], tables.t2.Columns[0], tables.t2.Columns[1], tables.t2.Columns[2] });
        join.Select(new List<DataColumn> { tables.t1.Columns[0], tables.t1.Columns[1],  tables.t2.Columns[2] });
        join.InnerJoin();
        join.PrintTable();
        Console.WriteLine();

        DataRow r = join.NewRow();
        r[0] = 3;
        r[1] = "t1.C";
        r[2] = 3;
        r[3] = 4;
        r[4] = "t2.C";

        join.InnerJoin();
        join.PrintTable();
        Console.WriteLine();
        join.FillBack();
        tables.t1.PrintTable();
        tables.t2.PrintTable();


        join.Rows.Add(r);
        join.FillBack();
        Console.WriteLine("After add row");
        join.PrintTable();
        tables.t1.PrintTable();
        tables.t2.PrintTable();
        Console.WriteLine();

        join.Rows[0][0] = 5;
        join.Rows[0][2] = 6;
        join.FillBack();
        Console.WriteLine("After update");
        join.PrintTable();
        tables.t1.PrintTable();
        tables.t2.PrintTable();
        Console.WriteLine();

        join.Rows[0].Delete();
        join.FillBack();
        Console.WriteLine("After delete");
        join.PrintTable();
        tables.t1.PrintTable();
        tables.t2.PrintTable();
        Console.WriteLine();


    }
}
   


