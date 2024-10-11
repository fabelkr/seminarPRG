using System;

namespace RPS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RPS_Game rpsGame = new RPS_Game();
            RPSLS_Game rpslsGame = new RPSLS_Game();
            GameSpecs gameSpecs = new GameSpecs();

            gameSpecs.setGameSpecs();
            // RPS_Game.rpsGame(gameSpecs);

            // Console.ReadKey(); //Aby se nam to hnedka neukoncilo
        }
    }
}

/*
    * Jednoduchy program na procviceni podminek a cyklu.
    * 
    * Vytvor program, kde bude uzivatel hrat s pocitacem kamen-nuzky-papir.
    * 
    * Struktura:
    * 
    * - nadefinuj promenne, ktere budes potrebovat po celou dobu hry, tedy skore obou, pripadne cokoliv dalsiho budes potrebovat
    *
    * Opakuj tolikrat, kolik kol chces hrat, nebo treba do doby, nez bude mit jeden z hracu pocet bodu nutnych k vyhre:
    * {
    *      Dokud uzivatel nezada vstup spravne:
    *      {
    *          - nacitej vstup od uzivatele
    *      }
    *      
    *      - vygeneruj s pomoci rng.Next() nahodny vstup pocitace
    *      
    *      Pokud vyhral uzivatel:
    *      {
    *          - informuj uzivatele, ze vyhral kolo
    *          - zvys skore uzivateli o 1
    *      }
    *      Pokud vyhral pocitac:
    *      {
    *          - informuj uzivatele, ze prohral kolo
    *          - zvys skore pocitaci o 1
    *      }
    *      Pokud byla remiza:
    *      {
    *          - informuj uzivatele, ze doslo k remize
    *      }
    * }
    * 
    * - informuj uzivatele, jake mel skore on/a a pocitac a kdo vyhral.
    */