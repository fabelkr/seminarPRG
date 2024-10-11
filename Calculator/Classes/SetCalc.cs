using System;
using System.Xml.Serialization;

namespace Calculator.Classes
{
    public class SetCalc
    {

            //Set class instance
            Logic Logic = new Logic();
            ComposeEq ComposeEq = new ComposeEq();

            Convertor conv = new Convertor();
        public void Run()
        {

            //setting UI
            Console.WriteLine("0. Exit\n1. Lets fucking count \n2. Lets convert numbers");

            switch(Console.ReadLine())
            {
                case "0":
                    Environment.Exit(0);
                    break;
                case "1":
                    Console.WriteLine("Enter the math problem");
                    string input = Console.ReadLine() ?? string.Empty;
                    if(input == string.Empty){
                        Console.WriteLine("Invalid value, try again");
                        Run();
                    }
                    input = input.Replace(" ", string.Empty);
                    input = input.Replace("\t", string.Empty);
                    //service validation checkup
                    // Console.WriteLine(input);
                    ComposeEq.EquationCompsoser(input, Logic);
                    Run();
                    break;
                case "2":
                    Console.WriteLine("\nChoose an operation:");
                    Console.WriteLine("1. Decimal to Binary");
                    Console.WriteLine("2. Binary to Decimal");
                    Console.WriteLine("3. Decimal to Hexadecimal");
                    Console.WriteLine("4. Hexadecimal to Decimal");
                    Console.WriteLine("5. Binary to Hexadecimal");
                    Console.WriteLine("6. Hexadecimal to Binary");
                    Console.WriteLine("7. Exit");
                    int convertChoice = Convert.ToInt32(Console.ReadLine());

                switch (convertChoice)
                {
                    case 1:
                        Console.WriteLine("Enter Decimal number\n");
                        int decimall = Convert.ToInt32(Console.ReadLine());
                        conv.decimalToBinary(decimall);
                        break;
                    
                    case 2:
                        Console.WriteLine("Enter Binary number\n");
                        string binary = Console.ReadLine() ?? string.Empty;
                        if (binary == string.Empty)
                        {
                            Console.WriteLine("Invalid input, try again.");
                            Run();
                        }
                        conv.binaryToDecimal(binary);
                        break;

                    case 3:
                        Console.WriteLine("Enter Decimal number\n");
                        int decimax = Convert.ToInt32(Console.ReadLine());
                        conv.decimalToHexadecimal(decimax);
                        break;

                    case 4:
                        Console.WriteLine("Enter Hexadecimal number\n");
                        string hexadec = Console.ReadLine() ?? string.Empty;
                        if (hexadec == string.Empty)
                        {
                            Console.WriteLine("Invalid input, try again.");
                            Run();
                        }
                        conv.hexadecimalToDecimal(hexadec);
                        break;

                    case 5:
                        Console.WriteLine("Enter Binary number\n");
                        string binhex = Console.ReadLine() ?? string.Empty;
                        if (binhex == string.Empty)
                        {
                            Console.WriteLine("Invalid input, try again.");
                            Run();
                        }
                        conv.binaryToHexadecimal(binhex);
                        break;

                    case 6:
                        Console.WriteLine("Enter Hexadecimal number\n");
                        string hexabin = Console.ReadLine() ?? string.Empty;
                        if (hexabin == string.Empty)
                        {
                            Console.WriteLine("Invalid input, try again.");
                            Run();
                        }
                        conv.hexadecimalToBinary(hexabin);
                        break;

                    case 7:
                        Environment.Exit(0);
                        break;
                    
                    default:
                        Console.WriteLine("Invalid choice\n");
                        break;
                    }
                    break;
                default:
                Console.WriteLine("Invalid input, try again.");
                Run();
                break;
            }
            Run();
        }
    }
}