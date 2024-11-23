using System;

namespace List_dict.Classes{
    class Atomic{
        public void PrintList(List<string> nameList){
            foreach (string name in nameList)
            {
                Console.WriteLine(name);
            }
        }
    }
}