using System;
using System.ComponentModel;

namespace App.lib.RenderASCII
{
    public class Ships
    {
        public static string RenderASCIIYamato()
        {
            return @"                      
                                                                                    *                         
                                                                                _.--^--. 
                                                                         _.-- |     |                   
                                                                    _.-- |          |            .
                                                            _.-- |                  |     _. __--.-'  --.
                                                    _.-- |                        _.|-^  :   _,.;_   .  .
                                            _.- | ;                          ../   I |  . || _,.;_  .
                                        .--                              _.--       |  -:||
                                    .  -- |  ;                          I |_I____._\|__/___._._===-->
                                    _.----||   O___O                   :_.--^   :_]|______|    
                            |__.-- |      ||     |                     ;I H| |_______'(|)|
                            | :   '       | ;    |    ;                 |:I_H|_|______[ '._|    _.---.______
                        I   | ;  |        || ;__[_]__;                |||____________| '_|    \|___;/
_______.---._    ______ I  /|:  '     \   ||  ; |..|           || '-.b.      |_[_|_X__[|___:_,.-^>_.---.______
;___ :|\----/  I_/_|;| \:/\ __nm___nm   _d______||_||___b.__E33E    | I|__| m |_ m__|'^|  \|  ;    \___/
 ;      ______.---._<^-.,_____ .--_____nm____; |_dHH|_|.-|dHH|_|,-=|______|_I|__|_W_^__W_||_:___,.--._nnn__m__//_o
  :\         :   |/ |  |   __ m ___ .d88b. H m m || |_|  H*''| .mmmmmmmmm^^|_|' 7 | * *      *  *  *     .V.    ;
   :_\__,.,_n_m_;___|]_I[__]W_____Y88P_H_W_W_||_|__H&[]|_____^MMMM^______|W__H%$&$__I_____ -'________.-' ^(8)-    ;
    |<    H  * * *  * *  * *  *  * *  * * * * * * * *  *  *  *  *  *            * * * *      *  *  *   *  *      :
     |  _|_H_|_         ________________________________________YAMATO_____________________________               ;
      '------------------------------------------------------------------------------------------------------------'
    ";
        }

        public static string RenderASCIIIowa()
        {
            return @"
                       # #
                       #_#
                    _ |___| _              
       <- - - <-====| | | | | |====-> - - ->
         ===| |.----------------. | |=== 
  <-----------'   .  .  .  .  .  '-____________
   \    __:_____-----------------____:____    /
    \________________________________Iowa____/
    ";
        }

        public static string RenderASCIIBoominBeaver()
        {
            return @"
                        A       /`.
                _______AAA_____/`..`\
        <<-----UUUUUUUUUUU    / \`.__\
               | `.______________\
               | .|  __|__|____|| 
               |.-|  | _|____|_||
 <----|=====___|..|,--|_|____|_||.-==-.-;)
   \``---../_____ |______|,____||_____
  ~~\  ```````````---............    J ~~
 ~~~ \        BOOMIN'BEAVER          |  ~~~
 ~ ~~~`----...._____~~~~~~____~~~  __| ~~~
    ";
        }

        public static string RenderASCIISubmarine()
        {
            return @"
        _-   _-    o===7   _-    _-  _-   _-
    _-    _-    _-  -_||_    _-       _-    _-
        _-   _-    _-| _ |       _-   _-    _-
    _-    _-    _-  /_(_)_\        _-    _-       _-
                _.-'_______`-.___     _-  _-
            _..--`               `-..'_______. _-   _-
        _.-'  o/o                 o/o`-..__.'        ~  ~
    .-'    o                          ____/    ~ ~ ~
o--       o|o     -----------   o|o      `.._. // ~  ~
    `-._   o                  o|o      |||<|||~  ~ ~  ~
        `-.__o\o                 o|o     .'-'  \\ ~  ~
           `-.______Submarine____\_...-``'.       ~  ~
                                  `._______`.
    ";
        }

        public static string RenderASCIIDestroyer()
        {
            return @"
                       # #
                       #_#
                    _ |___| _              
             <-====| | | | | |====->
         ===| |.----------------. | |===
  <-----------'   .  .  .  .  .  '-____________
   \                                          /
    \________________________________Iowa____/
    ";
        }

        public static string RenderASCIICarrier()
        {
            return @"
                       # #
                       #_#
                    _ |___| _              
             <-====| | | | | |====->
         ===| |.----------------. | |===
  <-----------'   .  .  .  .  .  '-____________
   \                                          /
    \________________________________Iowa____/
    ";
        }

        public static string RenderASCIIBattleship()
        {
            return @"
                       # #
                       #_#
                    _ |___| _              
             <-====| | | | | |====->
         ===| |.----------------. | |===
  <-----------'   .  .  .  .  .  '-____________
   \                                          /
    \________________________________Iowa____/
    ";
        }

        public static string RenderASCIICruiser()
        {
            return @"
                       # #
                       #_#
                    _ |___| _              
             <-====| | | | | |====->
         ===| |.----------------. | |===
  <-----------'   .  .  .  .  .  '-____________
   \                                          /
    \________________________________Iowa____/
    ";
        }

        public static void CheckShipToRender(string shipName){
            if (shipName == "Yamato")
            {
                Console.WriteLine(RenderASCIIYamato());
            }
            else if (shipName == "Iowa")
            {
                Console.WriteLine(RenderASCIIIowa());
            }
            else if (shipName == "Boomin Beaver")
            {
                Console.WriteLine(RenderASCIIBoominBeaver());
            }
            else if (shipName == "Submarine")
            {
                Console.WriteLine(RenderASCIISubmarine());
            }
            else if (shipName == "Destroyer")
            {
                Console.WriteLine(RenderASCIIDestroyer());
            }
            else if (shipName == "Carrier")
            {
                Console.WriteLine(RenderASCIICarrier());
            }
            else if (shipName == "Battleship")
            {
                Console.WriteLine(RenderASCIIBattleship());
            }
            else if (shipName == "Cruiser")
            {
                Console.WriteLine(RenderASCIICruiser());
            }
        }

        public static string ReturnPseudoCarrier(){
            return "C ";
        }

        public static string ReturnPseudoBattleship(){
            return "T ";
        }

        public static string ReturnPseudoCruiser(){
            return "R ";
        }

        public static string ReturnPseudoDestroyer(){
            return "D ";
        }

        public static string ReturnPseudoSubmarine(){
            return "S ";
        }

        public static string ReturnPseudoBoominBeaver(){
            return "B ";
        }

        public static string ReturnPseudoIowa(){
            return "I ";
        }

        public static string ReturnPseudoYamato(){
            return "Y ";
        }

        public static string CheckPseudoToRender(string shipName){
            if (shipName == "Yamato")
            {
                return ReturnPseudoYamato();
            }
            else if (shipName == "Iowa")
            {
                return ReturnPseudoIowa();
            }
            else if (shipName == "Boomin Beaver")
            {
                return ReturnPseudoBoominBeaver();
            }
            else if (shipName == "Submarine")
            {
                return ReturnPseudoSubmarine();
            }
            else if (shipName == "Destroyer")
            {
                return ReturnPseudoDestroyer();
            }
            else if (shipName == "Carrier")
            {
                return ReturnPseudoCarrier();
            }
            else if (shipName == "Battleship")
            {
                return ReturnPseudoBattleship();
            }
            else if (shipName == "Cruiser")
            {
                return ReturnPseudoCruiser();
            }
            else
            {
                return "Unknown Ship";
            }
        }
    }
}