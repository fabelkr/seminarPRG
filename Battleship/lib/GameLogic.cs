using System;
using System.ComponentModel;
using App.lib;
using App.lib.Computer;

namespace App.lib
{
    class GameLogic
    {
        GameSettings settings;
        GameConstructor constructor;

        public GameLogic(GameSettings settings)
        {
            this.settings = settings;
            constructor = new GameConstructor(settings);
        }
    }
}