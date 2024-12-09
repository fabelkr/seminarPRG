using System;
using System.ComponentModel;
using Microsoft.VisualBasic;

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

        public static void StartGameMessage()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Game started!");
            Console.ResetColor();
            Console.WriteLine("You can switch between yours and the enemy's map by typing 'map' while it is your turn.");
            Console.Write("On the map,");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("'X'");
            Console.ResetColor();
            Console.Write("indicates a hit,");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("'O'");
            Console.ResetColor();
            Console.Write("indicates a miss, and '~' indicates area, where you haven't shot yet.\n");
            Console.WriteLine("Select the weapon you want to use and fire the first shot:");
            Console.WriteLine("You can look at the weapon specifications any time by typing 'help' and then heading to the 'Weapons' section.");
        }

        public static string MapViewAnouncement(bool mapView)
        {   
            if(mapView == true){
                Console.ForegroundColor = ConsoleColor.Green;
                return "\nYou are currently viewing your map.\n";
            }
            else {
                Console.ForegroundColor = ConsoleColor.Red;
                return "\nYou are currently viewing the enemy's map.\n";
            }
        }
    }
}