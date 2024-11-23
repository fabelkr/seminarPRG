using System;
using List_dict.Classes;

namespace List_dict
{
    class Program
    {
        static Atomic atom = new Atomic();
        static void Main(string[] args)
        {
            List<string> nameList = new List<string> {"Karel", "Jozef", "Petr", "Jan", "Jana", "Marie"};

            atom.PrintList(nameList);

            Console.WriteLine("\nRemoving Karel from the list\n");

            nameList.Remove("Karel");

            atom.PrintList(nameList);

            nameList.RemoveAt(3);

            Console.WriteLine("\nRemoving the 4th element from the list\n");

            atom.PrintList(nameList);

            Dictionary<string, string> foodDict = new Dictionary<string, string> {{"Karel", "Buchtičky se šodó"}, {"Jozef", "Salám"}, {"Petr", "Kebab"}, {"Jan", "Řízek"}, {"Jana", "Jitrničky"}, {"Marie", "Hovna s mákem"}};

            foodDict.Add("Pavla", "Knedlík s uzeným");

            foreach (KeyValuePair<string, string> food in foodDict)
            {
                Console.WriteLine($"Oblibene jidlo stutenta {food.Key} je {food.Value}");
            }
        }
    }
}
