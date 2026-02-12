// See https://aka.ms/new-console-template for more information
using System.Linq.Expressions;

Console.WriteLine("Hello, World!");

Expression<Func<int, int, int>> exp = (a, b) => a + b;
Func<int, int, int> exp2 = (a, b) => a + b;

Console.WriteLine(exp);
Console.WriteLine(exp2);
