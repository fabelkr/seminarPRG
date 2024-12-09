using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using App.lib.Computer;
using App.lib.RenderASCII;

namespace App.lib
{
    public class GameConstructor
    {
        private Dictionary<string, bool> shipState;
        private Dictionary<string, bool> shipOrientation;

        //null = water, false = ship, true = hit
        public bool?[,] map;
        private bool mapMasking = false;


        private bool mapView = false;

        private GameSettings settings;
        string currentAddedShip;

            
        //Dictionary for storing data if there are any coordinates set for the ship
        Dictionary<string, bool> CoordinateIsSet = new Dictionary<string, bool>();

            SetCPU CPU;
        //Constuctor
        public GameConstructor(GameSettings settings)
        {
            this.settings = settings;
            //initialize the shipState dictionary
            CPU = new SetCPU(settings, this);
            shipState = new Dictionary<string, bool>();
            //Sets the state of each in game included ship to false by default (Not hit)
            foreach (KeyValuePair<string, int[]> ship in settings.shipSpecifications)
            {
                shipState[ship.Key] = false;
            }
            //TEST
            // foreach (KeyValuePair<string, bool> state in shipState)
            // {
            //     Console.WriteLine($"ship: {state.Key}, state: {state.Value}");
            // }
        }

        public void CreateNewGame()
        {
            //TEST
            // Console.WriteLine(settings.mapType);
            // Console.WriteLine(settings.mapHeight);
            // Console.WriteLine(settings.mapWidth);
            GameLogic logic = new GameLogic(settings);

            Console.Clear();
            CreateMap();

            //Fill the dictionary with the ship names and set the value to false by deafult (ship is not placed on the map)
            foreach(KeyValuePair<string, int[]> ship in settings.shipSpecifications)
            {
                CoordinateIsSet[ship.Key] = false;
            }

            //TEST
            // int cooind = 1;
            // foreach(KeyValuePair<string, bool> coordinates in CoordinateIsSet)
            // {
                
            //     Console.WriteLine($"{cooind}. Name = {coordinates.Key}, Size = [{string.Join(", ", coordinates.Value)}]");
            //     cooind++;
            //     Console.ResetColor();
            // }
            Dictionary<int, string> indexOfSelectedShip = new Dictionary<int, string>();
            //The loop will run until all of the ships are placed on the map

            for(int i = 1; i <= settings.shipSpecifications.Count; i++)
            {
                indexOfSelectedShip[i] = settings.shipSpecifications.ElementAt(i - 1).Key;
            }
            while (CoordinateIsSet.ContainsValue(false)){
                Console.Clear();
                PrintMap(ref map);
                int shipsCounter = 1;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Select the ship you want to place on the map: ");
                Console.WriteLine("You have to place all ships on the map to start the game.");
                Console.ForegroundColor = settings.colorTheme ?? ConsoleColor.White;
                //TEST
                // indexOfSelectedShip.Clear();
                // foreach(KeyValuePair<int, string> index in indexOfSelectedShip)
                // {
                //     Console.WriteLine($"Name = {index.Key}, Size = {index.Value}");
                // }
                for (int i = 0; i < settings.shipSpecifications.Count; i++)
                {
                    var ship = settings.shipSpecifications.ElementAt(i);
                    if (settings.shipNames.Contains(ship.Key) && CoordinateIsSet[ship.Key] == false)
                    {
                        Console.WriteLine(i + 1 + ". " + ship.Key + " (" + ship.Value[0] + " X " + ship.Value[1] + ")");
                        Ships.CheckShipToRender(ship.Key); // Pass the specific ship name to render the desired ASCII art
                        shipsCounter++;
                        //TEST
                        // foreach(KeyValuePair<int, string> index in indexOfSelectedShip)
                        // {
                        //     Console.WriteLine($"Name = {index.Key}, Size = {index.Value}");
                        // }
                    }
                }
                PrintMap(ref map);
                Console.ResetColor();
                int selectedShip;
                if (int.TryParse(Console.ReadLine(), out selectedShip) && indexOfSelectedShip.ContainsKey(selectedShip) && selectedShip <= settings.shipSpecifications.Count)
                {
                    Console.WriteLine("Set the coordinates of " + indexOfSelectedShip[selectedShip] + " in following format: collumn, row");
                    SetShipCoordinates(indexOfSelectedShip[selectedShip], indexOfSelectedShip, selectedShip);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please select a ship from the list.");
                    Atomic.GameSettingsError();
                }
            }
            Console.WriteLine("All ships are placed on the map. The game is ready to start.");
            Console.WriteLine("Press any key to start the game.");
            Console.ReadKey();
            Console.Clear();
            CPU.GetCPUSet();
            StartGame();
        }
        public void CreateMap()
        {
            if (settings.mapType == true)
            {
                //if mapType == true, create a rectangular/square shaped map
                CreateRectangularMap(ref map);
                PrintMap(ref map);
            }
            else
            {
                //if mapType == false, create a circle shaped map
                CreateCircularMap(ref map);
            }
        }

