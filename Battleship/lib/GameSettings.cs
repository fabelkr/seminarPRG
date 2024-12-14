using System;
using System.ComponentModel;
using System.Data;
using App.lib.Computer;
using App.lib.RenderASCII;

namespace App.lib
{
    public class GameSettings
    {
        //All nescessary variables declaration
        static bool exit = false;
        public bool? mapType = true;
        public int? mapWidth = 10;
        public int? mapHeight = 10;
        public int? mapDiameter = 20;
        public int? difficulty = null;
        public System.ConsoleColor? colorTheme = null;

        public string[] weaponNames = [
            "Torpedo",
            "Missile",
            "Depth Charge",
            "Nuke",
            "EMP",
            "Scanner",
            "Carpet Bomber"
        ];

        int[,] weaponImpact;

        public string[] shipNames = [
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

        //Dictionary for ship specifications (name, size)
        public Dictionary<string, int[]> shipSpecifications;
        //Dictionary for weapon specifications (name, impact)
        public Dictionary<string, int[]> weaponSpecifications;

        //Constructor with ship specifications dictionary in it 
        public GameSettings()
        {
            shipSpecifications = new Dictionary<string, int[]>();
            weaponSpecifications = new Dictionary<string, int[]>();

            //Adding default ships to the dictionary (excluding Yamato, Iowa and Boomin Beaver)
            for (int i = 0; i < 5; i++)
            {
                shipSpecifications.Add(shipNames[i], new int[] { shipSizes[i, 0], shipSizes[i, 1] });
            }
        }

        //ridiculously long function for setting up the game
        public void SetNewGame(){
                Console.Clear();
                Console.WriteLine("Welcome to the Battleship Game!\n");

                weaponImpact = new int[,] {
                    {1, 1}, //normal
                    {1, 1}, //100% chance - random scan
                    {3, 3}, //normal
                    {5, 5}, //normal
                    {mapWidth ?? 0, mapHeight ?? 0}, //normal, prevents enemy from using high tech weapons and will be blinded
                    {3, 3}, //normal, scans area for enemy ships
                    {1, 2} //normal, drops bombs in a line
                };

                //Adding default weapons to the dictionary (excluding Scanner and Carpet Bomber, EMP and Nuke)
                for (int i = 0; i < 3; i++)
                {
                    weaponSpecifications.Add(weaponNames[i], new int[] { weaponImpact[i, 0], weaponImpact[i, 1] });
                }

                while (!exit)
                {
                
                //Definition on line 140
                DisplayMenu();

                switch (Console.ReadLine())
                {
                    case "1":
                        SelectMapType();
                        break;
                    case "2":
                        SelectDifficulty();
                        break;
                    case "3":
                        ModifyShips();
                        break;
                    case "4":
                        SelectColorTheme();
                        break;
                    case "5":
                        StartGame();
                        break;
                    case "6":
                        exit = true;
                        break;
                    case "7":
                        DisplayHelp();
                        break;
                    case "8":
                        DisplayShipInfo();
                        break;
                    case "9":
                        DisplayWeaponInfo();
                        break;
                    case "10":
                        ModifyWeapons();
                        break;
                    case "11":
                        RemoveShip();
                        break;
                    default:
                        Console.WriteLine("You selected an invalid option");
                        break;
                }
            }
        }

        //When called, this function will display the main menu
        private void DisplayMenu(){
                Console.ForegroundColor = ConsoleColor.DarkRed;
                ExcessGUI.RenderStartGUI();
                Console.ResetColor();
                //Console informations UI
                Console.WriteLine("\n\nSet new game before you start playing\n");
                //Basic settings UI
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Optional = white");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Obligatory = red");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Already set = green");
                Console.ResetColor();
                Console.WriteLine();

                DisplayMapOptions();
                DisplayDifficultyOptions();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("3. Add or modificate your ships");
                DisplayColorThemeOptions();
                if (mapType == null || difficulty == null || colorTheme == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.WriteLine("5. Start game");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("6. Exit game");
                Console.WriteLine("7. Help");
                Console.WriteLine("8. Get ship info");
                Console.WriteLine("9. Get weapon info");
                Console.WriteLine("10. Modify weapons");
                Console.WriteLine("11. Remove ship");

                Console.ResetColor();

                //Dev testing
                // Console.WriteLine(mapDiameter);
                // Console.WriteLine(mapWidth);
                // Console.WriteLine(mapHeight);
                // Console.WriteLine(difficulty);
                // Console.WriteLine(colorTheme);
                // CalculateMapArea();
        }

        //Ridiculously long mess, that creates a help section (I will propably include all of this in the readme file anyway)
        private void DisplayHelp()
        {
            Console.Clear();
            Console.WriteLine("=== HELP SECTION ===\n");
            Console.WriteLine("Welcome to the Battleship Game Setup!\n");
            Console.WriteLine("Here are the available options and their descriptions:\n");

            Console.WriteLine("- Map selection");
            Console.WriteLine("   - Choose the type and size of the map.");
            Console.WriteLine("   - Options: Rectangle (width x height) or Circle (diameter).\n");

            Console.WriteLine("- Difficulty selection");
            Console.WriteLine("   - Set the difficulty level of the game.");
            Console.WriteLine("   - Options: Normal (PC makes rational moves), Easy (PC makes random guesses), Demon (PC knows possition of all your ships).\n");

            Console.WriteLine("- Add or modify your ships");
            Console.WriteLine("   - Add new ships or modify existing ones.");
            Console.WriteLine("   - Note: Some ships are non-modifiable.\n");

            Console.WriteLine("- Select color theme");
            Console.WriteLine("   - Choose a color theme for displaying your ships.");
            Console.WriteLine("   - Options: Pink, Green, Yellow, Red.\n");

            Console.WriteLine("- Start game");
            Console.WriteLine("   - Start the game with the current settings.");
            Console.WriteLine("   - Note: All obligatory items must be set before starting the game.\n");

            Console.WriteLine("- Exit game");
            Console.WriteLine("   - Exit the game.\n");

            Console.WriteLine("- Help");
            Console.WriteLine("   - Display this help section.\n");

            Console.WriteLine("- Get ship info");
            Console.WriteLine("   - Display information about all added ships.\n");

            Console.WriteLine("- Get weapon info");
            Console.WriteLine("   - Display information about all added weapons.");
            Console.WriteLine("   - Closer explanation of the weapons:\n");
            Console.WriteLine("      - Torpedo: 1 X 1 - fires one projectile to a desired location - unlimited");
            Console.WriteLine("      - Missile: 1 X 1 - fires one shot from start/end of the row/column and hits the first ship in the row/column if there is any - every 5 turns recharges");
            Console.WriteLine("      - Depth Charge: 3 X 3 - fires a projectile to a desired location (epicentre) and hits all ships in the area - single use");
            Console.WriteLine("      - Nuke: 5 X 5 - fires a projectile to a desired location (epicentre) and hits all ships in the area - single use");
            Console.WriteLine("      - EMP: global impact - prevents enemy from using high tech weapons (Nuke, EMP, Scanner, Carpet Bomber) and will be blinded for 2 turns - recharges on kill");
            Console.WriteLine("      - Scanner: 3 X 3 - scans desired area for enemy ships - single use");
            Console.WriteLine("      - Carpet Bomber: 1 X 2 - drops bombs in a line - recharges on kill\n");

            Console.WriteLine("- Modify weapons");
            Console.WriteLine("   - Add or remove weapons.");
            Console.WriteLine("   - Note: Default weapons are non-removable.\n");

            Console.WriteLine("- Remove ship");
            Console.WriteLine("   - Remove a ship from the list.");
            Console.WriteLine("   - Note: You cant remove all of the ships\n");

            Console.WriteLine("=== END OF HELP SECTION ===");
            Atomic.GameSettingsError();
        }

        //function for rednering map options menu
        private void DisplayMapOptions(){
            if (mapType == true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"1. {mapWidth} X {mapHeight} {(mapWidth == mapHeight ? "square" : "rectangle")} (You can modify the size and shape of the map)");
            }
            else if (mapType == false)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"1. {mapDiameter} diameter circle");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("1. Map selection");
            }
        }

        //function for rendering difficulty options menu
        private void DisplayDifficultyOptions(){
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
        }


        //function for rendering color theme options menu
        private void DisplayColorThemeOptions(){
            if(colorTheme == ConsoleColor.Magenta){
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("4. Pink");
            }
            else if(colorTheme == ConsoleColor.Green){
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("4. Green");
            }
            else if(colorTheme == ConsoleColor.Yellow){
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("4. Yellow");
            }
            else if(colorTheme == ConsoleColor.Red){
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("4. Red (already set even though the text is red :)");
            }
            else{
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("4. Color theme selection");
            }
            Console.ResetColor();
        }


        //function for selecting map type
        private void SelectMapType(){
            Console.Write("\n1. Rectangle \n2. Circle\n");
            switch (Console.ReadLine())
            {
                case "1":
                    mapType = true;
                    Console.WriteLine("Enter a map width and height in this order");

                    mapWidth = Convert.ToInt32(Console.ReadLine());
                    mapHeight = Convert.ToInt32(Console.ReadLine());

                    if (mapHeight >= 50 || mapWidth >= 50)
                    {
                        Console.WriteLine("\nYou can not set a map with a dimension higher than 50");
                        mapHeight = 10;
                        mapWidth = 10;
                        Atomic.GameSettingsError();
                    }

                    //TEST
                    // Console.WriteLine("mw"+mapWidth);
                    // Console.WriteLine("mh"+mapHeight);
                    // Console.WriteLine(CalculateMapArea());

                    CompareMapAndShipsArea("map", "all ships", "mapWidth");
                    Console.Clear();
                    break;
                case "2":
                    mapType = false;
                    int? mapDiameterTemp = mapDiameter;
                    Console.WriteLine("Enter a map diameter");

                    mapDiameter = Convert.ToInt32(Console.ReadLine());

                    if (mapDiameter >= 50)
                    {
                        Console.WriteLine("\nYou can not set a map with a dimension higher than 50");
                        mapDiameter = 20;
                        Atomic.GameSettingsError();
                    }

                    CompareMapAndShipsArea("map size lower", "all ships", "mapDiameter", explicitDiameter: mapDiameterTemp);
                    CheckMapSizeValidity(mapDiameterTemp);
                    Console.Clear();
                    break;
                default:
                    Console.WriteLine("\nYou selected an invalid option");
                    break;
            }
        }


        //function for selecting difficulty
        private void SelectDifficulty()
        {
            Console.WriteLine("\n1. rational thinking PC \n2. random thinking PC \n3. hacking PC");
            switch (Console.ReadLine())
            {
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
        }


        //function for rendering modifying ships menu
        private void ModifyShips(){
            Console.WriteLine("\n1. Add ship \n2. Modify ship");
            switch(Console.ReadLine()){
                case "1":
                    AddShip();
                    break;
                case "2":
                    ModifyShip();
                    break;
                default:
                    Console.WriteLine("\nYou selected an invalid option");
                    break;
            }
        }

        //function that renders menu for adding ships
        private void AddShip()
        {
            Dictionary<int, string> indexToShipMap = new Dictionary<int, string>();
                int displayIndex = 1;

                for(int i = 0; i < 8; i++){
                    Console.WriteLine($"{displayIndex}. Name = {shipNames[i]}, Size = {shipSizes[i, 0]} X {shipSizes[i, 1]}");
                    indexToShipMap[displayIndex] = shipNames[i];
                    displayIndex++;
                }

                //TEST
                // foreach (var entry in indexToShipMap)
                // {
                //     Console.WriteLine($"Index: {entry.Key}, Ship: {entry.Value}");
                // }

                int selectedIndex;
                if (int.TryParse(Console.ReadLine(), out selectedIndex) && indexToShipMap.ContainsKey(selectedIndex))
                {
                    AddShipToSpecifications(indexToShipMap[selectedIndex]);
                }
                else
                {
                    Console.WriteLine("\nYou selected an invalid option");
                }
        }

        //function that adds desired ship to the dictionary based on the indexToShipMap[selectedIndex]
        private void AddShipToSpecifications(string shipType)
        {
            if (shipSpecifications.ContainsKey(shipType))
            {
                Console.WriteLine($"\nYou have already {shipType} included in the list");
                Atomic.GameSettingsError();
            }
            else
            {
                shipSpecifications.Add(shipType, new int[] { shipSizes[Array.IndexOf(shipNames, shipType), 0], shipSizes[Array.IndexOf(shipNames, shipType), 1] });
                CompareMapAndShipsArea("bigger total area of ships ", "map", "shipAddition", shipType);
            }
        }

        //function that renders menu for removing ships from the dictionary
        //It also targets the desired ship, which you want to remove from the dictionary with the selectedIndex and 
        private void RemoveShip(){
            {
                Dictionary<int, string> indexToShipMap = new Dictionary<int, string>();
                int displayIndex = 1;

                foreach (KeyValuePair<string, int[]> ship in shipSpecifications)
                {
                    Console.WriteLine($"{displayIndex}. Name = {ship.Key}, Size = [{string.Join(", ", ship.Value)}]");
                    indexToShipMap[displayIndex] = ship.Key;
                    displayIndex++;
                    Console.ResetColor();
                }

                //TEST
                // foreach (var entry in indexToShipMap)
                // {
                //     Console.WriteLine($"Index: {entry.Key}, Ship: {entry.Value}");
                // }
                // foreach (var entry in indexToShipMap)
                // {
                //     Console.WriteLine($"Index: {entry.Key}, Ship: {entry.Value}");
                // }

                int selectedIndex;
                if (int.TryParse(Console.ReadLine(), out selectedIndex) && indexToShipMap.ContainsKey(selectedIndex))
                {
                    RemoveShipFromSpecifications(indexToShipMap[selectedIndex]);
                }
                else
                {
                    Console.WriteLine("\nYou selected an invalid option");
                }
            }
        }

        //function that removes desired ship from the dictionary
        private void RemoveShipFromSpecifications(string shipType)
        {
            if(shipSpecifications.Count == 1){
                Console.WriteLine($"\nYou can not remove all ships");
                Atomic.GameSettingsError();
            }
            else if (shipSpecifications.ContainsKey(shipType))
            {
                shipSpecifications.Remove(shipType);
            }
            else
            {
                Console.WriteLine($"\nYou can't remove ships that are not included in the list ({shipType})");
                Atomic.GameSettingsError();
            }
        }

        //function that renders menu for modifying ships
        private void ModifyShip(){
            Console.WriteLine("\nChoose ship to modify\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("non-modifiable ships = red");
            Console.ResetColor();

            int shipIndex = 1;
            foreach (KeyValuePair<string, int[]> ship in shipSpecifications)
            {
                if(ship.Key == "Yamato" || ship.Key == "Iowa" || ship.Key == "Boomin Beaver"){
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                //Renders the ship info
                Console.WriteLine($"{shipIndex}. Name = {ship.Key}, Size = [{string.Join(", ", ship.Value)}]");
                shipIndex++;
                Console.ResetColor();
            }

            switch (Console.ReadLine())
            {
                case "1":
                    ModifyShipSize("Carrier");
                    break;
                case "2":
                    ModifyShipSize("Battleship");
                    break;
                case "3":
                    ModifyShipSize("Cruiser");
                    break;
                case "4":
                    ModifyShipSize("Destroyer");
                    break;
                case "5":
                    ModifyShipSize("Submarine");
                    break;
                case "6":
                    Console.WriteLine("\nYamato is non-modifiable");
                    break;
                case "7":
                    Console.WriteLine("\nIowa is non-modifiable");
                    break;
                case "8":
                    Console.WriteLine("\nBoomin Beaver is non-modifiable");
                    break;
                default:
                    Console.WriteLine("\nYou selected an invalid option");
                    break;
            }
        }

        //function that renders the sub-menu for modifying the ship size and modifies the ship size
        // It also checks if the new ship size meets the requirements (if it wouldnt overlap the map borders)
        private void ModifyShipSize(string shipType)
        {
            int index = Array.IndexOf(shipNames, shipType);
            int[] temp = new int[] { shipSizes[index, 0], shipSizes[index, 1] };

            Console.WriteLine($"\n{shipType} {temp[0]} X {temp[1]}");
            Console.WriteLine("\n1. Change width \n2. Change height");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Enter new width");
                    shipSizes[index, 0] = Convert.ToInt32(Console.ReadLine());
                    break;
                case "2":
                    Console.WriteLine("Enter new height");
                    shipSizes[index, 1] = Convert.ToInt32(Console.ReadLine());
                    break;
                default:
                    Console.WriteLine("\nYou selected an invalid option");
                    return;
            }

            shipSpecifications[shipType] = new int[] { shipSizes[index, 0], shipSizes[index, 1] };

            // Compare the new values with the requirements
            if (!CompareMapAndShipsLength(shipType) || !CompareMapAndShipsArea("smaller map area", "ships", "mapWidth", "", temp) || !CompareLogicalDimensions(shipType))
            {
                // If the new values do not meet the requirements, reassign the values from the temp variable
                //This part is for the map type rectangle/square
                if(mapType == true){
                    shipSizes[index, 0] = temp[0];
                    shipSizes[index, 1] = temp[1];
                    //Reasign the old value to the dictionary
                    shipSpecifications[shipType] = new int[] { temp[0], temp[1] };
                    Console.WriteLine($"\nThe new size of {shipType} does not meet the requirements (One dimension of your ship is bigger than both dimensions of the map, or both your ship dimensions are bigger, than the lower map dimension). Reverting to the previous size.");
                    Atomic.GameSettingsError();
                }
                //This part is for the map type circle (it is the same as the previous one, but it did not work without it, so dont touch it :D)
                else{
                    shipSizes[index, 0] = temp[0];
                    shipSizes[index, 1] = temp[1];
                    //Reasign the old value to the dictionary
                    shipSpecifications[shipType] = new int[] { temp[0], temp[1] };
                    Console.WriteLine($"\nThe new size of {shipType} does not meet the requirements (The diameter of the map is smaller than the length of your ship). Reverting to the previous size.");
                    Atomic.GameSettingsError();
                }
            }
            else
            {
                // If the new values meet the requirements, update the shipSpecifications dictionary
                shipSpecifications[shipType] = new int[] { shipSizes[index, 0], shipSizes[index, 1] };
            }
        }

        //function that sets the color theme
        private void SelectColorTheme()
        {
            Console.WriteLine("\n1. Pink \n2. Green \n3. Yellow \n4. Red");
            switch (Console.ReadLine())
            {
                case "1":
                    colorTheme = ConsoleColor.Magenta;
                    break;
                case "2":
                    colorTheme = ConsoleColor.Green;
                    break;
                case "3":
                    colorTheme = ConsoleColor.Yellow;
                    break;
                case "4":
                    colorTheme = ConsoleColor.Red;
                    break;
                default:
                    Console.WriteLine("\nYou selected an invalid option");
                    break;
            }
        }

        private SetCPU CPU;
        private void StartGame()
        {
            // Ensure that all obligatory items are set before starting the game
            if (mapType == null || difficulty == null || colorTheme == null)
            {
                Console.WriteLine("\nYou have to set all obligatory items before you start the game");
                if (difficulty == null)
                {
                    Console.WriteLine("Please select the difficulty:");
                    SelectDifficulty();
                }
                if (mapType == null)
                {
                    Console.WriteLine("Please select the map type:");
                    SelectMapType();
                }
                if (colorTheme == null)
                {
                    Console.WriteLine("Please select the color theme:");
                    SelectColorTheme();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                exit = true;
                GameConstructor start = new GameConstructor(this);
                CPU = new SetCPU(this, start);
                start.CreateNewGame();
            }
        }

        //function that renders the ship info
        private void DisplayShipInfo()
        {
            int shipIndex = 1;
            foreach (KeyValuePair<string, int[]> ship in shipSpecifications)
            {
                Console.WriteLine($"{shipIndex}. Name = {ship.Key}, Size = [{string.Join(", ", ship.Value)}]");
                shipIndex++;
                Console.ResetColor();
            }
            Atomic.GameSettingsError();
        }

        //function that renders the weapon info
        private void DisplayWeaponInfo()
        {
            int weaponIndex = 1;
            foreach (KeyValuePair<string, int[]> weapon in weaponSpecifications)
            {
                Console.WriteLine($"{weaponIndex}. Name = {weapon.Key}, Impact = [{string.Join(", ", weapon.Value)}]");
                weaponIndex++;
                Console.ResetColor();
            }
            Atomic.GameSettingsError();
        }

        //function that renders the weapon modification menu
        private void ModifyWeapons()
        {
            Console.WriteLine("\n1. Add weapons\n 2. Remove weapons");

            switch (Console.ReadLine())
            {
                case "1":
                    AddWeapon();
                    break;
                case "2":
                    RemoveWeapon();
                    break;
                default:
                    Console.WriteLine("\nYou selected an invalid option");
                    break;
            }
        }

        //function that renders weapons adding menu
        private void AddWeapon()
        {
            Console.WriteLine("\nIf you add weapons, they will be added to both teams and added weapons cannot be modified.\nYou can add each weapon only once.");
            Console.WriteLine($"\n1. EMP ({mapWidth} X {mapHeight}) \n2. Nuke (5 X 5) \n3. Scanner (3 X 3) \n4. Carpet Bomber (1 X 2)");
            switch (Console.ReadLine())
            {
                case "1":
                    AddWeaponToSpecifications("EMP");
                    break;
                case "2":
                    AddWeaponToSpecifications("Nuke");
                    break;
                case "3":
                    AddWeaponToSpecifications("Scanner");
                    break;
                case "4":
                    AddWeaponToSpecifications("Carpet Bomber");
                    break;
                default:
                    Console.WriteLine("\nYou selected an invalid option");
                    break;
            }
        }

        //function that adds desired weapon to the dictionary
        private void AddWeaponToSpecifications(string weaponType)
        {
            //Prevents adding the same weapon twice
            if (weaponSpecifications.ContainsKey(weaponType))
            {
                Console.WriteLine($"\nYou have already added {weaponType}");
                Atomic.GameSettingsError();
            }
            bool canAddWeapon = true;
            for (int i = 0; i < 6; i++)
            {
                if ((weaponImpact[Array.IndexOf(weaponNames, weaponType), 0] > mapWidth) || 
                    (weaponImpact[Array.IndexOf(weaponNames, weaponType), 1] > mapHeight) ||
                    (weaponImpact[Array.IndexOf(weaponNames, weaponType), 0] > mapDiameter) ||
                    (weaponImpact[Array.IndexOf(weaponNames, weaponType), 1] > mapDiameter)){
                    Console.WriteLine($"\nYou can not add {weaponType} because it is too big for the map (Do not add this wepon, or modify the size of map to add this weapon)");
                    Atomic.GameSettingsError();
                    canAddWeapon = false;
                    break;
                }
            }
            if (canAddWeapon)
            {
                weaponSpecifications.Add(weaponType, new int[] { weaponImpact[Array.IndexOf(weaponNames, weaponType), 0], weaponImpact[Array.IndexOf(weaponNames, weaponType), 1] });
            }
            else
            {
                Console.WriteLine("\nOops, something went wrong");
                Atomic.GameSettingsError();
            }
        }

        //function that renders the sub-menu for removing the weapon
        private void RemoveWeapon()
        {
            Console.WriteLine("\nChoose weapon to remove\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("non-removable weapons = red");
            Console.ResetColor();

            int weaponIndex = 1;
            foreach (KeyValuePair<string, int[]> weapon in weaponSpecifications)
            {
                if(weapon.Key == "Missile" || weapon.Key == "Torpedo" || weapon.Key == "Depth Charge"){
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.WriteLine($"{weaponIndex}. Name = {weapon.Key}, Impact = [{string.Join(", ", weapon.Value)}]");
                weaponIndex++;
                Console.ResetColor();
            }

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("\nTorpedo is non-removeable");
                    Atomic.GameSettingsError();
                    break;
                case "2":
                    Console.WriteLine("\nMissile is non-removeable");
                    Atomic.GameSettingsError();
                    break;
                case "3":
                    Console.WriteLine("\nDepth Charge is non-removeable");
                    Atomic.GameSettingsError();
                    break;
                case "4":
                    RemoveWeaponFromSpecifications("Nuke");
                    break;
                case "5":
                    RemoveWeaponFromSpecifications("EMP");
                    break;
                case "6":
                    RemoveWeaponFromSpecifications("Scanner");
                    break;
                case "7":
                    RemoveWeaponFromSpecifications("Carpet Bomber");
                    break;
                default:
                    Console.WriteLine("\nYou selected an invalid option");
                    break;
            }
        }

        //function that removes desired weapon from the dictionary
        private void RemoveWeaponFromSpecifications(string weaponType)
        {
            if (weaponSpecifications.ContainsKey(weaponType))
            {
                weaponSpecifications.Remove(weaponType);
            }
            else
            {
                Console.WriteLine($"\nYou can't remove weapons that are not included in the list ({weaponType})");
                Atomic.GameSettingsError();
            }
        }

        //function that calculates the map area
        private int CalculateMapArea()
        {
            //If the map is a rectangle/square
            if (mapType == true)
            {
                return (mapWidth ?? 0) * (mapHeight ?? 0);
            }
            //If the map is a circle
            else
            {
                return (int)Math.Pow(mapDiameter ?? 0, 2) / 4 * 3;
            }
        }

        //function that calculates the total area of all ships together
        private int CalculateShipsArea()
        {
            int shipsArea = 0;
            foreach (KeyValuePair<string, int[]> ship in shipSpecifications)
            {
                int area = ship.Value[1] * ship.Value[0];
                shipsArea += ship.Value[0] * ship.Value[1];
                //TESTING
                // Console.WriteLine($"Ship: {ship.Key}, Area: {area}, Total Area: {shipsArea}");
            }
            return shipsArea;
        }

        //function that compares the map dimensions with the ship dimensions
        private bool CompareMapAndShipsLength(string shipType)
        {
            int index = Array.IndexOf(shipNames, shipType);
            int shipWidth = shipSizes[index, 0];
            int shipHeight = shipSizes[index, 1];
            int mapW = mapWidth ?? 0;
            int mapH = mapHeight ?? 0;
            //TESTING
            // Console.WriteLine(!((shipWidth > mapW && shipWidth > mapH) || (shipHeight > mapW && shipHeight > mapH)));
            // Return false only if the ship's width or height is greater than both the map's width and height (because the player will be able to rotate the ship later on in the game)
            return !((shipWidth > mapW && shipWidth > mapH) || (shipHeight > mapW && shipHeight > mapH));
        }

        //function that compares the map area with the total area of all ships
        private bool CompareMapAndShipsArea(string setter, string getter, string setterType, string? shipType = null, int[]? explicitWH = null, int? explicitDiameter = null)
        {
            if (explicitWH == null)
            {
                explicitWH = new int[] { 10, 10 };
            }

            if (CalculateMapArea() < CalculateShipsArea())
            {
                if(mapType == false){
                    mapDiameter = explicitDiameter;
                }
                Console.WriteLine($"\nYou can not set a {setter} than the total area of {getter}");
                Atomic.GameSettingsError();
                return false;
                mapHeight = mapDiameter;
                mapWidth = mapDiameter;
            }
            return true;
        }
        //TODO: ship cannot have size in any direction less than 1
        //function that compares the ship dimensions with the map dimensions (It is similar as the CompareMapAndShipsLength function, but it is used for the map type circle and it works, so dont worry about it)
        private bool CompareLogicalDimensions(string shipType)
        {
            int? mapLower = mapWidth < mapHeight ? mapWidth : mapHeight;
            int index = Array.IndexOf(shipNames, shipType);
            int shipWidth = shipSizes[index, 0];
            int shipHeight = shipSizes[index, 1];
            //TESTING
            // Console.WriteLine(!((shipWidth > mapLower) && (shipHeight > mapLower)));
            return !((shipWidth > mapLower) && (shipHeight > mapLower));
        }

        //function that checks if the map size is valid (if the map is big enough for all ships)
        private bool CheckMapSizeValidity(int? mapDiameterTemp)
        {
            if (mapType == false)
            {
                foreach (KeyValuePair<string, int[]> ship in shipSpecifications)
                {
                        if (ship.Value[0] > mapDiameter || ship.Value[1] > mapDiameter)
                        {
                            mapDiameter = mapDiameterTemp;
                            Console.WriteLine($"\nYou can not set a ship bigger than the map diameter");
                            Atomic.GameSettingsError();
                            return false;
                        }
                }
            }
            return (mapDiameter ?? 0) >= CalculateShipsArea();
        }
    }
}