using System;

namespace App.lib{
    internal class Enemy{
        private string name;
        private int damageBase;
        private int damage;
        private int healthBase;
        private int health;
        private int lvl;
        private int score;
        private Random rng = new Random();

        public Enemy(int health, int damage, int lvl, string name){
            healthBase = health;
            this.health = healthBase * lvl;

            damageBase = damage;
            this.damage = damageBase * lvl;

            this.name = name;
            this.lvl = lvl;
        }
        public int GetHealth(){
            return health;
        }

        public int GetDamage(){
            return damage;
        }

        public void Hurt(int damage){
            health -= damage;
            Console.WriteLine("Enemy got hit for " + damage + " damage");
            if (health <= 0){
                health = 0;
                Console.WriteLine("Enemy is dead");
            }
        }

        public bool IsDead(){
            return health <= 0;
        }

        public int GetRandomDamage(){
            return rng.Next((int)(damage * 0.5f), (int)(damage * 1.5f));
        }
    }
}