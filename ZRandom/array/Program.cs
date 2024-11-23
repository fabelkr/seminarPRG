using System;

namespace MyNamespace
{
    class Program
    {
        static void Main(string[] args)
        {
            float[] intArray = { 4, 9, 5, 6, 8 };

            foreach (int i in intArray)
            {
                Console.WriteLine(i);
            }

            // for (int i = 0; i < intArray.Length; i++)
            // {
            //     Console.WriteLine(intArray[i]);
            // }

            float sum = 0;

            for (int i = 0; i < intArray.Length; i++)
            {
                sum += intArray[i];
            }

            float average = sum / intArray.Length;

            float min = intArray[0];
            float max = intArray[0];

            for (int i = 0; i < intArray.Length; i++)
            {
                if (intArray[i] < min)
                {
                    min = intArray[i];
                }

                if (intArray[i] > max)
                {
                    max = intArray[i];
                }
            }


            Console.WriteLine( "suma je:" + sum);
            Console.WriteLine("prumer je:" + average);
            Console.WriteLine("min je:" + min);
            Console.WriteLine("max je:" + max);

            Console.WriteLine("zadej hledaný prvek");

            float search = float.Parse(Console.ReadLine());

            for (int i = 0; i < intArray.Length; i++)
            {
                if (intArray[i] == search)
                {
                    Console.WriteLine("prvek je na indexu:" + i);
                    break;
                }
            }

            Random random = new Random();

            float[] extendedArray = new float[100];

            for (int i = 0; i < extendedArray.Length; i++)
            {
                extendedArray[i] = random.Next(0, 10);
            }

            int[] count = new int[10];

            foreach (float i in extendedArray)
            {
                count[(int)i]++;
            }

            for (int i = 0; i < count.Length; i++)
            {
                Console.WriteLine($"počet{i} = {count[i]}");
            }
        }
    }
}
