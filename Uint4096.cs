using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Big_Ints_.NET
{
    public sealed class Uint4096 : BigIntsBase<Uint4096>, IBigInts
    {
        public override int Length { get => 4096; }

        public Uint4096() : base() { }
        public Uint4096(string Bits) : base(Bits) { }
        public Uint4096(BitArray array) : base(array) { }
        public Uint4096(int Arg) : base(Arg) { }
        public Uint4096(ulong Arg) : base(Arg) { }
        public Uint4096(IBigInts.BigIntsBaseConstraints Arg) : base(Arg) { }


        public static Uint4096 operator ++(Uint4096 Arg) => Arg.Add(1);
        public static Uint4096 operator --(Uint4096 Arg) => Arg.Subtract(1);

        public override string ToString()
        {
            return base.ToString();
        }

        public static Uint4096 MaxValue { get => new Uint4096(new BitArray(4096, true)); }
        public static Uint4096 MinValue { get => new Uint4096(new BitArray(4096, false)); }
    }
}