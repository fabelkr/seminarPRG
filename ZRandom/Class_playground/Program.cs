using System;
using App.lib;

namespace App{
    internal class Program{
        static void Duel(Player player, Enemy enemy){
            while (!player.IsDead() && !enemy.IsDead()){
                enemy.Hurt(player.GetRandomDamage());
                if (enemy.IsDead()){
                    Console.WriteLine("Player wins");
                    break;
                }
                player.Hurt(enemy.GetRandomDamage());

                Console.WriteLine("Player health: " + player.GetHealth());
                Console.WriteLine("Enemy health: " + enemy.GetHealth());
            }
            Console.WriteLine();
        }
        static void Main(string[] args){
            Player player = new Player(100, 10, "Player 1");
            Enemy enemy = new Enemy(20, 2, 1, "Enemy 1");

            Duel(player, enemy);

            Enemy enemy2 = new Enemy(20, 5, 2, "Enemy 2");

            Duel(player, enemy2);
        }
    }
}
