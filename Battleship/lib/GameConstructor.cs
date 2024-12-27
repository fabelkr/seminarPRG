using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Sockets;
using App.lib.Computer;
using App.lib.RenderASCII;
//TODO: testing testing and testing
//TODO: Add ASCII art for ships
namespace App.lib
{
    public class GameConstructor
    {
        public Dictionary<string, bool[,]> shipState;
        public Dictionary<string, List<(int, int)>> shipPositionsMap = new Dictionary<string, List<(int, int)>>();

        //Player plays = true, CPU plays = false
        public bool turn = true;

        //0 = water, 1 = ship, 2 = hit, 3 = sunken ship, 4 = missed shot
        public int[,] map;
        private bool mapMasking = false;
        public bool empCPU = false;
        public bool empPlayer = false;

        private bool missileOrientation;
        private bool carpetOrientation;
        int sunkenShipCounter = 0;
        public int sunkenShipCounterCPU = 0;

        public int turnIndex = 1;

        public int selectedWeapon;

        private bool mapView = false;

        private GameSettings settings;
        string currentAddedShip;
        bool[,] shipCoordinates;

            
        //Dictionary for storing data if there are any coordinates set for the ship
        Dictionary<string, bool> CoordinateIsSet = new Dictionary<string, bool>();

            SetCPU CPU;
        //Constuctor
        public GameConstructor(GameSettings settings)
        {
            this.settings = settings;
            //initialize the shipState dictionary
            CPU = new SetCPU(settings, this);
            shipState = new Dictionary<string, bool[,]>();
            shipPositionsMap = new Dictionary<string, List<(int, int)>>();
            //Sets the state of each in game included ship to false by default (Not hit)
            foreach (KeyValuePair<string, int[]> ship in settings.shipSpecifications)
            {
                shipCoordinates = new bool[ship.Value[0], ship.Value[1]];

                // Initialize all coordinates of ship to false (not hit)
                for (int i = 0; i < ship.Value[0]; i++)
                {
                    for (int j = 0; j < ship.Value[1]; j++)
                    {
                        shipCoordinates[i, j] = false;
                    }
                }

                // Add the ship coordinates and its state to the dictionary
                shipState[ship.Key] = shipCoordinates;

            }
            //TEST
            // foreach (KeyValuePair<string, bool> state in shipState)
            // {
            //     Console.WriteLine($"ship: {state.Key}, state: {state.Value}");
            // }
        }

        public void CreateNewGame()
        {
                //TODO: TEST
                foreach (KeyValuePair <string, bool[,]> state in shipState){
                    Console.WriteLine("name:" + state.Key + "value" + state.Value);
                }

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

            for(int i = 1; i <= settings.shipSpecifications.Count; i++)
            {
                indexOfSelectedShip[i] = settings.shipSpecifications.ElementAt(i - 1).Key;
            }
            //The loop will run until all of the ships are placed on the map
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

        public void CreateRectangularMap(ref int[,] map)
        {
            int width = settings.mapWidth ?? 10;
            int height = settings.mapHeight ?? 10;

            // Create 2D array to store the map
            //null = water, false = ship, true = hit
            map = new int[height, width];

            // Initialize the map with water ("~") - without ships
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    map[i, j] = 0;
                }
            }
        }

        public void CreateCircularMap(ref int[,] map)
        {
            int diameter = settings.mapDiameter ?? 20;
            int radius = diameter / 2;

            // Create 2D array to store the circular map
            //null = water, false = ship, true = hit
            map = new int[diameter, diameter];

            // Initialize the map with water ('~') like it was a square (rendering it as a circle is done in the section below)
            for (int i = 0; i < diameter; i++)
            {
                for (int j = 0; j < diameter; j++)
                {
                    map[i, j] = 0;
                }
            }

            PrintMap(ref map);
        }

