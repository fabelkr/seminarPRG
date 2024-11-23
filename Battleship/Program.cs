
using System;
using System.ComponentModel;
using App.lib;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            //creating class instances
            GameSettings setGame = new GameSettings();

            setGame.SetNewGame();
        }
    }
}