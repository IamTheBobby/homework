using System;

class Welcome
{
    public static void Main()
    {
        string a = "";
        string b = "";
        double x = 0;
        double y = 0;
        Console.Write("Please input a number:");
        a = Console.ReadLine();
        Console.Write("Now,please input another number:");
        b = Console.ReadLine();
        x = double.Parse(a);
        y = double.Parse(b);
        Console.WriteLine("The produce of two number you input is:" + (x * y));
    }
}