        public void PrintMap(ref int[,] map)
        {
            int height = map.GetLength(0);
            int width = map.GetLength(1);

            // Column coordinates indexing
            Console.Write("  ");
            for (int j = 0; j < width; j++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                //copilot helped me with the formated output of indexing
                Console.Write($"{j,2}");
            }
            Console.ResetColor();
            Console.WriteLine();

            for (int i = 0; i < height; i++)
            {
                // Row coordinates indexing
                Console.ForegroundColor = ConsoleColor.Red;
                //copilot helped me with the formated output of indexing
                Console.Write($"{i,2} ");
                Console.ResetColor();

                int counter = 0;
                for (int j = 0; j < width; j++)
                {
                    if(mapMasking == true){
                        Console.ForegroundColor = Atomic.MakeChessboard(counter, i);
                        if(map[i, j] == 1 || map[i, j] == 0){
                            Console.Write("~ ");
                        }
                        else if(map[i, j] == 4){
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("O ");
                            Console.ResetColor();
                        }
                        else if(map[i, j] == 2){
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("X ");
                            Console.ResetColor();
                        }
                        else if(map[i, j] == 3){
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Write("# ");
                            Console.ResetColor();
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
                            if (map[i, j] == 0)
                            {
                                Console.Write("~ ");
                            }
                            // Ship
                            else if (map[i, j] == 1)
                            {
                                Console.ForegroundColor = settings.colorTheme ?? ConsoleColor.White;
                                Console.Write("S ");
                            }
                            // Hit
                            else if (map[i, j] == 2)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("X ");
                            }
                            else if (map[i, j] == 4)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("O ");
                            }
                            else if (map[i, j] == 3)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Write("# ");
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
                        if (x + j >= width || y + i >= height || map[y + i, x + j] != 0)
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
                        if (y + i >= height || x + j >= width || map[y + i, x + j] != 0)
                        {
                            return false; // Overlapping ship found
                        }
                    }
                }
            }

            // Additional checks can be added here (e.g., checking for overlapping ships)

