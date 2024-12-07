
using System;
using System.ComponentModel;
using App.lib;
using App.lib.Computer;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            //creating class instances
            GameSettings setGame = new GameSettings();
            GameConstructor constructor = new GameConstructor(setGame);
            SetCPU CPU = new (setGame, constructor);

            setGame.SetNewGame();
            // Atomic.RenderZpevRyb();
        }
    }
}