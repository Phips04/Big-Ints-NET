using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Big_Ints_.NET
{
    public sealed class Uint512 : BigIntsBase<Uint512>, IBigInts
    {
        public override int Length { get => 512; }

        public Uint512() : base() { }
        public Uint512(string Bits) : base(Bits) { }
        public Uint512(BitArray array) : base(array) { }
        public Uint512(int Arg) : base(Arg) { }
        public Uint512(ulong Arg) : base(Arg) { }
        public Uint512(IBigInts.BigIntsBaseConstraints Arg) : base(Arg) { }


        public static Uint512 operator ++(Uint512 Arg) => Arg.Add(1);
        public static Uint512 operator --(Uint512 Arg) => Arg.Subtract(1);

        public override string ToString()
        {
            return base.ToString();
        }

        public static Uint512 MaxValue { get => new Uint512(new BitArray(512, true)); }
        public static Uint512 MinValue { get => new Uint512(new BitArray(512, false)); }
    }
}