            return true;
        }

        public void PlaceShipOnMap(string shipName, int x, int y, bool orientation)
        {
            int shipLength = settings.shipSpecifications[shipName][1];
            int shipWidth = settings.shipSpecifications[shipName][0];

            // Initialize ship coordinates
            bool[,] shipCoordinates = new bool[shipWidth, shipLength];
            List<(int, int)> shipPositions = new List<(int, int)>();
            //true = horizontal
            if (orientation)
            {
                for (int i = 0; i < shipWidth; i++)
                {
                    for (int j = 0; j < shipLength; j++)
                    {
                        map[y + i, x + j] = 1;
                        shipCoordinates[i, j] = false;
                        shipPositions.Add((y + i, x + j)); // Store the position as a unique integer
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
                        map[y + j, x + i] = 1;
                        shipCoordinates[i, j] = false;
                        shipPositions.Add((y + j, x + i)); // Store the position as a unique integer
                    }
                }
            }

            // Add the ship coordinates and its state to the dictionary
            shipState[shipName] = shipCoordinates;

            // Store the ship positions in the dictionary
            shipPositionsMap[shipName] = shipPositions;
        }

        private void StartGame()
        {
            Console.Clear();
            int empDurationPlayer = 3;
            int empDurationCPU = 3;
            mapMasking = true;
            Atomic.StartGameMessage();

            while (settings.shipSpecifications.Count >= sunkenShipCounter || settings.shipSpecifications.Count >= sunkenShipCounterCPU)
            {
                if (turn){
                    
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Your points: " + sunkenShipCounter + " Ships sunk");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("CPU points" + sunkenShipCounterCPU + " Ships sunk");
                    Console.ResetColor();


                    while (true)
                    {
                        Console.WriteLine(Atomic.MapViewAnouncement(mapView));
                        if (mapView)
                        {
                            mapMasking = false;
                            PrintMap(ref map);
                        }
                        else
                        {
                            mapMasking = true;
                            PrintMap(ref CPU.mapCPU);
                        }
                        if (sunkenShipCounterCPU == settings.shipSpecifications.Count)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You have lost the game! Better luck next time!");
                            Console.WriteLine("Press any key to exit the game.");
                            Console.ReadKey();
                            Environment.Exit(0);
                        }
                        else if (sunkenShipCounter == settings.shipSpecifications.Count)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("You have won the game! Congratulations!");
                            Console.WriteLine("Press any key to exit the game.");
                            Console.ReadKey();
                            Environment.Exit(0);
                        }
                        Console.WriteLine("Type 'map' to switch views or press Enter to continue.");
                        string input = Console.ReadLine();

                        if (input == "map")
                        {
                            mapView = !mapView;
                            // mapMasking = false;
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                    if(settings.rechargeTime.Any(x => x > 0)){

                        for(int i = 0; i < settings.rechargeTime.Length; i++){

                            if(settings.rechargeTime[i] > 0){
                                settings.rechargeTime[i]--;

                                if(settings.rechargeTime[i] == 0){
                                    settings.remainingWeaponUsage[i] = true;
                                }
                            }
                        }
                    }
                    if(settings.rechargeTimeCPU.Any(x => x > 0)){

                        for(int i = 0; i < settings.rechargeTimeCPU.Length; i++){
                            if(settings.rechargeTimeCPU[i] > 0){
                                settings.rechargeTimeCPU[i]--;

                                if(settings.rechargeTimeCPU[i] == 0){
                                    settings.remainingWeaponUsageCPU[i] = true;
                                }
                            }
                        }
                    }

                    if(empPlayer){
                        Console.WriteLine("You have been hit by EMP strike. You can use only torpedo, this efect will last for: " + empDurationPlayer + " turns.");
                        Console.WriteLine("Press any key to shoot with torpedo");
                        Console.ReadKey();
                        SetShotCoordinates("Torpedo");
                        empDurationPlayer--;
                        if(empDurationPlayer == 0){
                            empPlayer = false;
                            empDurationPlayer = 3;
                        }
                    }
                    else
                    {
                        bool validWeaponSelected = false;

                        while (!validWeaponSelected)
                        {
                            for (int i = 0; i < settings.weaponSpecifications.Count; i++)
                            {
                                var weapon = settings.weaponSpecifications.ElementAt(i);
                                Console.Write(i + 1 + ". " + weapon.Key + " (" + weapon.Value[0] + " X " + weapon.Value[1] + ") (weapon ready: ");
                                if(settings.remainingWeaponUsage[4] == false && weapon.Key == "EMP"){
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write("no");
                                }
                                else if (settings.remainingWeaponUsage[i])
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.Write("yes");
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write("no");
                                }
                                Console.ResetColor();
                                Console.WriteLine(")");
                            }

                            Dictionary<int, string> indexOfSelectedWeapon = new Dictionary<int, string>();

                            for (int i = 1; i <= settings.weaponSpecifications.Count; i++)
                            {
                                indexOfSelectedWeapon[i] = settings.weaponSpecifications.ElementAt(i - 1).Key;
                            }

                            if (int.TryParse(Console.ReadLine(), out selectedWeapon) && indexOfSelectedWeapon.ContainsKey(selectedWeapon))
                            {
                                string selectedWeaponName = indexOfSelectedWeapon[selectedWeapon];
                                int weaponIndex = Array.IndexOf(settings.weaponNames, selectedWeaponName);

                                if(settings.rechargeTime[weaponIndex] > 5000){
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("This is a single use weapon, you can't use it anymore.");
                                    Console.ResetColor();
                                }
                                else if (selectedWeaponName == "EMP" && settings.rechargeTimeEMP > 0)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("You need to sink enemy ship to use EMP again. Please select another weapon.");
                                    Console.ResetColor();
                                }
                                else if (settings.rechargeTime[weaponIndex] > 0)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("This weapon is recharging. Please select another weapon.");
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.WriteLine("Selected weapon: " + selectedWeaponName);
                                    SetShotCoordinates(selectedWeaponName);
                                    validWeaponSelected = true;
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid input. Please select a weapon from the list.");
                                Console.ResetColor();
                            }
                        }
                    }
                    turn = false;
                    turnIndex++;
                }
                else{
                    CPU.SelectWeaponCPU();
                    turn = true;
                    turnIndex++;
                    if(empCPU){
                        empDurationCPU--;
                        if(empDurationCPU == 0){
                            empCPU = false;
                            empDurationCPU = 3;
                        }
                    }
                }
            }
        }

        private void SetShotCoordinates(string weaponType){
            int x = 0;
            int y = 0;
            if(weaponType == "Carpet Bomber"){
                Console.WriteLine("Set the orientation of the carpet bomber:\n1. horizontal\n2. vertical");
                switch (Console.ReadLine())
                {
                    case "1":
                        carpetOrientation = false;
                        break;
                    case "2":
                        carpetOrientation = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Defaulting to horizontal orientation.");
                        carpetOrientation = false;
                        break;
                }

                Console.WriteLine("Enter the column coordinate (x):");
                if (int.TryParse(Console.ReadLine(), out x) && x >= 0 && x < settings.mapWidth)
                {

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid number for the column.");
                    Console.ResetColor();
                    SetShotCoordinates("Carpet Bomber");
                }

                Console.WriteLine("Enter the row coordinate (y):");
                if (int.TryParse(Console.ReadLine(), out y) && y >= 0 && y < settings.mapHeight)
                {

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Please enter a valid number for the row.");
                    Console.ResetColor();
                    SetShotCoordinates("Carpet Bomber");
                }
                settings.remainingWeaponUsage[6] = false;
            }
            else if(weaponType == "Missile"){
                Console.WriteLine("You can send the missile from the sides of the map only.");
                Console.WriteLine("Enter the side of the map you want to send the missile from:\n1. Top\n2. Left");
                switch (Console.ReadLine())
                {
                    case "1":
                        y = 0;
                        Console.WriteLine("Enter the column coordinate (x):");
                        if (int.TryParse(Console.ReadLine(), out x) && x >= 0 && x < settings.mapWidth)
                        {

                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid input. Please enter a valid number for the column.");
                            Console.ResetColor();
                            SetShotCoordinates("Missile");
                        }
                        if(x != 0){
                            missileOrientation = false;
                        }
                        else{
                            Console.WriteLine("Do you wnat to shoot horizontally or vertically?");
                            Console.WriteLine("1. Horizontal\n2. Vertical");
                            if(Console.ReadLine() == "1"){
                                missileOrientation = false;
                            }
                            else{
                                missileOrientation = true;
                            }
                        }
                        break;
                    case "2":
                        x = 0;
                        Console.WriteLine("Enter the row (y):");
                        if (int.TryParse(Console.ReadLine(), out y) && y >= 0 && y < settings.mapHeight)
                        {

                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid input. Please enter a valid number for the row.");
                            Console.ResetColor();
                            SetShotCoordinates("Missile");
                        }
                        if(y != 0){
                            missileOrientation = true;
                        }
                        else{
                            Console.WriteLine("Do you wnat to shoot horizontally or vertically?");
                            Console.WriteLine("1. Horizontal\n2. Vertical");
                            if(Console.ReadLine() == "1"){
                                settings.rechargeTime[1] = 5;
                                settings.remainingWeaponUsage[1] = false;
                                missileOrientation = false;
                            }
                            else{
                                settings.rechargeTime[1] = 5;
                                settings.remainingWeaponUsage[1] = false;
                                missileOrientation = true;
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid input. Defaulting to top orientation.");
                        missileOrientation = true;
                        break;
                }
                settings.remainingWeaponUsage[1] = false;
                settings.rechargeTime[1] = 5;
            }
            else if(weaponType == "EMP"){
                x = 0;
                y = 0;
            }
            else{
                Console.WriteLine("Enter the target coordinates");

                while (true)
                {
                    Console.WriteLine("Enter the column coordinate (x):");
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
                    Console.WriteLine("Enter the row (y):");
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
            }
            ValidateShotCoordinates(weaponType, ref x, ref y);
            Fire(weaponType, x, y);
        }

        private void ValidateShotCoordinates(string weaponName, ref int x, ref int y)
        {
            int width = settings.mapWidth ?? 10;
            int height = settings.mapHeight ?? 10;

            int weaponWidth = settings.weaponSpecifications[weaponName][0];
            int weaponHeight = settings.weaponSpecifications[weaponName][1];

            if (x < 0 || x + weaponWidth > width || y < 0 || y + weaponHeight > height)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You are shooting outside the map. You need to change the coordinates by pressing enter (or if you resist to do so, type \"no\" and let the game crash).");
                Console.ResetColor();
                if (Console.ReadLine() == "no")
                {
                    RenderMatrixEffect();
                }
                else
                {
                    bool valid = false;
                    while (!valid)
                    {
                        Console.WriteLine("Enter new x coordinate:");
                        if (int.TryParse(Console.ReadLine(), out x) && x >= 0 && x + weaponWidth <= width)
                        {
                            valid = true;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid input. Please enter a valid number for the x coordinate.");
                            Console.ResetColor();
                        }
                    }

                    valid = false;
                    while (!valid)
                    {
                        Console.WriteLine("Enter new y coordinate:");
                        if (int.TryParse(Console.ReadLine(), out y) && y >= 0 && y + weaponHeight <= height)
                        {
                            valid = true;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid input. Please enter a valid number for the y coordinate.");
                            Console.ResetColor();
                        }
                    }
                }
            }
        }

        private void Fire(string weaponName, int x, int y)
        {
            if (weaponName == "Depth Charge")
            {
                DepthCharge(ref CPU.mapCPU, weaponName, x, y);
            }
            else if (weaponName == "Torpedo")
            {
                Torpedo(ref CPU.mapCPU, x, y);
            }
            else if (weaponName == "Missile")
            {
                Missile(ref CPU.mapCPU, x, y, missileOrientation);
            }
            else if (weaponName == "Nuke")
            {
                DepthCharge(ref CPU.mapCPU, weaponName, x, y);
            }
            else if (weaponName == "EMP")
            {
                EMP();
            } 
            else if (weaponName == "Scanner")
            {
                Scanner(ref CPU.mapCPU, x, y);
            }
            else if (weaponName == "Carpet Bomber")
            {
                CarpetBomber(ref CPU.mapCPU, x, y, carpetOrientation);
            }
            else
            {
                Console.WriteLine("Invalid weapon name.");
            }
            mapMasking = true;
        }

        private void UpdateShipState(int y, int x)
        {
            foreach (var ship in CPU.shipPositionsMapCPU)
            {
                string shipName = ship.Key;
                List<(int, int)> shipPositions = ship.Value;

                // Check if the hit coordinates match any of the ship's positions
                if (shipPositions.Contains((y, x)))
                {
                    // Directly update the ship's state in the shipStateCPU dictionary
                    for (int i = 0; i < shipPositions.Count; i++)
                    {
                        if (shipPositions[i] == (y, x))
                        {
                            // Find the exact position in the shipCoordinates array
                            for (int j = 0; j < CPU.shipStateCPU[shipName].GetLength(0); j++)
                            {
                                for (int k = 0; k < CPU.shipStateCPU[shipName].GetLength(1); k++)
                                {
                                    if (shipPositions[j * CPU.shipStateCPU[shipName].GetLength(1) + k] == (y, x))
                                    {
                                        CPU.shipStateCPU[shipName][j, k] = true; // Mark the position as hit
                                    }
                                }
                            }
                        }
                    }

                    if (IsShipSunk(shipName))
                    {
                        if (turn)
                        {
                            Console.WriteLine($"Ship {shipName} is sunk!");
                            sunkenShipCounter++;
                            settings.remainingWeaponUsage[6] = true;
                            settings.remainingWeaponUsage[4] = true;  
                        }
                        else
                        {
                            sunkenShipCounterCPU++;
                            settings.remainingWeaponUsageCPU[6] = true;
                            settings.remainingWeaponUsageCPU[4] = true;
                            Console.ReadKey();
                        }

                    }

                    return;
                }
            }
        }

        private bool IsShipSunk(string shipName)
        {
            bool[,] shipCoordinates = CPU.shipStateCPU[shipName];
            for (int i = 0; i < shipCoordinates.GetLength(0); i++)
            {
                for (int j = 0; j < shipCoordinates.GetLength(1); j++)
                {
                    if (!shipCoordinates[i, j])
                    {
                        return false; // Ship is not completely sunk
                    }
                }
            }
            foreach (var position in CPU.shipPositionsMapCPU[shipName])
            {
                int y = position.Item1;
                int x = position.Item2;
                CPU.mapCPU[y, x] = 3; // Assign the value 3 to the map coordinates
            }
            settings.remainingWeaponUsage[4] = true;
            settings.rechargeTimeEMP = 0;
            return true; // Ship is completely sunk
        }

        public void DepthCharge(ref int[,] objectiveMap, string weaponName, int x, int y)
        {
            int weaponWidth = settings.weaponSpecifications[weaponName][0];
            int weaponHeight = settings.weaponSpecifications[weaponName][1];

            for (int i = 0; i < weaponHeight; i++)
            {
                for (int j = 0; j < weaponWidth; j++)
                {
                    if (objectiveMap[y + i, x + j] == 1)
                    {
                        objectiveMap[y + i, x + j] = 2; // Hit
                        if(objectiveMap == CPU.mapCPU){
                            UpdateShipState(y + i, x + j);
                        }
                        else if (objectiveMap == map){
                            CPU.UpdateShipStateCPU(y + i, x + j);
                        }
                        // PrintShipState();
                    }
                    else if (objectiveMap[y + i, x + j] == 0)
                    {
                        objectiveMap[y + i, x + j] = 4; // Miss
                    }
                }
            }

            //Set the recharge time for the weapon (Depth Charge or Nuke)
            if(objectiveMap == CPU.mapCPU){
                if(weaponName == "Depth Charge"){
                    settings.remainingWeaponUsage[2] = false;
                    settings.rechargeTime[2] = 7000;
                }
                else{
                    settings.remainingWeaponUsage[3] = false;
                    settings.rechargeTime[3] = 7000;
                }
            }
            else if (objectiveMap == map){
                if(weaponName == "Depth Charge"){
                    settings.remainingWeaponUsageCPU[2] = false;
                    settings.rechargeTimeCPU[2] = 7000;
                }
                else{
                    settings.remainingWeaponUsage[3] = false;
                    settings.rechargeTimeCPU[3] = 7000;
                }
            }
        }

        //TEST - debuging
        // private void PrintShipState()
        // {
        //     foreach (KeyValuePair<string, bool[,]> state in CPU.shipStateCPU)
        //     {
        //         Console.WriteLine("Ship name: " + state.Key);

        //         for (int i = 0; i < state.Value.GetLength(0); i++)
        //         {
        //             for (int j = 0; j < state.Value.GetLength(1); j++)
        //             {
        //                 Console.Write(state.Value[i, j] ? "1 " : "0 ");
        //             }
        //             Console.WriteLine();
        //         }
        //         Console.WriteLine();
        //     }

        //     PrintShipCoordinates();
        // }

        // private void PrintShipCoordinates()
        // {
        //     foreach (KeyValuePair<string, List<(int, int)>> ship in CPU.shipPositionsMapCPU)
        //     {
        //         Console.WriteLine("Ship name: " + ship.Key);
        //         List<(int, int)> shipPositions = ship.Value;

        //         foreach (var pos in shipPositions)
        //         {
        //             Console.WriteLine($"({pos.Item1}, {pos.Item2})");
        //         }
        //         Console.WriteLine();
        //     }
        // }

        public void Torpedo(ref int[,] map, int x, int y){
            if (map[y, x] == 1)
            {
                map[y, x] = 2; // Hit
                if(map == CPU.mapCPU){
                    UpdateShipState(y, x);
                }
                else{
                    CPU.UpdateShipStateCPU(y, x);
                }
                //PrintShipState();
            }
            else if (map[y, x] == 0)
            {
                map[y, x] = 4; // Miss
            }
            else if (map[y, x] == 4)
            {
                Console.WriteLine("You have already hit this position.");
            }
        }

        public void Missile(ref int[,] map, int x, int y, bool orientation){
            // If starting point is in the 0x 0y corner
            if (x == 0 && y == 0)
            {
                if (orientation)
                {
                    // Vertical orientation
                    for (int i = 0; i < settings.mapHeight; i++)
                    {
                        if (map[i, x] == 1)
                        {
                            map[i, x] = 2; // Hit
                            if(map == CPU.mapCPU){
                                UpdateShipState(i, x);
                            }
                            else{
                                CPU.UpdateShipStateCPU(i, x);
                                settings.rechargeTimeCPU[1] = 5;
                                settings.remainingWeaponUsageCPU[1] = false;
                            }
                            break;
                        }
                        else if(map[i, x] == 2)
                        {
                            map[i, x] = 2;
                        }
                        else if(map[i, x] == 3){
                            map[i, x] = 3;
                        }
                        else
                        {
                            map[i, x] = 4;
                        }
                    }
                }
                else
                {
                    // Horizontal orientation
                    for (int j = 0; j < settings.mapWidth; j++)
                    {
                        if (map[y, j] == 1)
                        {
                            map[y, j] = 2; // Hit
                            if(map == CPU.mapCPU){
                                UpdateShipState(y, j);
                            }
                            else{
                                CPU.UpdateShipStateCPU(y, j);
                                settings.rechargeTimeCPU[1] = 5;
                                settings.remainingWeaponUsageCPU[1] = false;
                            }
                            break;
                        }
                        else if(map[y, j] == 2)
                        {
                            map[y, j] = 2;
                        }
                        else if(map[y, j] == 3){
                            map[y, j] = 3;
                        }
                        else
                        {
                            map[y, j] = 4;
                        }
                    }
                }
            }
            // Starting from the left edge, horizontal orientation
            else if (x == 0)
            {
                for (int j = 0; j < settings.mapWidth; j++)
                {
                    if (map[y, j] == 1)
                    {
                        map[y, j] = 2; // Hit
                        if(map == CPU.mapCPU){
                            UpdateShipState(y, j);
                        }
                        else{
                            CPU.UpdateShipStateCPU(y, j);
                        }
                        break;
                    }
                    else if(map[y, j] == 2)
                    {
                        map[y, j] = 2;
                    }
                    else if(map[y, j] == 3){
                        map[y, j] = 3;
                    }
                    else
                    {
                        map[y, j] = 4;
                    }
                }
            }
            else if (y == 0)
            {
                // Starting from the top edge, vertical orientation
                for (int i = 0; i < settings.mapHeight; i++)
                {
                    if (map[i, x] == 1)
                    {
                        map[i, x] = 2; // Hit
                        if(map == CPU.mapCPU){
                            UpdateShipState(i, x);
                        }
                        else{
                            CPU.UpdateShipStateCPU(i, x);
                        }
                        break;
                    }
                    else if(map[i, x] == 2)
                    {
                        map[i, x] = 2;
                    }
                    else if(map[i, x] == 3){
                        map[i, x] = 3;
                    }
                    else
                    {
                        map[i, x] = 4;
                    }
                }
            }
        }

        public void Scanner(ref int[,] map, int x, int y)
        {
            mapMasking = false;

            // this prints the 3x3 scanned area 
            for (int i = y - 1; i <= y + 1; i++)
            {
                for (int j = x - 1; j <= x + 1; j++)
                {
                    //this prints line 3X3 of scanned area
                    if (i >= 0 && i < map.GetLength(0) && j >= 0 && j < map.GetLength(1))
                    {
                        if(map[i, j] == 1)
                        {
                            Console.ForegroundColor = settings.colorTheme ?? ConsoleColor.White;
                        }
                        else if(map[i, j] == 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        else if(map[i, j] == 4)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        Console.Write(map[i, j] == 1 ? "S " : map[i, j] == 2 ? "X " : map[i, j] == 4 ? "O " : "~ ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }
                Console.WriteLine();
            }

            mapMasking = true;
        }

        public void CarpetBomber(ref int[,] map, int x, int y, bool orientation)
        {
            if (orientation)
            {
                // Vertical
                for (int i = 0; i < 2; i++)
                {
                    if (y + i < settings.mapHeight)
                    {
                        if (map[y + i, x] == 1)
                        {
                            map[y + i, x] = 2;
                        if(map == CPU.mapCPU){
                            UpdateShipState(y + i, x);
                        }
                        else{
                            CPU.UpdateShipStateCPU(y + i, x);
                        }
                        }
                        else
                        {
                            map[y + i, x] = 4;
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j < 2; j++)
                {
                    if (x + j < settings.mapWidth)
                    {
                        if (map[y, x + j] == 1)
                        {
                            map[y, x + j] = 2;
                            if(map == CPU.mapCPU){
                                UpdateShipState(y, x + j);
                            }
                            else{
                                CPU.UpdateShipStateCPU(y, x + j);
                            }
                        }
                        else
                        {
                            map[y, x + j] = 4;
                        }
                    }
                }
            }
        }

        public void EMP()
        {
            if(turn)
            {
                settings.remainingWeaponUsage[4] = false;
                settings.rechargeTimeEMP = 2500;
                empCPU = true;
            }
            else
            {
                empPlayer = true;
            }
        }

        private void RenderMatrixEffect()
        {
            Console.Clear();
            Random random = new Random();
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            string username = Environment.UserName;
            string machineName = Environment.MachineName;
            string osVersion = Environment.OSVersion.ToString();
            string currentDirectory = Environment.CurrentDirectory;
            string systemDirectory = Environment.SystemDirectory;
            int processorCount = Environment.ProcessorCount;
            string userDomainName = Environment.UserDomainName;
            string ipAddress = GetLocalIPAddress();

            for (int frame = 0; frame < 20; frame++)
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write((char)random.Next(0x4E00, 0x9FFF));
                    }
                    Console.WriteLine();
                }
                System.Threading.Thread.Sleep(100);
                Console.Clear();
            }

            for (int flash = 0; flash < 5; flash++)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"WARNING: SYSTEM FAILURE IMMINENT, {username.ToUpper()}");
                System.Threading.Thread.Sleep(500);
                Console.Clear();
                System.Threading.Thread.Sleep(500);
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine($"Machine Name: {machineName}");
            Console.WriteLine($"OS Version: {osVersion}");
            Console.WriteLine($"Current Directory: {currentDirectory}");
            Console.WriteLine($"System Directory: {systemDirectory}");
            Console.WriteLine($"Processor Cores: {processorCount}");
            Console.WriteLine($"User Domain Name: {userDomainName}");
            Console.WriteLine($"IP Address: {ipAddress}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Just kidding, {username}! Please enter valid coordinates next time.");
            Console.ResetColor();
        }

        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}