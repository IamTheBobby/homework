using System;

public class Prime
{
    public static void Main(string[] args)
    {
        int b = 0;

        do
        {
            Console.Write("Please input a number(>1):");
            string a = Console.ReadLine();
            b = int.Parse(a);
        } while (b <= 1);
        

        Console.Write(" \n****您输入的数"+b+"的素数因子为:");
        int primeFactor = 2;
        while(primeFactor <= b)
        {
            while (b % primeFactor == 0)
            {
                Console.Write(primeFactor + " ");
                b /= primeFactor;
            }
            primeFactor += 1;
        }

        Console.WriteLine("****");
        Console.WriteLine();
    }
}
