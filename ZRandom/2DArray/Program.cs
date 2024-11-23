using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Made by Jan Borecky for PRG seminar at Gymnazium Voderadska, year 2024-2025.
 * Extended by students.
 */

namespace _2D_Array_Playground
{
    internal class Program
    {
        static void PrintArray(int[,] arrayToPrint)
        {
            for (int i = 0; i < arrayToPrint.GetLength(0); i++)
            {
                for (int j = 0; j < arrayToPrint.GetLength(1); j++)
                {
                    Console.Write(arrayToPrint[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n");
        }

        static void ResetArray(int[,] arrayToReset)
        {
            for (int i = 0; i < arrayToReset.GetLength(0); i++)
            {
                for (int j = 0; j < arrayToReset.GetLength(1); j++)
                {
                    arrayToReset[i, j] = i * 5 + j + 1;
                }
            }
        }

        static void Main(string[] args)
        {
            //TODO 1: Vytvoř integerové 2D pole velikosti 5 x 5, naplň ho čísly od 1 do 25 a vypiš ho do konzole (5 řádků po 5 číslech).
            int[,] array = new int[5, 5];
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = i * 5 + j + 1;
                    Console.Write(array[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n");

            //TODO 2: Vypiš do konzole n-tý řádek pole, kde n určuje proměnná nRow.
            int nRow = 0;

            for (int i = 0; i < array.GetLength(1); i++)
            {
                Console.Write(array[nRow, i] + " ");
            }
            Console.WriteLine("\n");

            //TODO 3: Vypiš do konzole n-tý sloupec pole, kde n určuje proměnná nColumn.
            int nColumn = 0;

            for (int i = 0; i < array.GetLength(0); i++)
            {
                Console.Write(array[i, nColumn] + " " + "\n");
            }
            Console.WriteLine("\n");

            //TODO 4: Prohoď prvek na souřadnicích [xFirst, yFirst] s prvkem na souřadnicích [xSecond, ySecond] a vypiš celé pole do konzole po prohození.
            //Nápověda: Budeš potřebovat proměnnou navíc, do které si uložíš první z prvků před tím, než ho přepíšeš druhým, abys hodnotou prvního prvku potom mohl přepsat druhý
            int xFirst, yFirst, xSecond, ySecond;
            xFirst = yFirst = 0;
            xSecond = ySecond = 4;

            int temp = array[xFirst, yFirst];
            array[xFirst, yFirst] = array[xSecond, ySecond];
            array[xSecond, ySecond] = temp;

            PrintArray(array);

            //TODO 5: Prohoď n-tý řádek v poli s m-tým řádkem (n je dáno proměnnou nRowSwap, m mRowSwap) a vypiš celé pole do konzole po prohození.
            int nRowSwap = 0;
            int mRowSwap = 1;

            for (int i = 0; i < array.GetLength(1); i++)
            {
                int tempSwapRow = array[nRowSwap, i];
                array[nRowSwap, i] = array[mRowSwap, i];
                array[mRowSwap, i] = tempSwapRow;
            }

            PrintArray(array);

            //TODO 6: Prohoď n-tý sloupec v poli s m-tým sloupcem (n je dáno proměnnou nColSwap, m mColSwap) a vypiš celé pole do konzole po prohození.
            int nColSwap = 0;
            int mColSwap = 1;

            for (int i = 0; i < array.GetLength(0); i++)
            {
                int tempSwapCol = array[i, nColSwap];
                array[i, nColSwap] = array[i, mColSwap];
                array[i, mColSwap] = tempSwapCol;
            }

            PrintArray(array);

            //TODO 7: Otoč pořadí prvků na hlavní diagonále (z levého horního rohu do pravého dolního rohu) a vypiš celé pole do konzole po otočení.

            ResetArray(array);

            for (int i = 0; i < 4; i++)
            {
                int LT_Corner = array[i, i];
                int RB_Corner = array[4 - i, 4 - i]; // Adjusted indices
                int tempDiag = LT_Corner;
                LT_Corner = RB_Corner;
                RB_Corner = tempDiag;

                array[i, i] = LT_Corner;
                array[4 - i, 4 - i] = RB_Corner; // Adjusted indices
            }

            PrintArray(array);

            //TODO 8: Otoč pořadí prvků na vedlejší diagonále (z pravého horního rohu do levého dolního rohu) a vypiš celé pole do konzole po otočení.
        }
    }
}
