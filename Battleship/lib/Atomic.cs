using System;
using System.ComponentModel;

namespace App.lib
{
    //This class is used to handle atomic level operations (very small operations)
    public class Atomic
    {
        //This method is used to display the error message and it saves few lines of code
        public static void GameSettingsError()
        {
            Console.WriteLine("\nPress any key to return to main menu");
            Console.ReadKey();
            Console.Clear();
        }

        public static System.ConsoleColor MakeChessboard(int counter, int i)
        {
            if (counter % 2 == 1 && i % 2 == 1)
            {
                return ConsoleColor.Blue;
            }
            else if (counter % 2 == 0 && i % 2 == 0)
            {
                return ConsoleColor.Blue;
            }
            else {
                return ConsoleColor.White;
            }
        }

        // public static void RenderZpevRyb(){
        //     for(int i = 0; i < 12; i++){
        //         for(int j = 0; j < 5; j++){
        //             if(i % 2 == 1){
        //                 Console.WriteLine(" - ");
        //             }
        //             else {
        //                 Console.WriteLine(" U ");
        //             }
        //         }
        //     }
        // }
    }
}