using System;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using App.lib.RenderASCII;

namespace App.lib.Computer
{
    public class SetCPU
    {
        private GameSettings settings;
        private GameConstructor constructor;
        public int[,] mapCPU;

        // Constructor to initialize the settings and constructor objects
        public SetCPU(GameSettings settings, GameConstructor constructor)
        {
            this.settings = settings;
            this.constructor = constructor;
            mapCPU = new int[settings.mapHeight ?? 10, settings.mapWidth ?? 10];
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
                        PlaceCPUShipOnMap(x, y, orientation, shipWidth, shipLength);
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

        private void PlaceCPUShipOnMap(int x, int y, bool orientation, int shipWidth, int shipLength)
        {
            if (orientation) // Horizontal
            {
                for (int i = 0; i < shipWidth; i++)
                {
                    for (int j = 0; j < shipLength; j++)
                    {
                        mapCPU[y + i, x + j] = 1;
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
                    }
                }
            }
        }
    }
}