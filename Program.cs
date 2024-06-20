using System.Data;

namespace JoinTables;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        FewTables tables = new();
        EditableJoin join = new EditableJoin(tables.joinSet);
        Console.WriteLine("After init");
        join.PrintTable();
        tables.t1.PrintTable();
        tables.t2.PrintTable();
        Console.WriteLine();
        DataRow r = join.NewRow();
        r[0] = 3;
        r[1] = "t1.C";
        r[2] = 3;
        r[3] = 4;
        r[4] = "t2.C";

        join._InnerJoin(tables.t1, tables.t2);


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
   


