namespace JoinTables;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        FewTables tables = new();
        Join join = new Join(tables.joinSet);
        join.FillResult();
        join.PrintTable();
    }
}
   


