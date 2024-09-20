public class CanContinue
{
    public static void canContinueStaticInt(int iterationCount)
    {
        int iteration = 0;
        bool canContinue = true;
        while (canContinue)
        {
            Console.WriteLine("muzu pokracovat " + iteration);
            iteration++;
            if (iteration >= iterationCount)
            {
                canContinue = false;
            }
        }
        Console.WriteLine();
    }
    public static void canContinueDynamicInt()
    {   
        bool continueRunning = true;

        do
        {
            Console.WriteLine("Please enter a command:");
            Console.WriteLine("0: Exit");
            Console.WriteLine("1: Run with 10 iterations");
            Console.WriteLine("2: Run with custom iterations");

            switch (Console.ReadLine())
            {
                case "0":
                    Console.WriteLine("Goodbye\n");
                    continueRunning = false;
                    break;
                case "1":
                    canContinueStaticInt(10);
                    break;
                case "2":
                    Console.WriteLine("How many times do you want to iterate?\n");
                    int iterationCount = Convert.ToInt32(Console.ReadLine());
                    canContinueStaticInt(iterationCount);
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        } while (continueRunning);
    }
}