        public void CreateRectangularMap(ref bool?[,] map)
        {
            int width = settings.mapWidth ?? 10;
            int height = settings.mapHeight ?? 10;

            // Create 2D array to store the map
            //null = water, false = ship, true = hit
            map = new bool?[height, width];

            // Initialize the map with water ("~") - without ships
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    map[i, j] = null;
                }
            }
        }

        public void CreateCircularMap(ref bool?[,] map)
        {
            int diameter = settings.mapDiameter ?? 20;
            int radius = diameter / 2;

            // Create 2D array to store the circular map
            //null = water, false = ship, true = hit
            map = new bool?[diameter, diameter];

            // Initialize the map with water ('~') like it was a square (rendering it as a circle is done in the section below)
            for (int i = 0; i < diameter; i++)
            {
                for (int j = 0; j < diameter; j++)
                {
                    map[i, j] = null;
                }
            }

            PrintMap(ref map);
        }

        public void PrintMap(ref bool?[,] map)
        {
            int height = map.GetLength(0);
            int width = map.GetLength(1);

            // Column coordinates indexing
            Console.Write("  ");
            for (int j = 0; j < width; j++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{j,2}");
            }
            Console.ResetColor();
            Console.WriteLine();

            for (int i = 0; i < height; i++)
            {
                // Row coordinates indexing
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"{i,2} ");
                Console.ResetColor();

                int counter = 0;
                for (int j = 0; j < width; j++)
                {
                    if(mapMasking == true){
                        Console.ForegroundColor = Atomic.MakeChessboard(counter, i);
                        if(map[i, j] == false || map[i, j] == null || map[i, j] == true){
                            Console.Write("~ ");
                        }
                    }else{
                        // Check if the point is within the circle (for circular maps)
                        if (settings.mapType == false && Math.Pow(i - height / 2, 2) + Math.Pow(j - width / 2, 2) > Math.Pow(width / 2, 2))
                        {
                            Console.Write("   "); // Print empty space outside the circle
                        }
                        else
                        {
                            // Water
                            Console.ForegroundColor = Atomic.MakeChessboard(counter, i);
                            if (map[i, j] == null)
                            {
                                Console.Write("~ ");
                            }
                            // Ship
                            else if (map[i, j] == false)
                            {
                                Console.ForegroundColor = settings.colorTheme ?? ConsoleColor.White;
                                Console.Write("S ");
                            }
                            // Hit
                            else if (map[i, j] == true)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("X ");
                            }
                            counter++;
                        }
                    }
                }
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        private void SetShipCoordinates(string shipName, Dictionary<int, string> indexOfSelectedShip, int selectedShip)
        {
            int x, y;

            while (true)
            {
                Console.WriteLine("Enter the starting column (x):");
                if (int.TryParse(Console.ReadLine(), out x))
                {
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid number for the column.");
                    Console.ResetColor();
                }
            }

            while (true)
            {
                Console.WriteLine("Enter the starting row (y):");
                if (int.TryParse(Console.ReadLine(), out y))
                {
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid number for the row.");
                    Console.ResetColor();
                }
            }

            Console.WriteLine("\nSet the orientation of the ship:\n1. horizontal\n2. vertical");
            bool orientation = true; // Default to horizontal
            switch (Console.ReadLine())
            {
                case "1":
                    orientation = true;
                    break;
                case "2":
                    orientation = false;
                    break;
                default:
                    Console.WriteLine("Invalid input. Defaulting to horizontal orientation.");
                    orientation = true;
                    break;
            }
            // Validate the coordinates and orientation
            if (ValidateShipPlacement(shipName, x, y, orientation))
            {
                //TEST
                // Console.WriteLine("PROBEHLO");
                currentAddedShip = shipName;
                //Counts with the ship as it was a part of the map
                PlaceShipOnMap(shipName, x, y, orientation);
                //Now it prints the map againg with all of the ships, that are already placed
                PrintMap(ref map);
                //Coordinates are set for the ship
                CoordinateIsSet[shipName] = true;
                indexOfSelectedShip.Remove(selectedShip);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid coordinates. The ship goes out of bounds or overlaps with another ship.");
                Atomic.GameSettingsError();
            }
        }

        private bool ValidateShipPlacement(string shipName, int x, int y, bool orientation)
        {
            int width = settings.mapWidth ?? 10;
            int height = settings.mapHeight ?? 10;
            int shipLength = settings.shipSpecifications[shipName][1];
            int shipWidth = settings.shipSpecifications[shipName][0];

            if (orientation) // Horizontal
            {
                if (x + shipLength > width || y + shipWidth > height)
                {
                    return false; // Ship goes out of bounds horizontally
                }
                for (int i = 0; i < shipWidth; i++)
                {
                    for (int j = 0; j < shipLength; j++)
                    {
                        if (x + j >= width || y + i >= height || map[y + i, x + j] != null)
                        {
                            return false; // Overlapping ship found
                        }
                    }
                }
            }
            else // Vertical
            {
                if (y + shipLength > height || x + shipWidth > width)
                {
                    return false; // Ship goes out of bounds vertically
                }
                for (int i = 0; i < shipLength; i++)
                {
                    for (int j = 0; j < shipWidth; j++)
                    {
                        if (y + i >= height || x + j >= width || map[y + i, x + j] != null)
                        {
                            return false; // Overlapping ship found
                        }
                    }
                }
            }

            // Additional checks can be added here (e.g., checking for overlapping ships)

            return true;
        }

        private void PlaceShipOnMap(string shipName, int x, int y, bool orientation)
        {
            int shipLength = settings.shipSpecifications[shipName][1];
            int shipWidth = settings.shipSpecifications[shipName][0];

            //true = horizontal
            if (orientation)
            {
                for (int i = 0; i < shipWidth; i++)
                {
                    for (int j = 0; j < shipLength; j++)
                    {
                        map[y + i, x + j] = false;
                    }
                }
            }
            // false = vertical
            else
            {
                for (int i = 0; i < shipWidth; i++)
                {
                    for (int j = 0; j < shipLength; j++)
                    {
                        map[y + j, x + i] = false;
                    }
                }
            }
        }

        private void StartGame()
        {
            mapMasking = true;
            Atomic.StartGameMessage();
            
            Console.WriteLine(Atomic.MapViewAnouncement(mapView));
            PrintMap(ref CPU.mapCPU);


            //TEST
            // if(Console.ReadLine() == "1"){
            //     mapMasking = false;
            // }
            // PrintMap(ref CPU.mapCPU);
        }
    }
}