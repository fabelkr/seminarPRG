# Calculator

You can count with this thing. My calculator respects the arithmetics operators precedence, which is quite useful. At this moment, this app can count with four operators basic, which are "+, -, *, /", but I surelly want to add more functions. My app allows you to return to your last result and work with it, by typing "ans" or "ANS" is ok too, if you forgot to turn off your capslock . My calculator project also checks if you dont divide by 0 and in that case it throws a message and returns you to entering the mathematical problem. You can access the PI number by typing "pi" or "PI" instead of numer.

In this calculator, you can use parentheses and it doesnt matter, how many of them you use and also it doesnt matter how you use them...
f.e: "(x+y*y)+(y-z/x)/(x+y+z)" or "x+y*(x+y*(y-x))"
In the console, you can also see the whole process, how the expression with parentheses is being solved, which is pretty interesting and looks cool.

This calculator can also convert throughuout decimal, hexadecimal and binary numer formates, which can be usefull for example if you want to know color hex code and you know just its rgb value and more. (Only non-negative numbers can be converted)

If you wanted to see the proscess, how I was making myself sure if the app works, or not, I recommend you to uncomment all of the commented "//service checkups" code. It may help understanding how this app works.

Also I have one or two "TODO:" in my code. This is the code, I used before but then I optimised it, or simply did not want it. 

## Table of Contents

- [Inputs](#inputs)
- [Contact](#contact)
- [Features](#features)
- [Side features](#side-features)

## Inputs

When you dotnet run the project, press "1" or "2" to continue, then my app asks you politely to enter a math expression you want to be solved.

Enter your math problem in a correct formate, which is: your number in decimal formate followed by a mathematical operator ("+, -, *, /"). It doesnt matter, if you type the input with or without spaces. f.e.: 10 * 10 + 2

Also you can explicitly enter "ans" or "ANS" instead of any number and the app will work with it as you entered the last result of your mathematical problem (if you havent entered any math problem yet, and you tried to use "ans", it will count with ans as if it was 0(so be careful when multiplying, as it could corrupt your result))... f.e.: ans + 2

Enter "pi" or "PI" as your input, to work with number PI.

If you pressed "2" in the start, you are going to convert numbers. I dont think it is nescessary to help with input, bacause it really speaks for itself.

## Features

- Counting with parentheses
- Unlimited number of inputs (both numbers and operators)
- Accessible last result via "ans" input
- PI -> input == "pi"
- Counting with negative numbers
- Converts numbers throughout three number formates (hexadecimal, binary, decimal)

## Side features

- It doesnt matter whether you enter the expression with or without spaces or tabs
- The final result is $${\color{green}Green}$$
- It doesnt matter, if you use "ans", "ANS", "aNS" (the inputs simply are not case       sensitive) -> the same goes for "pi" input
- I tried to learn and use README.md file...So mabey +1 point?? :D



```bash
# Clone the repository
git clone https://github.com/fabelkr/Calculator.git