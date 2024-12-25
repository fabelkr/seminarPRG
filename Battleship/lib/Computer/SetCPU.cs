using System;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using App.lib.RenderASCII;

namespace App.lib.Computer
{
    public class SetCPU
    {
        private GameSettings settings;
        private GameConstructor constructor;

        public Dictionary<string, List<(int, int)>> shipPositionsMapCPU = new Dictionary<string, List<(int, int)>>();
        public Dictionary<string, bool[,]> shipStateCPU = new Dictionary<string, bool[,]>();
        public int[,] mapCPU;

        private string chosenWeaponCPU;
        private bool firstWeaponCPU = false;

        int xC;
        int yC;
        bool missileOrientationCPU = false;

        // Constructor to initialize the settings and constructor objects
        public SetCPU(GameSettings settings, GameConstructor constructor)
        {
            this.settings = settings;
            this.constructor = constructor;
            mapCPU = new int[settings.mapHeight ?? 10, settings.mapWidth ?? 10];
            foreach (KeyValuePair<string, int[]> ship in settings.shipSpecifications)
            {
                shipStateCPU[ship.Key] = new bool[ship.Value[0], ship.Value[1]];
            }
        }

        public void GetCPUSet()
        {
            MapBuilder();
        }

        private void MapBuilder()
        {
            if(settings.mapType == true){
                constructor.CreateRectangularMap(ref mapCPU);
                SetCPURectangularCoordinates();
            }
            if (settings.mapType == false)
            {
                constructor.CreateCircularMap(ref mapCPU);
                SetCPUCircularCoordinates();
            }
        }

        private void SetCPURectangularCoordinates()
        {
            Random randomCPURectangularCoordinates = new Random();

            foreach(KeyValuePair<string, int[]> ship in settings.shipSpecifications){

                int shipWidth = ship.Value[0];
                int shipLength = ship.Value[1];
                int x, y;
                bool orientation;
                string shipName = ship.Key;
                bool validCoordinates = false;

                while (!validCoordinates)
                {
                    x = randomCPURectangularCoordinates.Next(0, settings.mapWidth ?? 10);
                    y = randomCPURectangularCoordinates.Next(0, settings.mapHeight ?? 10);
                    orientation = randomCPURectangularCoordinates.Next(0, 2) == 0;

                    if (ValidateCPUCoordinates(x, y, orientation, shipWidth, shipLength))
                    {
                        validCoordinates = true;

                        // Place the ship on the map
                        PlaceCPUShipOnMap(shipName, x, y, orientation, shipWidth, shipLength);
                    }
                }
            }
            //TEST
            // constructor.PrintMap(ref mapCPU);
            // constructor.PrintMap(ref constructor.map);
        }

        private void SetCPUCircularCoordinates()
        {
            Random randomCPUCircularCoordinates = new Random();
        }

