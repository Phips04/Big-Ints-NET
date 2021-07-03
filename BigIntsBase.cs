using System;
using System.Collections;
using System.Collections.Generic;

namespace Big_Ints_.NET
{
    public abstract class BigIntsBase<T> : IBigInts.BigIntsBaseConstraints,
        IEquatable<BigIntsBase<T>> where T : IBigInts.BigIntsBaseConstraints, IBigInts, new()
    {
        #region Constructors
        public BigIntsBase()
        {
            Data = new BitArray(Length, false);
        }

        public BigIntsBase(string Bits)
        {
            Data = StringToBitArray(Bits, Length);
        }

        public BigIntsBase(BitArray Arg)
        {
            BitArray NewArg;

            //Data = Arg;
            if (Arg.Length != 0 && Arg[^1] == false)
            {
                int UnsignificantBits = 0;
                while(Arg[Arg.Length - UnsignificantBits - 1] == false)
                {
                    UnsignificantBits++;
                    if (Arg.Length - UnsignificantBits == 0)
                    {
                        Data = new BitArray(Length, false);
                        return;
                    }
                }

                NewArg = new BitArray(Arg.Length - UnsignificantBits, false);
                for (int i = 0; i < Arg.Length - UnsignificantBits; i++)
                {
                    NewArg[i] = Arg[i];
                }
            }
            else
            {
                NewArg = Arg;
            }

            if (NewArg.Length > Length)
            {
                throw new ArgumentException("The BitArray received was to big");
            }

            Data = new BitArray(Length, false);

            for (int i = 0; i < NewArg.Length; i++)
            {
                Data[i] = NewArg[i];
            }
        }

        public BigIntsBase(int Arg)
        {
            if (Arg < 0)
            {
                throw new ArgumentException($"Uint{Length} is unsigned, the constructor received a negative value");
            }
            else
            {
                Data = Uint64ToBitArray((ulong)Arg, Length);
            }
        }

        public BigIntsBase(ulong Arg)
        {
            Data = Uint64ToBitArray(Arg, Length);
        }

        public BigIntsBase(IBigInts.BigIntsBaseConstraints Arg)
        {
            Data = new BitArray(Length, false);

            for (int i = 0; i < Arg.Length; i++)
            {
                if (Arg.Data[Arg.Length - i - 1] == true)
                {
                    if (Arg.Length - i > Length)
                    {
                        throw new InvalidCastException("The number provided can not be represented by the current type");
                    }
                    else
                    {
                        for (int x = 0; x < Arg.Length - i; x++)
                        {
                            Data[x] = Arg.Data[x];
                        }
                    }

                    return;
                }
            }
        }
        #endregion

        #region Different helpers
        private static BitArray Uint64ToBitArray(ulong Arg, int Length)
        {
            BitArray result = new BitArray(Length, false);

            for (int i = 63; i > -1; i--)
            {
                if (Arg >= Math.Pow(2, i))
                {
                    result[i] = true;
                    Arg -= (ulong)Math.Pow(2, i);
                }
                else
                {
                    result[i] = false;
                }
            }

            return result;
        }

        private static ulong TryToUlong_(BitArray Arg)
        {
            ulong result = 0;

            if (FirstIsSmaller(Uint64ToBitArray(ulong.MaxValue, Arg.Length), Arg))
            {
                throw new ArgumentException("Arithmetic overflow");
            }

            for (int i = 0; i < 64; i++)
            {
                if (Arg[i] == true)
                {
                    result += (ulong)Math.Pow(2, i);
                }
            }

            return result;
        }

        private static BitArray StringToBitArray(string Arg, int Length)
        {
            BitArray Result = new BitArray(Length, false);

            if (Arg.Length != 0)
            {
                while (Arg[0] == '0')
                {
                    Arg = Arg.Remove(0, 1);
                    if (Arg.Length == 0)
                    {
                        break;
                    }
                }
            }

            if (Arg.Length >= Length)
            {
                throw new ArgumentException($"The method received mor than {Length} digits");
            }

            for (int i = 0; i < Arg.Length; i++)
            {
                if (Arg[Arg.Length - 1 - i] == '1')
                {
                    Result[i] = true;
                }
                else if(Arg[Arg.Length - 1 - i] == '0')
                {
                    Result[i] = false;
                }
                else
                {
                    throw new ArgumentException("The provided argument included characters which were not either '0' or '1'");
                }
            }

            return Result;
        }

        private static bool FirstIsSmaller(BitArray First, BitArray Second)
        {
            for (int i = 0; i < First.Length; i++)
            {
                if (First[First.Length - 1 - i] && !Second[Second.Length - 1 - i])
                {
                    return false;
                }
                else if (!First[First.Length - 1 - i] && Second[Second.Length - 1 - i])
                {
                    return true;
                }
            }

            return false;
        }

        private static bool CompareBitArrays(BitArray First, BitArray Second)
        {
            for (int i = 0; i < First.Length; i++)
            {
                if (First[i] != Second[i])
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Arithmetics center
        #region Addition
        private static BitArray Internal_Add(BitArray Arg1, BitArray Arg2)
        {
            BitArray result = new BitArray(Arg1.Length, false);

            byte Reminder = 0;

            for (int i = 0; i < Arg1.Length; i++)
            {
                if (Arg1[i] == true && Arg2[i] == true && Reminder == 1)
                {
                    result[i] = true;
                    if (i == Arg1.Length)
                    {
                        throw new StackOverflowException($"The result will grow larger than 2^{Arg1.Length}, which is the maximal value of uint128");
                    }
                    Reminder = 1;
                    continue;
                }

                if ((Arg1[i] == true && Arg2[i] == true && Reminder == 0) ||
                    (Arg1[i] == true && Arg2[i] == false && Reminder == 1) ||
                    (Arg1[i] == false && Arg2[i] == true && Reminder == 1))
                {
                    result[i] = false;
                    if (i == Arg1.Length - 1)
                    {
                        throw new StackOverflowException($"The result will grow larger than 2^{Arg1.Length}, which is the maximal value of uint128");
                    }
                    Reminder = 1;
                    continue;
                }
                if ((Arg1[i] == true && Arg2[i] == false && Reminder == 0) ||
                    (Arg1[i] == false && Arg2[i] == true && Reminder == 0) ||
                    (Arg1[i] == false && Arg2[i] == false && Reminder == 1))
                {
                    result[i] = true;
                    Reminder = 0;
                    continue;
                }
                else
                {
                    result[i] = false;
                    Reminder = 0;
                    continue;
                }
            }

            return result;
        }

        public T Add(ulong Arg)
        {
            T ReturnValue = new T
            {
                Data = Internal_Add(Data, Uint64ToBitArray(Arg, Length))
            };

            return ReturnValue;
        }

        public T Add(BigIntsBase<T> First, params BigIntsBase<T>[] AdditionalArgs)
        {
            T ResultValue = new T
            {
                Data = Internal_Add(Data, First.Data)
            };

            for (int i = 0; i < AdditionalArgs.Length; i++)
            {
                ResultValue.Data = Internal_Add(ResultValue.Data, AdditionalArgs[i].Data);
            }

            return ResultValue;
        }
        #endregion

        #region Subtraction
        private static BitArray Internal_Subtract(BitArray Arg1, BitArray Arg2)
        {
            BitArray result = new BitArray(Arg1.Length, false);

            int Reminder = 0;

            if (FirstIsSmaller(Arg1, Arg2))
            {
                throw new ArithmeticException($"The result will be negative, Uint{Arg1.Length} is unsigned");
            }

            for (int i = 0; i < Arg1.Length; i++)
            {

                if ((Arg1[i] == true && Arg2[i] == true && Reminder == 1) ||
                    (Arg1[i] == false && Arg2[i] == true && Reminder == 0) ||
                    (Arg1[i] == false && Arg2[i] == false && Reminder == 1))
                {
                    result[i] = true;
                    Reminder = 1;
                    continue;
                }
                if ((Arg1[i] == true && Arg2[i] == true && Reminder == 0) ||
                    (Arg1[i] == true && Arg2[i] == false && Reminder == 1) ||
                    (Arg1[i] == false && Arg2[i] == false && Reminder == 0))
                {
                    result[i] = false;
                    Reminder = 0;
                    continue;
                }

                if (Arg1[i] == false && Arg2[i] == true && Reminder == 1)
                {
                    result[i] = false;
                    Reminder = 1;
                    continue;
                }

                if (Arg1[i] == true && Arg2[i] == false && Reminder == 0)
                {
                    result[i] = true;
                    Reminder = 0;
                    continue;
                }
            }

            return result;
        }

        public T Subtract(ulong Arg)
        {
            T ReturnValue = new T
            {
                Data = Internal_Subtract(Data, Uint64ToBitArray(Arg, Length))
            };

            return ReturnValue;
        }

        public T Subtract(BigIntsBase<T> First, params BigIntsBase<T>[] AdditionalArgs)
        {
            T ResultValue = new T
            {
                Data = Internal_Subtract(Data, First.Data)
            };

            for (int i = 0; i < AdditionalArgs.Length; i++)
            {
                ResultValue.Data = Internal_Subtract(ResultValue.Data, AdditionalArgs[i].Data);
            }

            return ResultValue;
        }
        #endregion

        #region Multiplication
        private static BitArray Internal_Multiply(BitArray Arg1, BitArray Arg2)
        {
            List<BitArray> ItemsToAdd = new List<BitArray>();

            int ThisLength = 0;
            int InitLength = 0;
            bool ThisLengthFound = false;
            bool InitLengthFound = false;

            for (int i = 0; i < Arg1.Length; i++)
            {
                if (Arg1[Arg1.Length - 1 - i] == true && ThisLengthFound == false)
                {
                    ThisLength = Arg1.Length - i;
                    ThisLengthFound = true;
                }

                if (Arg2[Arg1.Length - 1 - i] == true && InitLengthFound == false)
                {
                    InitLength = Arg1.Length - i;
                    InitLengthFound = true;
                }

                if (ThisLengthFound && InitLengthFound)
                {
                    if (ThisLength + InitLength - 1 > Arg1.Length)
                    {
                        throw new ArithmeticException($"The result will grow larger than 2^{Arg1.Length}, which is the maximal value of Uint{Arg1.Length}");
                    }
                }
            }

            for (int i = 0; i < Arg1.Length; i++)
            {
                if (Arg2[i] == true)
                {
                    BitArray item = new BitArray(Arg1.Length, false);
                    for (int x = 0; x < Arg1.Length; x++)
                    {
                        if (x < i)
                        {
                            item[x] = false;
                        }
                        else
                        {
                            item[x] = Arg1[x - i];
                        }
                    }

                    ItemsToAdd.Add(item);
                }
            }

            BitArray result = new BitArray(Arg1.Length, false);

            foreach (var item in ItemsToAdd)
            {
                result = Internal_Add(result, item);
            }

            return result;
        }

        public T Multiply(ulong Arg)
        {
            T ReturnValue = new T
            {
                Data = Internal_Multiply(Data, Uint64ToBitArray(Arg, Length))
            };

            return ReturnValue;
        }

        public T Multiply(BigIntsBase<T> First, params BigIntsBase<T>[] AdditionalArgs)
        {
            T ResultValue = new T
            {
                Data = Internal_Multiply(Data, First.Data)
            };

            for (int i = 0; i < AdditionalArgs.Length; i++)
            {
                ResultValue.Data = Internal_Multiply(ResultValue.Data, AdditionalArgs[i].Data);
            }

            return ResultValue;
        }
        #endregion

        #region Division
        private static BitArray Internal_Divide(BitArray Arg1, BitArray Arg2)
        {
            if (CompareBitArrays(Arg2, new BitArray(Arg2.Length, false)))
            {
                throw new ArithmeticException("Argument was '0', division by '0' is not defined");
            }

            if (FirstIsSmaller(Arg1, Arg2))
            {
                return new BitArray(Arg1.Length, false);
            }

            string ThisStream = "";
            string InitStream = "";
            string ThisCurrentStream = "1";
            string ResultStream = "";
            bool StartRecordingThis = false;
            bool StartRecordingInit = false;

            for (int i = 0; i < Arg1.Length; i++)
            {
                if (Arg1[Arg1.Length - 1 - i] == true)
                {
                    ThisStream += "1";
                    StartRecordingThis = true;
                }
                else if (StartRecordingThis == true)
                {
                    ThisStream += "0";
                }

                if (Arg2[Arg1.Length - 1 - i] == true)
                {
                    InitStream += "1";
                    StartRecordingInit = true;
                }
                else if (StartRecordingInit == true)
                {
                    InitStream += "0";
                }
            }

            ThisStream = ThisStream.Remove(0, 1);

            while (ThisStream.Length > 0)
            {

                if (FirstIsSmaller(StringToBitArray(ThisCurrentStream, Arg1.Length), StringToBitArray(InitStream, Arg1.Length)))
                {
                    ThisCurrentStream += ThisStream[0];
                    ThisStream = ThisStream.Remove(0, 1);
                    ResultStream += "0";
                }
                else
                {
                    BitArray CThis = StringToBitArray(ThisCurrentStream, Arg1.Length);
                    BitArray CInit = StringToBitArray(InitStream, Arg1.Length);

                    BitArray RawResult = Internal_Subtract(CThis, CInit);

                    string result = "";
                    bool StartWritingZeros = false;

                    for (int i = 0; i < Arg1.Length; i++)
                    {
                        if (RawResult[Arg1.Length - 1 - i] == true)
                        {
                            result += "1";
                            StartWritingZeros = true;
                        }
                        if (RawResult[Arg1.Length - 1 - i] == false && StartWritingZeros == true)
                        {
                            result += "0";
                        }
                    }

                    ThisCurrentStream = result;
                    ThisCurrentStream += ThisStream[0];
                    ThisStream = ThisStream.Remove(0, 1);
                    ResultStream += "1";
                }
            }
            if (FirstIsSmaller(StringToBitArray(ThisCurrentStream, Arg1.Length), StringToBitArray(InitStream, Arg1.Length)))
            {
                ResultStream += "0";
            }
            else
            {
                ResultStream += "1";
            }

            while (ResultStream[0] == '0')
            {
                ResultStream = ResultStream.Remove(0, 1);
            }

            return StringToBitArray(ResultStream, Arg1.Length);
        }

        public T Divide(ulong Arg)
        {
            T ReturnValue = new T
            {
                Data = Internal_Divide(Data, Uint64ToBitArray(Arg, Length))
            };

            return ReturnValue;
        }

        public T Divide(BigIntsBase<T> First, params BigIntsBase<T>[] AdditionalArgs)
        {
            T ResultValue = new T
            {
                Data = Internal_Divide(Data, First.Data)
            };

            for (int i = 0; i < AdditionalArgs.Length; i++)
            {
                ResultValue.Data = Internal_Divide(ResultValue.Data, AdditionalArgs[i].Data);
            }

            return ResultValue;
        }
        #endregion

        #region Power
        private static BitArray Internal_Pow(BitArray Base, BitArray exp)
        {
            BitArray result = new BitArray(Base.Length, false);
            result[0] = true;

            if (exp == new BitArray(exp.Length, false))
            {
                return result;
            }



            for (BitArray i = new BitArray(Base.Length, false); FirstIsSmaller(i, exp); i = Internal_Add(i, Uint64ToBitArray(1, exp.Length)))
            {
                result = Internal_Multiply(result, Base);
            }

            return result;
        }

        public T Pow(ulong Arg)
        {
            T ReturnValue = new T
            {
                Data = Internal_Pow(Data, Uint64ToBitArray(Arg, Length))
            };

            return ReturnValue;
        }

        public T Pow(BigIntsBase<T> First, params BigIntsBase<T>[] AdditionalArgs)
        {
            T ResultValue = new T
            {
                Data = Internal_Pow(Data, First.Data)
            };

            for (int i = 0; i < AdditionalArgs.Length; i++)
            {
                ResultValue.Data = Internal_Pow(ResultValue.Data, AdditionalArgs[i].Data);
            }

            return ResultValue;
        }
        #endregion

        #region Modulo
        private static BitArray Internal_Mod(BitArray This, BitArray Arg)
        {
            BitArray Result = Internal_Subtract(This, Internal_Multiply(Internal_Divide(This, Arg), Arg));
            return Result;
        }

        public T Mod(ulong Arg)
        {
            T Result = new T
            {
                Data = Internal_Mod(Data, Uint64ToBitArray(Arg, Data.Length))
            };
            return Result;

        }

        public T Mod(BigIntsBase<T> First, params BigIntsBase<T>[] AdditionalArgs)
        {
            T Result = new T
            {
                Data = Internal_Mod(Data, First.Data)
            };

            for (int i = 0; i < AdditionalArgs.Length; i++)
            {
                Result.Data = Internal_Mod(Result.Data, AdditionalArgs[i].Data);
            }

            return Result;
        }
        #endregion

        #region Logarithm
        private static BitArray Internal_Log(BitArray Powres, BitArray Base)
        {
            if (CompareBitArrays(Base, new BitArray(Base.Length, false)))
            {
                throw new ArgumentException("Argument was '0', logarithm which base = '0' is not defined");
            }

            if (CompareBitArrays(Base, Uint64ToBitArray(1, Base.Length)))
            {
                throw new ArithmeticException("Base was '1', logarithm which base is 1 is not defined");
            }

            if (CompareBitArrays(Powres, new BitArray(Powres.Length, false)))
            {
                throw new ArgumentException("The value of the object calling the function 'log(uint128 base)'" +
                    " was '0', logarithm with exponent = '0' is not defined");
            }

            if (FirstIsSmaller(Powres, Base))
            {
                return new BitArray(Powres.Length, false);
            }

            ulong CurrentPosition =  (ulong)Base.Length / 2;
            ulong CurrentOffsetLength = (ulong)Base.Length / 4;

            while (true)
            {
                try
                {
                    if (CompareBitArrays(Powres, Internal_Pow(Base, Uint64ToBitArray(CurrentPosition, Base.Length)))
                        || (FirstIsSmaller(Internal_Pow(Base, Uint64ToBitArray(CurrentPosition, Base.Length)), Powres)
                        && FirstIsSmaller(Powres, Internal_Pow(Base, Uint64ToBitArray(CurrentPosition + 1, Base.Length)))))
                    {
                        return Uint64ToBitArray(CurrentPosition, Base.Length);
                    }
                    if (FirstIsSmaller(Powres, Internal_Pow(Base, Uint64ToBitArray(CurrentPosition, Base.Length))))
                    {
                        CurrentPosition -= CurrentOffsetLength;
                        CurrentOffsetLength /= 2;
                        continue;
                    }
                    if (FirstIsSmaller(Internal_Pow(Base, Uint64ToBitArray(CurrentPosition, Base.Length)), Powres))
                    {
                        CurrentPosition += CurrentOffsetLength;
                        CurrentOffsetLength /= 2;
                        continue;
                    }
                }
                catch (System.Exception)
                {
                    CurrentPosition -= CurrentOffsetLength;
                    CurrentOffsetLength /= 2;
                    continue;
                }
                
            }
        }

        public T Log2()
        {
            T Result = new T
            {
                Data = Internal_Log(Data, Uint64ToBitArray(2, Data.Length))
            };
            return Result;
        }

        public T Log10()
        {
            T Result = new T
            {
                Data = Internal_Log(Data, Uint64ToBitArray(10, Data.Length))
            };
            return Result;
        }

        public T Log(ulong Arg)
        {
            T Result = new T
            {
                Data = Internal_Log(Data, Uint64ToBitArray(Arg, Data.Length))
            };
            return Result;
        }

        public T Log(BigIntsBase<T> Arg)
        {
            T Result = new T
            {
                Data = Internal_Log(Data, Arg.Data)
            };
            return Result;
        }
        #endregion

        #region Square root
        private static BitArray Internal_Sqrt(BitArray Arg)
        {
            if (CompareBitArrays(Arg, new BitArray(Arg.Length, false)))
            {
                return new BitArray(Arg.Length, false);
            }

            BitArray result = new BitArray(Arg.Length, false);
            
            BitArray CurrentPosition = Uint64ToBitArray(2, Arg.Length);
            CurrentPosition = Internal_Pow(CurrentPosition, Uint64ToBitArray((ulong)(Arg.Length / 2), Arg.Length));
            BitArray CurrentOffset = Internal_Divide(CurrentPosition, Uint64ToBitArray(2, CurrentPosition.Length));

            while (true)
            {
                try
                {
                    if ((FirstIsSmaller(Internal_Multiply(CurrentPosition, CurrentPosition), Arg)
                    && FirstIsSmaller(Arg, Internal_Multiply(Internal_Add(CurrentPosition, Uint64ToBitArray(1, CurrentPosition.Length)),
                    Internal_Add(CurrentPosition, Uint64ToBitArray(1, CurrentPosition.Length)))))
                    || CompareBitArrays(Arg, Internal_Multiply(CurrentPosition, CurrentPosition)))
                    {
                        return CurrentPosition;
                    }
                    if (FirstIsSmaller(Arg, Internal_Multiply(CurrentPosition, CurrentPosition)))
                    {
                        CurrentPosition = Internal_Subtract(CurrentPosition, CurrentOffset);
                        CurrentOffset = Internal_Divide(CurrentOffset, Uint64ToBitArray(2, CurrentOffset.Length));
                        continue;
                    }
                    if (FirstIsSmaller(Internal_Multiply(CurrentPosition, CurrentPosition), Arg))
                    {
                        CurrentPosition = Internal_Add(CurrentPosition, CurrentOffset);
                        CurrentOffset = Internal_Divide(CurrentOffset, Uint64ToBitArray(2, CurrentOffset.Length));
                        continue;
                    }
                }
                catch (System.Exception)
                {
                    CurrentPosition = Internal_Subtract(CurrentPosition, CurrentOffset);
                    CurrentOffset = Internal_Divide(CurrentOffset, Uint64ToBitArray(2, CurrentOffset.Length));
                    continue;
                }               
            }
        }

        public T Sqrt()
        {
            T Result = new T
            {
                Data = Internal_Sqrt(Data)
            };
            return Result;
        }
        #endregion

        #region Factorial
        private static BitArray Internal_Factorial(BitArray Arg)
        {
            if (CompareBitArrays(Arg, new BitArray(Arg.Length, false)))
            {
                return Uint64ToBitArray(1, Arg.Length);
            }

            BitArray Result = Arg;

            for (BitArray i = Uint64ToBitArray(1, Arg.Length);
                FirstIsSmaller(i, Arg); i = Internal_Add(i, Uint64ToBitArray(1, i.Length)))
            {
                Result = Internal_Multiply(i, Result);
            }

            return Result;
        }

        public T Factorial()
        {
            T Result = new T
            {
                Data = Internal_Factorial(Data)
            };
            return Result;
        }

        #endregion

        #endregion

        #region Conversion methods
        public int TryToInt()
        {
            int result = 0;

            if (FirstIsSmaller(Uint64ToBitArray(int.MaxValue, Data.Length), Data))
            {
                throw new Exception("Arithmetic overflow");
            }

            for (int i = 0; i < 31; i++)
            {
                if (Data[i] == true)
                {
                    result += (int)Math.Pow(2, i);
                }
            }

            return result;
        }

        public ulong TryToUlong() => TryToUlong_(Data);

        public BitArray GetBinaryRespresentation() => Data;

        public override string ToString()
        {
            if (CompareBitArrays(Data, new BitArray(Data.Length, false)))
            {
                return "0";
            }

            string Result = "";
            bool IsSignificant = false;

            for (int i = 0; i < Length; i++)
            {
                if (Data[Length -1 - i] == true)
                {
                    Result += '1';
                    IsSignificant = true;
                }
                else if(IsSignificant)
                {
                    Result += '0';
                }
            }

            return Result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BigIntsBase<T>);
        }

        public bool Equals(BigIntsBase<T> other)
        {
            return other != null &&
                   EqualityComparer<BitArray>.Default.Equals(Data, other.Data);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Data);
        }

        #endregion

        #region Operators
        #region Arithmetic operators
        public static T operator +(BigIntsBase<T> a, BigIntsBase<T> b) => a.Add(b);
        public static T operator +(BigIntsBase<T> a, ulong b) => a.Add(b);

        public static T operator -(BigIntsBase<T> a, BigIntsBase<T> b) => a.Subtract(b);
        public static T operator -(BigIntsBase<T> a, ulong b) => a.Subtract(b);

        public static T operator *(BigIntsBase<T> a, BigIntsBase<T> b) => a.Multiply(b);
        public static T operator *(BigIntsBase<T> a, ulong b) => a.Multiply(b);

        public static T operator /(BigIntsBase<T> a, BigIntsBase<T> b) => a.Divide(b);
        public static T operator /(BigIntsBase<T> a, ulong b) => a.Divide(b);

        public static T operator %(BigIntsBase<T> a, BigIntsBase<T> b) => a.Mod(b);
        public static T operator %(BigIntsBase<T> a, ulong b) => a.Mod(b);
        #endregion

        #region Comparsion operators
        public static bool operator ==(BigIntsBase<T> First, BigIntsBase<T> Second)
        {
            if (CompareBitArrays(First.Data, Second.Data))
            {
                return true;
            }

            return false;
        }

        public static bool operator !=(BigIntsBase<T> First, BigIntsBase<T> Second)
        {
            if (!CompareBitArrays(First.Data, Second.Data))
            {
                return true;
            }

            return false;
        }

        public static bool operator <(BigIntsBase<T> First, BigIntsBase<T> Second)
        {
            if (FirstIsSmaller(First.Data, Second.Data))
            {
                return true;
            }

            return false;
        }

        public static bool operator >(BigIntsBase<T> First, BigIntsBase<T> Second)
        {
            if (FirstIsSmaller(Second.Data, First.Data))
            {
                return true;
            }

            return false;
        }

        public static bool operator <=(BigIntsBase<T> First, BigIntsBase<T> Second)
        {
            if (!FirstIsSmaller(Second.Data, First.Data))
            {
                return true;
            }

            return false;
        }

        public static bool operator >=(BigIntsBase<T> First, BigIntsBase<T> Second)
        {
            if (!FirstIsSmaller(First.Data, Second.Data))
            {
                return true;
            }

            return false;
        }

        #endregion

        #endregion
    }
}
