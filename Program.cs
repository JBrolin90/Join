using System.Data;

namespace JoinTables;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, Joiners!");

        JoinTest test = new();
        //test.TestModify();
        //test.TestAdd();
        test.TestDelete();

    }
}
   


