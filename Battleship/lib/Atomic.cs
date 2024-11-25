using System;
using System.ComponentModel;

namespace App.lib
{
    //This class is used to handle atomic level operations (very small operations)
    public class Atomic
    {
        //This method is used to display the error message and it saves few lines of code
        public void GameSettingsError()
        {
            Console.WriteLine("\nPress any key to return to main menu");
            Console.ReadKey();
        }
    }
}