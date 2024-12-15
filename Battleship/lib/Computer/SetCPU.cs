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
            //TODO: Validate if the weapon is in the weaponSpecifications
            //TODO: Fix the wepon charge limits
            bool valid = false;
            Random WeaponIndex = new Random();
            
            //rational
            if(constructor.empCPU){
                chosenWeaponCPU = settings.weaponNames[0];
            }
            else if(settings.difficulty == 1){
                //set strategic starting weapon for CPU
                if(constructor.turnIndex < 3){
                    int startingWeapon = WeaponIndex.Next(1, 3);
                    //missile
                    if(startingWeapon == 1){
                        chosenWeaponCPU = settings.weaponNames[1];
                    }
                    //Depth charge
                    else{
                        chosenWeaponCPU = settings.weaponNames[2];
                    }
                }
                else{
                    int usedWeaponIndex = WeaponIndex.Next(1, settings.weaponSpecifications.Count + 1);
                    while(!valid){
                        if (settings.remainingWeaponUsage[usedWeaponIndex])
                        {
                            valid = true;
                        }
                    }
                    chosenWeaponCPU = settings.weaponNames[usedWeaponIndex];
                }
            }
            SetShotCoordinatesCPU(chosenWeaponCPU);
        }

        public void SetShotCoordinatesCPU(string weaponType){
            bool valid = false;
            Random setShotCoordinatesCPU = new Random();

            while(!valid){
                xC = setShotCoordinatesCPU.Next(0, settings.mapWidth ?? 10);
                yC = setShotCoordinatesCPU.Next(0, settings.mapHeight ?? 10);

                if(constructor.map[xC, yC] == 4 || constructor.map[xC, yC] == 2 || constructor.map[xC, yC] == 3){
                }
                else if(settings.weaponSpecifications[weaponType][0] + xC > settings.mapWidth || settings.weaponSpecifications[weaponType][1] + yC > settings.mapHeight){
                }
                else{
                    valid = true;
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
    }
}