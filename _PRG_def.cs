using System;
using System.Runtime.CompilerServices;

namespace MyConsoleApp
{
    public class Program
{
    private static Random rng = new Random();

    static void Main(string[] args)
    {
        CanContinue canContinueInstance = new CanContinue();
        // for (int i = 0; i < 10; i++)
        // {
        //     Console.WriteLine(rng.Next(0, 100));
        // }
        
        // {
        //     Console.WriteLine(rng.Next(0, 100));
        //     iteration++;
        // }

        int a = rng.Next(0, 100);

        isGreaterThanX(50);
        // canContinueStaticInt(10);
        CanContinue.canContinueDynamicInt();
    }

    public static void isGreaterThanX(int x)
    {
        int a = -1;
    
        if (a == -1)
        {
            a = rng.Next(0, 100);
        }
    
        if (a > x)
        {
            Console.WriteLine(a + " is greater than " + x);
        }
        else
        {
            Console.WriteLine(a + " is less than or equal to " + x);
        }
    }

    
    // public static void canContinueStaticInt(int iterationCount)
    // {
    //     int iteration = 0;
    //     bool canContinue = true;
    //     while (canContinue)
    //     {
    //         Console.WriteLine("muzu pokracovat " + iteration);
    //         iteration++;
    //         if (iteration >= iterationCount)
    //         {
    //             canContinue = false;
    //         }
    //     }
    // }
    // public static void canContinueDynamicInt()
    // {   
    //     bool continueRunning = true;

    //     do
    //     {
    //         Console.WriteLine("Please enter a command:");
    //         Console.WriteLine("0: Exit");
    //         Console.WriteLine("1: Run with 10 iterations");
    //         Console.WriteLine("2: Run with custom iterations");

    //         switch (Console.ReadLine())
    //         {
    //             case "0":
    //                 Console.WriteLine("Goodbye");
    //                 continueRunning = false;
    //                 break;
    //             case "1":
    //                 canContinueStaticInt(10);
    //                 break;
    //             case "2":
    //                 Console.WriteLine("How many times do you want to iterate?");
    //                 int iterationCount = Convert.ToInt32(Console.ReadLine());
    //                 canContinueStaticInt(iterationCount);
    //                 break;
    //             default:
    //                 Console.WriteLine("Invalid input");
    //                 break;
    //         }
    //     } while (continueRunning);
    // }
}
}