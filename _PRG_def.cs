using System;

namespace MyConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rng = new Random();
            int a = rng.Next(0, 100);

            if(a > 50){
                Console.WriteLine(a + " is greater than 50");
            } else {
                Console.WriteLine(a + " a is less than or equal to 50");
            }

            Console.WriteLine("Hello World!");
        }
    }
}