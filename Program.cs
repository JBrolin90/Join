// See https://aka.ms/new-console-template for more information
using System.Data;

Console.WriteLine("Hello, World!");

FewTables tables = new();
Join join = new(tables.joinSet);
join.BuildResultTable();
join.FillResult();
join.PrintResult();
