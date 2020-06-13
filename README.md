# Big-Ints-NET
Types for big integers based on bit arrays providing up to 16384 bits, developed for Microsoft .NET core

This project is based on a former version written in C++. Besides the translation I did some further improvements to optimize the reuse of
code. The final types are based on an abstract and generic base class, and all of them implement an interface which includes the internal
BitArray. The types available provide 128, 256, ..., 16384 bits to store numbers with up to 4932 decimal digits.

The types give the following arithmetic methods to their objects
-Add
-Subtract
-Multiply
-Divide
-Modulo
-Factorial
-Logarithm
-Square root

Caution:
The code has not been tested, errors could occure
I will do some tests, and tell you when all of them succeded

07/06/2020:
The constuctors work as expected now, some files have been updated

13/06/2020
Large parts of the arithmetic functions have been tested, the private member of BigIntsBase.cs 'Internal_Log' failed,
sqrt has not been tested already.

Updates in future might come, but not in the next weeks
