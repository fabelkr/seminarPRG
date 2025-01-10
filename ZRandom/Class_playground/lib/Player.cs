using System;

namespace App.lib{
    internal class Player{
        private string name;
        private int damage;
        private int health;
        private int score;
        private Random rng = new Random();

        public Player(int health, int damage, string name){
            SetHealth(health);
            this.name = name;
            this.damage = damage;
        }

        public void SetHealth(int value){
            health = value;
            if (health <= 0){
                health = 0;
                Console.WriteLine("Player is dead");
            }
        }
        public int GetHealth(){
            return health;
        }

        public int GetDamage(){
            return damage;
        }

        public int GetRandomDamage(){
            return rng.Next((int)(damage * 0.5f), (int)(damage * 1.5f));
        }

        public void Hurt(int damage){
            health -= damage;
            Console.WriteLine("Player got hit for " + damage + " damage");
            if (health <= 0){
                health = 0;
                Console.WriteLine("Player is dead");
            }
        }

        public bool IsDead(){
            return health <= 0;
        }
    }
}