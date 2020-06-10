using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Big_Ints_.NET
{
    public sealed class Uint1024 : BigIntsBase<Uint1024>, IBigInts
    {
        public override int Length { get => 1024; }

        public Uint1024() : base() { }
        public Uint1024(string Bits) : base(Bits) { }
        public Uint1024(BitArray array) : base(array) { }
        public Uint1024(int Arg) : base(Arg) { }
        public Uint1024(ulong Arg) : base(Arg) { }


        public static Uint1024 operator ++(Uint1024 Arg) => Arg.Add(1);
        public static Uint1024 operator --(Uint1024 Arg) => Arg.Subtract(1);

        public override string ToString()
        {
            return base.ToString();
        }

        public static Uint1024 MaxValue { get => new Uint1024(new BitArray(1024, true)); }
        public static Uint1024 MinValue { get => new Uint1024(new BitArray(1024, false)); }
    }
}