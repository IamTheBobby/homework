using System;

public class Array
{
    public static void Main(string[] args)
    {
        int n = 0;

        do
        {
            Console.Write("请输入数组的长度(>0):");
            string a = Console.ReadLine();
            n = int.Parse(a);
        } while (n < 1);

        int[] array1 = new int[n];

        for(int i = 0; i < n; i++)
        {
            Console.Write("请输入数组中第" + (i + 1) + "个值:");
            string b = Console.ReadLine();
            array1[i] = int.Parse(b);
        }

        int max = array1[0];
        int min = array1[0];
        int sum = 0;
        double average = 0;
       
        for(int i = 0; i < n; i++)
        {
            if (max <= array1[i])
                max = array1[i];

            if (min >= array1[i])
                min = array1[i];

            sum += array1[i];
        }

        average = sum / n;

        Console.WriteLine("The max=" + max + " The min=" + min + " The sum=" + sum + " The average=" + average);        
    }
}
