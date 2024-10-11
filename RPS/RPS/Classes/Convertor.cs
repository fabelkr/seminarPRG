using System;

namespace Calculator.Classes
{
    public class Convertor
    {

        public void decimalToBinary(int decimall) {
            Console.WriteLine("Decimal: " + decimall);
            int index = 0;
            char[] binary = new char[32];

            if (decimall == 0) {
                binary[index] = '0';
            } else {
                while (decimall > 0) {
                    binary[index] = (char)((decimall % 2) + '0');
                    index++;
                    decimall /= 2;
                }
            }
            
            Array.Reverse(binary, 0, index);

            string binaryStr = new string(binary, 0, index);

            Console.WriteLine("Binary: " + binaryStr + "\n");
        }

        public int binaryToDecimal(string binary){
            int decimalVal = Convert.ToInt32(binary, 2);
            Console.WriteLine("Decimal: " + decimalVal + "\n");
            return decimalVal;
        }

        public void decimalToHexadecimal(int decimax){
            string hex = decimax.ToString("X");
            Console.WriteLine("Hexadecimal: " + hex + "\n");
        }

        public int hexadecimalToDecimal(string hexadec){
            int decimall = Convert.ToInt32(hexadec, 16);
            Console.WriteLine("Decimal: " + decimall + "\n");
            return decimall;
        }

        public void binaryToHexadecimal(string binhex){
            int decimall = binaryToDecimal(binhex);
            string hex = decimall.ToString("X");
            Console.WriteLine("Hexadecimal: " + hex + "\n");
        }

        public void hexadecimalToBinary(string hexabin){
            int decimalVal = hexadecimalToDecimal(hexabin);
            decimalToBinary(decimalVal);
        }
    }
}