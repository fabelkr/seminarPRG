using System;
using Calculator.Classes;

namespace Calculator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the calculator app.\n");
            SetCalc calc = new SetCalc();
            // Logic logic = new Logic();

            calc.Run();
        }
    }
}
           
            
             
            