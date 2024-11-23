using System;
using System.ComponentModel;

namespace App.lib
{
    public class GameSettings
    {
        //All nescessary variables declaration
        bool? mapType = null;
        int difficulty;
        string colorTheme;

        string[] shipNames = [
            "Carrier",
            "Battleship",
            "Cruiser",
            "Destroyer",
            "Submarine",
            "Yamato",
            "Iowa",
            "Boomin Beaver"
        ];

        int[,] shipSizes = {
            {1, 5},
            {1, 4},
            {1, 3},
            {1, 3},
            {1, 2},
            {2, 4},
            {1, 6},
            {1, 1}
        };

        Dictionary<string, int[]> shipSpecifications;

        //Constructor with ship specifications dictionary in it 
        public GameSettings()
        {
            shipSpecifications = new Dictionary<string, int[]>();

            //Adding ships to the dictionary (excluding Yamato, Iowa and Boomin Beaver)
            for (int i = 0; i < 5; i++)
            {
                shipSpecifications.Add(shipNames[i], new int[] { shipSizes[i, 0], shipSizes[i, 1] });
            }
        }
        public void SetNewGame(){
            //Dev test

            Console.WriteLine("Set new game before you start playing\n");
            //Basic settings UI
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Optional = white");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Obligatory = red");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Already set = green");

            Console.ResetColor();
            Console.WriteLine();

            //Map UI options
            if (mapType == true){
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("1. 10 X 10 square");
            }
            else if (mapType == false){
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("1. 20 diameter circle");
            }
            else{
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("1. Map selection");
            }

            //Difficulty UI options
            if(difficulty == 1){
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("2. Normal");
            }
            else if(difficulty == 2){
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("2. Easy");
            }
            else if(difficulty == 3){
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("2. Demon");
            }
            else{
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("2. Difficulty selection ");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("3. Add or modificate your ships");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("4. Select color theme");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("5. Start game");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("6. Exit game");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("7. Help ");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("8. Get ship info");

            Console.ResetColor();
            switch(Console.ReadLine()){
                case "1":
                    Console.Write("\n1. 10 X 10 square \n2. 20 diameter circle");
                        switch(Console.ReadLine()){
                            case "1":
                                mapType = true;
                                break;
                            case "2":
                                mapType = false;
                                break;
                            default:
                                Console.WriteLine("\nYou selected an invalid option");
                                break;
                        }
                    break;
                case "2":
                    Console.WriteLine("\n1. rational thinking PC \n2. random thinking PC \n3. hacking PC");
                        switch(Console.ReadLine()){
                            case "1":
                                difficulty = 1;
                                break;
                            case "2":
                                difficulty = 2;
                                break;  
                            case "3":
                                difficulty = 3;
                                break;
                            default:
                                Console.WriteLine("\nYou selected an invalid option");
                                break;
                        }
                    break;
                case "3":
                    Console.WriteLine("\n1. Add ship \n2. Modificate ship");
                        switch(Console.ReadLine()){
                            case "1":
                                Console.WriteLine("\nIf you add ships, they will be added to both teams and added ships cannot be modificated.\n");
                                Console.WriteLine("\n1. Yamato (2 X 4) \n2. Iowa (1 X 6) \n3. Boomin Beaver (1 X 1)");
                                switch(Console.ReadLine()){
                                    case "1":
                                        shipSpecifications.Add(shipNames[5], new int[] {shipSizes[5, 0], shipSizes[5, 1]});
                                        break;
                                    case "2":
                                        shipSpecifications.Add(shipNames[6], new int[] {shipSizes[6, 0], shipSizes[6, 1]});
                                        break;
                                    case "3":
                                        shipSpecifications.Add(shipNames[7], new int[] {shipSizes[7, 0], shipSizes[7, 1]});
                                        break;
                                    default:
                                        Console.WriteLine("\nYou selected an invalid option");
                                        break;
                                    // TODO:Back to menu
                                }
                                break;
                            case "2":
                                Console.WriteLine("\n1. Carrier \n2. Battleship \n3. Cruiser \n4. Destroyer \n5. Submarine");
                                //TODO: Doplnit funkci pro modifikaci lodí
                                break;
                            default:
                                Console.WriteLine("\nYou selected an invalid option");
                                break;
                        }
                    break;
                case "4":
                    Console.WriteLine("\n1. Pink \n2. Blue \n3. Green \n4. Yellow \n5. Red");
                        switch(Console.ReadLine()){
                            case "1":
                                colorTheme = "Pink";
                                break;
                            case "2":
                                colorTheme = "Blue";
                                break;
                            case "3":
                                colorTheme = "Green";
                                break;
                            case "4":
                                colorTheme = "Yellow";
                                break;
                            case "5":
                                colorTheme = "Red";
                                break;
                            default:
                                Console.WriteLine("\nYou selected an invalid option");
                                break;
                        }
                    break;
                case "5":
                    Console.WriteLine("Start the game");
                    //TODO: Start game add class and call its function + you can not start the game if obligatory items unset
                    break;
                case "6":
                    Environment.Exit(0);
                    break;
                case "7":
                    //TODO: Set help
                    Console.WriteLine();
                    break;
                case "8":
                    foreach (KeyValuePair<string, int[]> ship in shipSpecifications)
                    {
                        Console.WriteLine($"Name = {ship.Key}, Size = [{string.Join(", ", ship.Value)}]");
                    }
                    // TODO:Back to menu
                    break;
                default:
                    Console.WriteLine("You selected an invalid option");
                    break;
            
            }
        }
    }
}