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
        private GameSettings settings;
        string currentAddedShip;

        //null = water, false = ship, true = hit
        public bool?[,] map;
            
        //Dictionary for storing data if there are any coordinates set for the ship
        Dictionary<string, bool> CoordinateIsSet = new Dictionary<string, bool>();

        //Constuctor
        public GameConstructor(GameSettings settings)
        {
            this.settings = settings;
            //initialize the shipState dictionary
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
            SetCPU CPU = new SetCPU(settings, this);
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
            Console.WriteLine("All chips are placed on the map. The game is ready to start.");
            Console.WriteLine("Press any key to start the game.");
            Console.ReadKey();
            Console.Clear();
            CPU.GetCPUSet();
            //TODO: Implement the game logic
            //TODO: Implement the computer player
        }
        private void CreateMap()
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

            for (int i = 0; i < height; i++)
            {
                int counter = 0;
                for (int j = 0; j < width; j++)
                {
                    // Check if the point is within the circle (for circular maps)
                    if (settings.mapType == false && Math.Pow(i - height / 2, 2) + Math.Pow(j - width / 2, 2) > Math.Pow(width / 2, 2))
                    {
                        Console.Write("  "); // Print empty space outside the circle
                    }
                    else
                    {
                        //Water
                        Console.ForegroundColor = Atomic.MakeChessboard(counter, i);
                        if (map[i, j] == null)
                        {
                            Console.Write("~ ");
                        }
                        //ship
                        else if (map[i, j] == false)
                        {
                            Console.ForegroundColor = settings.colorTheme ?? ConsoleColor.White;
                            //This is how I wanted to render specific pseudonym for each ship type on the map, but it did not work
                            // Console.Write(Ships.CheckPseudoToRender(currentAddedShip));
                            Console.Write("S ");
                        }
                        //hit
                        else if (map[i, j] == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            // Ships.CheckPseudoToRender(currentAddedShip);
                            Console.Write("X ");
                            Console.ResetColor();
                        }
                        counter++;
                    }
                }
                Console.ResetColor();
                Console.WriteLine();
            }
        }

        private void SetShipCoordinates(string shipName, Dictionary<int, string> indexOfSelectedShip, int selectedShip)
        {
            Console.WriteLine("Enter the starting column (x):");
            int x = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the starting row (y):");
            int y = Convert.ToInt32(Console.ReadLine());

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
    }
}