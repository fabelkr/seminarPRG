using System.Security.Cryptography.X509Certificates;
using Calculator.Classes;

namespace Calculator
{
    public class Logic
    {
        ComposeEq Eq = new ComposeEq();

        public double Calculate(List<(double, int)> numbers, List<(char, int)> operations)
        {
            double result = 0;
            double finalResult;

            // Service validation checkup
            // Console.WriteLine("Numbers: " + string.Join(", ", numbers));
            // Console.WriteLine("Operations: " + string.Join(", ", operations));
            // Console.WriteLine(operations[0].Item1);
            // Console.WriteLine(operations[0].Item2);
            // Console.WriteLine(numbers[operations[0].Item2].Item1);

            //Multiplication and division first
            for (int i = 0; i < operations.Count; i++){
                if (operations[i].Item1 == '*' || operations[i].Item1 == '/'){

                    //Service validation checkup
                    // Console.WriteLine("Multiplication or division");

                    if (operations[i].Item1 == '*'){
                        result = numbers[i].Item1 * numbers[i + 1].Item1;
                    }
                    else{
                        if(numbers[i + 1].Item1 == 0){
                            Console.WriteLine("You can't divide by zero, try again.");
                            SetCalc run = new SetCalc();
                            run.Run();
                        }
                        result = numbers[i].Item1 / numbers[i + 1].Item1;
                    }
                    // Remove the first number and the operation, replace the second number with the current result
                    numbers[i] = (result, numbers[i].Item2);
                    numbers.RemoveAt(i + 1);
                    operations.RemoveAt(i);
                    // Decrement the index, because of the removal of an element from the list
                    i--;
                }
            }

            // Storage for the final result, which now replaces the first number
            finalResult = numbers[0].Item1;

            //Addition and subtraction second
            for (int i = 0; i < operations.Count; i++){
                if (operations[i].Item1 == '+' || operations[i].Item1 == '-'){

                    //Service validation checkup
                    // Console.WriteLine("Addition or subtraction");

                    if (operations[i].Item1 == '+'){
                        finalResult += numbers[i + 1].Item1;
                    }
                    else{
                        finalResult -= numbers[i + 1].Item1;
                    }
                }
            }
            this.ans = finalResult;
            //service validation checkup
            // Console.WriteLine(ans);
            return finalResult;
        }
        public double ans { get; set; }
    }
}