        private bool ValidateCPUCoordinates(int x, int y, bool orientation, int shipWidth, int shipLength){
            int width = settings.mapWidth ?? 10;
            int height = settings.mapHeight ?? 10;

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
                        if (x + j >= width || y + i >= height || mapCPU[y + i, x + j] != 0)
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
                        if (y + i >= height || x + j >= width || mapCPU[y + i, x + j] != 0)
                        {
                            return false; // Overlapping ship found
                        }
                    }
                }
            }
            return true; // All checks passed, coordinates are valid
        }

        private void PlaceCPUShipOnMap(string shipName, int x, int y, bool orientation, int shipWidth, int shipLength)
        {
            bool[,] shipCoordinatesCPU = new bool[shipWidth, shipLength];
            List<(int, int)> shipPositionsCPU = new List<(int, int)>();

            if (orientation) // Horizontal
            {
                for (int i = 0; i < shipWidth; i++)
                {
                    for (int j = 0; j < shipLength; j++)
                    {
                        mapCPU[y + i, x + j] = 1;
                        shipCoordinatesCPU[i, j] = false;
                        shipPositionsCPU.Add((y + i, x + j)); // Store the position as a tuple
                    }
                }
            }
            else // Vertical
            {
                for (int i = 0; i < shipWidth; i++)
                {
                    for (int j = 0; j < shipLength; j++)
                    {
                        mapCPU[y + j, x + i] = 1;
                        shipCoordinatesCPU[i, j] = false;
                        shipPositionsCPU.Add((y + j, x + i)); // Store the position as a tuple
                    }
                }
            }

            // Store the ship positions in the dictionary
            shipPositionsMapCPU[shipName] = shipPositionsCPU;
        }

        public void SelectWeaponCPU()
        {
            Console.WriteLine("trun . " + constructor.turnIndex);
            bool valid = false;
            Random WeaponIndex = new Random();
            
            //rational
            if(constructor.empCPU){
                Console.WriteLine("EMP is active");
                chosenWeaponCPU = settings.weaponNames[0];
            }
            else if(settings.difficulty == 1){
                //set strategic starting weapon for CPU
                if(constructor.turnIndex < 3){
                    int startingWeapon = WeaponIndex.Next(1, 3);
                    //missile
                    if(startingWeapon == 1){
                        chosenWeaponCPU = settings.weaponNames[1];
                        settings.rechargeTimeCPU[1] = 6;
                        firstWeaponCPU = true;
                        settings.remainingWeaponUsageCPU[1] = false;
                    }
                    //Depth charge
                    else{
                        chosenWeaponCPU = settings.weaponNames[2];
                        settings.remainingWeaponUsageCPU[2] = false;
                    }
                }
                else{
                    while(!valid){
                        int usedWeaponIndex = WeaponIndex.Next(0, settings.weaponSpecifications.Count);
                        chosenWeaponCPU = settings.weaponNames[usedWeaponIndex];
                        Console.WriteLine($"Generated weapon index: {usedWeaponIndex}, chosen weapon: {chosenWeaponCPU}");
                        if(chosenWeaponCPU == "EMP" && constructor.empPlayer){
                            valid = false;
                        }
                        else if (settings.remainingWeaponUsageCPU[usedWeaponIndex] == true && settings.weaponSpecifications.ContainsKey(chosenWeaponCPU))
                        {
                            if(chosenWeaponCPU == "Missile"){
                                if(firstWeaponCPU){
                                    firstWeaponCPU = false;
                                }
                                else{
                                    settings.rechargeTimeCPU[1] = 5;
                                }
                            }
                            valid = true;
                        }
                        else{
                            valid = false;
                            Console.WriteLine($"remainingWeaponUsageCPU[{usedWeaponIndex}] = {settings.remainingWeaponUsageCPU[usedWeaponIndex]}");
                            Console.WriteLine($"weaponSpecifications.ContainsKey({chosenWeaponCPU}) = {settings.weaponSpecifications.ContainsKey(chosenWeaponCPU)}");
                            Console.ReadKey();
                        }
                    }
                }
            }
            Console.WriteLine($"CPU chose {chosenWeaponCPU}");
            SetShotCoordinatesCPU(chosenWeaponCPU);
        }


        private List<(int, int)> allHitCoordinates = new List<(int, int)>();
        private List<(int, int)> forbidenCoordinates = new List<(int, int)>();
        public void SetShotCoordinatesCPU(string weaponType){
            bool valid = false;
            Random setShotCoordinatesCPU = new Random();
            List<(int, int)> itemsToRemove = new List<(int, int)>();

            foreach (var forbiden in allHitCoordinates)
            {
                if (constructor.map[forbiden.Item1, forbiden.Item2] == 3)
                {
                    Console.WriteLine($"Forbiden coordinates: ({forbiden.Item1}, {forbiden.Item2}) - removing");
                    itemsToRemove.Add(forbiden);
                }
            }

            // Remove the collected items after the iteration
            foreach (var item in itemsToRemove)
            {
                allHitCoordinates.Remove(item);
            }

            foreach (var coord in allHitCoordinates)
            {
                if(constructor.map[coord.Item2, coord.Item1] == 3){
                    forbidenCoordinates.Add(coord);
                }
            }
            foreach (var forbiden in forbidenCoordinates)
            {
                Console.WriteLine($"Forbiden coordinates: ({forbiden.Item2}, {forbiden.Item1})");
            }

            if(allHitCoordinates.Count == 0 || allHitCoordinates.All(coord => constructor.map[coord.Item1, coord.Item2] == 3)){

                do{
                    xC = setShotCoordinatesCPU.Next(0, settings.mapWidth ?? 10);
                    yC = setShotCoordinatesCPU.Next(0, settings.mapHeight ?? 10);
                }
                while (constructor.map[xC, yC] == 4 || constructor.map[xC, yC] == 2 || constructor.map[xC, yC] == 3 ||
                       settings.weaponSpecifications[weaponType][0] + xC > settings.mapWidth ||
                       settings.weaponSpecifications[weaponType][1] + yC > settings.mapHeight);
                valid = true;
            }
            else{
                valid = false;
                while (!valid)
                {
                    // Print the entire allHitCoordinates list
                    Console.WriteLine("All hit coordinates:");
                    foreach (var coord in allHitCoordinates)
                    {
                        Console.WriteLine($"({coord.Item2}, {coord.Item1})");
                    }

                    // Iterate over the list of hit coordinates
                    foreach (var hitCoordinate in allHitCoordinates)
                    {

                        int x = hitCoordinate.Item2;
                        int y = hitCoordinate.Item1;

                        // Check adjacent coordinates
                        List<(int, int)> adjacentCoordinates = new List<(int, int)>
                        {
                            (x - 1, y), // Left
                            (x + 1, y), // Right
                            (x, y - 1), // Up
                            (x, y + 1)  // Down
                        };

                        Console.WriteLine($"Checking adjacent coordinates for hit at ({x}, {y})");
                        // Shuffle the adjacent coordinates
                        adjacentCoordinates = adjacentCoordinates.OrderBy(a => setShotCoordinatesCPU.Next()).ToList();

                        foreach (var coord in adjacentCoordinates)
                        {
                            int adjX = coord.Item1;
                            int adjY = coord.Item2;

                            if(constructor.map[adjY, adjX] == 3 || constructor.map[adjY, adjX] == 4 || constructor.map[adjY, adjX] == 2){
                                Console.WriteLine($"Adjacent coordinate ({adjX}, {adjY}) is not valid");
                                valid = false;
                                continue;
                            }

                            
                            // Check the coordinates validity for the Depth Charge weapon in relation to the map boundaries
                            if (weaponType == "Depth Charge")
                            {
                                if (adjX >= 0 && adjX + 3 <= (settings.mapWidth ?? 10) && adjY >= 0 && adjY + 3 <= (settings.mapHeight ?? 10))
                                {
                                    xC = adjX;
                                    yC = adjY;
                                    valid = true;
                                    break;
                                }
                            }
                            // Check the coordinates validity for the Nuke weapon in relation to the map boundaries
                            else if (weaponType == "Nuke"){
                                if (adjX >= 0 && adjX + 5 <= (settings.mapWidth ?? 10) && adjY >= 0 && adjY + 5 <= (settings.mapHeight ?? 10))
                                {
                                    xC = adjX;
                                    yC = adjY;
                                    valid = true;
                                    break;
                                }
                            }
                            // Check the coordinates validity for other weapons in relation to the map boundaries
                            else if (adjX >= 0 && adjX <= (settings.mapWidth ?? 10) && adjY >= 0 && adjY <= (settings.mapHeight ?? 10))
                            {
                                // Check if the adjacent coordinate has already been shot at
                                if (constructor.map[adjY, adjX] != 4 && constructor.map[adjY, adjX] != 2 && constructor.map[adjY, adjX] != 3 && !forbidenCoordinates.Contains((adjX, adjY)))
                                {
                                    Console.WriteLine($"Valid adjacent coordinate found at ({adjX}, {adjY})");
                                    // Set the shot coordinates to the adjacent coordinate
                                    xC = adjX;
                                    yC = adjY;
                                    valid = true;
                                    break;
                                }

                            }
                        }

                        if (valid)
                        {
                            break;
                        }
                    }

                    // If no valid adjacent coordinate is found, generate random coordinates
                    if (!valid)
                    {
                        xC = setShotCoordinatesCPU.Next(0, settings.mapWidth ?? 10);
                        yC = setShotCoordinatesCPU.Next(0, settings.mapHeight ?? 10);

                        if (constructor.map[xC, yC] == 4 || constructor.map[xC, yC] == 2 || constructor.map[xC, yC] == 3)
                        {
                            continue;
                        }
                        else if (settings.weaponSpecifications[weaponType][0] + xC > settings.mapWidth || settings.weaponSpecifications[weaponType][1] + yC > settings.mapHeight)
                        {
                            continue;
                        }
                        else if (weaponType == "Torpedo" && (constructor.map[xC, yC] == 2 || constructor.map[xC, yC] == 3 || constructor.map[xC, yC] == 4))
                        {
                            continue;
                        }
                        else if (weaponType == "Depth Charge" && xC >= 0 && xC + 3 < (settings.mapWidth ?? 10) && yC >= 0 && yC + 3 < (settings.mapHeight ?? 10))
                        {
                            continue;
                        }
                        else
                        {
                            valid = true;
                        }
                    }
                }
            }

            if (weaponType == "Depth Charge")
            {
                constructor.DepthCharge(ref constructor.map, weaponType, xC, yC);
            }
            else if (weaponType == "Torpedo")
            {
                constructor.Torpedo(ref constructor.map, xC, yC);
            }
            else if (weaponType == "Missile")
            {
                settings.remainingWeaponUsageCPU[1] = false;
                if (xC > yC){
                    yC = 0;
                    missileOrientationCPU = true;
                }
                else if(yC > xC){
                    xC = 0;
                    missileOrientationCPU = false;
                }
                else if(xC == yC){
                    xC = 0;
                    yC = 0;
                    
                    int orientation = setShotCoordinatesCPU.Next(0, 2);
                    if(orientation == 0){
                        missileOrientationCPU = true;
                    }
                }
                constructor.Missile(ref constructor.map, xC, yC, missileOrientationCPU);
            }
            else if (weaponType == "Nuke")
            {
                constructor.DepthCharge(ref constructor.map, weaponType, xC, yC);
            }
            else if (weaponType == "EMP")
            {
                constructor.EMP();
            } 
            else if (weaponType == "Scanner")
            {
                constructor.Scanner(ref constructor.map, xC, yC);
            }
            else if (weaponType == "Carpet Bomber")
            {
                int orientation = setShotCoordinatesCPU.Next(0, 2);
                bool carpetOrientationCPU = false;
                if(orientation == 0){
                    carpetOrientationCPU = true;
                }
                constructor.CarpetBomber(ref constructor.map, xC, yC, carpetOrientationCPU);
            }
        }
        public void UpdateShipStateCPU(int a, int b){
            Console.WriteLine($"Updating ship state for hit at ({b}, {a})");

            foreach (var ship in constructor.shipPositionsMap)
            {
                string shipName = ship.Key;
                List<(int, int)> shipPositions = ship.Value;

                if (shipPositions.Contains((a, b)))
                {
                    Console.WriteLine($"Marking position ({a}, {b}) of ship {shipName} as hit");
                    allHitCoordinates.Add((a, b));

                    for (int i = 0; i < shipPositions.Count; i++)
                    {
                        if (shipPositions[i] == (a, b))
                        {
                            for (int j = 0; j < constructor.shipState[shipName].GetLength(0); j++)
                            {
                                for (int k = 0; k < constructor.shipState[shipName].GetLength(1); k++)
                                {
                                    if (shipPositions[j * constructor.shipState[shipName].GetLength(1) + k] == (a, b))
                                    {
                                        constructor.shipState[shipName][j, k] = true; // Mark the position as hit
                                    }
                                }
                            }
                        }
                    }

                    if (IsShipSunkCPU(shipName))
                    {
                            Console.WriteLine($"Your ship {shipName} is sunk!");
                            constructor.sunkenShipCounterCPU++;
                            settings.remainingWeaponUsageCPU[6] = true;
                            settings.remainingWeaponUsageCPU[4] = true;
                    }

                    return;
                }
                else
                {
                    Console.WriteLine($"No hit at ({a}, {b}) for ship {shipName}");
                }
            }
        }

        private bool IsShipSunkCPU(string shipName){
            bool[,] shipCoordinates = constructor.shipState[shipName];
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
            foreach (var position in constructor.shipPositionsMap[shipName])
            {
                int y = position.Item1;
                int x = position.Item2;
                constructor.map[y, x] = 3; // Assign the value 3 to the map coordinates
            }
            return true; // Ship is completely sunk
        }
    }
}