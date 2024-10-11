using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;


namespace Calculator.Classes
{
    public class ComposeEq
    {
        double subResult = 0;

        // public string nefunkcniKunda;

        private List<(double, int)> ParseNumbers(string input, Logic logic)
        {
            List<(double, int)> numbers = new List<(double, int)>();
            string numberStr = string.Empty;
            
            for (int i = 0; i < input.Length; i++)
            {

                if (char.IsDigit(input[i]) || input[i] == '.' || (input[i] == '-' && (i == 0 || "+-*/()".Contains(input[i - 1]))))
                {
                    if (input[i] == '-')
                    {
                        numberStr += input[i];
                        i++;
                    }
                    while (i < input.Length && (char.IsDigit(input[i]) || input[i] == '.'))
                    {
                        numberStr += input[i];
                        i++;
                    }
                    i--;
                }else if(char.ToLower(input[i]) == 'a' && char.ToLower(input[i + 1]) == 'n' && char.ToLower(input[i + 2]) == 's')
                {
                    numbers.Add((logic.ans, i));
                    i += 2;
                }else if(char.ToLower(input[i]) == 'p' && char.ToLower(input[i + 1]) == 'i')
                {
                    numbers.Add((Math.PI, i));
                    i++;
                }
                else if (!string.IsNullOrEmpty(numberStr))
                {
                    if (double.TryParse(numberStr, out double number))
                    {
                        numbers.Add((number, i - numberStr.Length));
                    }
                    numberStr = string.Empty;
                }
            }
            if (!string.IsNullOrEmpty(numberStr) && double.TryParse(numberStr, out double lastNumber))
            {
                numbers.Add((lastNumber, input.Length - numberStr.Length));
            }
            return numbers;
        }
        private List<(char, int)> ParseOperations(string input)
        {
            List<(char, int)> operations = new List<(char, int)>();
            for (int i = 0; i < input.Length; i++)
            {
                if ("+-*/()".Contains(input[i]))
                {

                    //condition to ensure, that the minus sign is considered as a subtraction and not a negative number
                    if (!(input[i] == '-' && (i == 0 || "+-*/(".Contains(input[i - 1]))))
                    {
                        operations.Add((input[i], i));
                    }
                }
            }
            return operations;
        }
        public void EquationCompsoser(string input, Logic logic){
            // int n = 0;

            //Lists for separating numbers and operations + works as tuples for counting index
            List<(char, int)> operations = ParseOperations(input);
            List<(double, int)> numbers = ParseNumbers(input, logic);

            //Null check
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "Input cannot be null");
            }

            //Input to char array
            // char[] equation = input.ToCharArray();

            // nefunkcniKunda = input;

            //TODO: This is the old way of parsing the input, which I optimised by the ParseNumbers and ParseOperations methods

            // List<(double, int)> numbers = new List<(double, int)>();
            // List<(char, int)> operations = new List<(char, int)>();

            //Input checker - sorts numbers and operations into correct lists
            // for (int i = 0; i < equation.Length; i++)
            // {
            //     //Checks if the character is a number - if there is a dot, the character is still considered a number (decimal numbers)
            //     //Also checks if the character is a negative sign, but only if it is the first character or if it is after a mathematical operator (otherwise it is considered a subtraction)
            //     if (char.IsDigit(equation[i]) || equation[i] == '.' || (equation[i] == '-' && (i == 0 || "+-*/(".Contains(equation[i - 1]))))
            //     {
            //         string numberStr = string.Empty;

            //         //Includes the negative sign in the same tuple into Item1 with the number
            //         if (equation[i] == '-')
            //         {
            //             numberStr += equation[i];
            //             i++;
            //         }

            //         while (i < equation.Length && (char.IsDigit(equation[i]) || equation[i] == '.'))
            //         {
            //             numberStr += equation[i];
            //             i++;
            //         }
            //         i--;
            //         //Passes the number with its index to the list
            //         if (double.TryParse(numberStr, out double number))
            //         {
            //             numbers.Add((number, n));
            //         }
            //     }
            //     //Checks if the character is an mathematial operator and passes it to the operators list
            //     else if ("+-*/()".Contains(equation[i]))
            //     {
            //         operations.Add((equation[i], n));
            //     }else if (new String(equation[i..(i+3)]) == "ans")
            //     {
            //         numbers.Add((logic.ans, n));
            //         i += 2;
            //     }
            //     else
            //     {
            //         throw new ArgumentException("Invalid input, please enter a valid mathematical equation");
            //     }
            //     n++;
            // }
            ParseNumbers(input, logic);
            ParseOperations(input);

            int open = -1;
            
            

            for(int i = 0; i < operations.Count; i++){
                //If the operation[i] is an opening parentheses
                if(operations[i].Item1 == '('){
                    //it sets the open variable to the index of the opening parentheses
                    open = operations[i].Item2;
                //If the operation[i] is a closing parentheses
                }else if(operations[i].Item1 == ')'){
                    // it sets the close variable to the index of the closing parentheses
                    int close = operations[i].Item2;
                    //Calculates the length of the substring between the opening and closing parentheses
                    //(There is +1, because the closing parentheses is not included in the substring)
                    int length = close - (open + 1);
                    //Creates a substring (string that is created from a part of an original string) from the input string between the opening and closing parentheses
                    string subEq = input.Substring(open + 1, length);
                    Console.WriteLine("\nSubEq: " + subEq);
                    //Calculates the result of the substring
                    subResult = logic.Calculate(ParseNumbers(subEq, logic), ParseOperations(subEq));
                    Console.WriteLine("SubResult: " + subResult);
                    //Updates the input string with the result of the substring
                    input = input.Substring(0, open) + subResult.ToString() + input.Substring(close + 1);
                    Console.WriteLine("Updated Input: " + input + "\n");
                    // Restarts the loop to process the updated input so we could find the next parentheses or the final result
                    operations = ParseOperations(input);
                    numbers = ParseNumbers(input, logic);
                    i = -1;
                }
            }
            double realResult = logic.Calculate(numbers, operations);
            if(operations.All(x => x.Item1 != '(')){
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("final result: " + realResult + "\n");
                Console.ResetColor();
            }
        }
        public void PrintResult(Logic logic)
        {
            Console.WriteLine("Result " + logic.ans);
        }
    }
}