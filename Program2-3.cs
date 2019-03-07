using System;
public class Prime100
{
    public static void Main(string[] args)
    {
        Console.WriteLine("埃氏筛法求2~100以内的素数:");

        int[] array1 = new int[99];

        for(int i = 0; i < 99; i++)
        {
            array1[i] = i + 2;
        }

        int k = 0;

        for (int i = 2; i <= 100; i += 2)
        {
            array1[i - 2] = 0;
        }

        array1[0] = 2;

        Console.WriteLine("\n去掉2的倍数后的结果如下:");

        for (int i = 0; i < 99; i++)
        {
            if (array1[i] != 0)
            {
                Console.Write(array1[i] + " ");

                k++;
                if (k % 10 == 0)
                    Console.WriteLine();
            }
        }

        k = 0;

        bool boolTest = true;

        for (int test = 3; test <= 10; test++)
        {
            for(int i = 2; i < test; i++)
            {
                if (test % 2 == 0)
                    boolTest = false;
            }

            if(boolTest)
            {
                for (int i = test; i <= 100; i += test)
                {
                    array1[i - 2] = 0;
                }

                array1[test - 2] = test;

                Console.WriteLine("\n去掉" + test + "的倍数后的结果如下:");

                for (int i = 0; i < 99; i++)
                {
                    if (array1[i] != 0)
                    {
                        Console.Write(array1[i] + " ");

                        k++;
                        if (k % 10 == 0)
                            Console.WriteLine();
                    }
                }

                k = 0;
            }

            boolTest = true;
        }

        Console.WriteLine("\n最后的结果为:");
        k = 0;
        for (int i = 0; i < 99; i++)
        {
            if (array1[i] != 0)
            {
                Console.Write(array1[i] + " ");

                k++;
                if (k % 10 == 0)
                    Console.WriteLine();
            }
        }
        Console.WriteLine();
    }
}
