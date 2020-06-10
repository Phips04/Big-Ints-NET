using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Big_Ints_.NET
{
    public sealed class Uint16384 : BigIntsBase<Uint16384>, IBigInts
    {
        public override int Length { get => 16384; }

        public Uint16384() : base() { }
        public Uint16384(string Bits) : base(Bits) { }
        public Uint16384(BitArray array) : base(array) { }
        public Uint16384(int Arg) : base(Arg) { }
        public Uint16384(ulong Arg) : base(Arg) { }


        public static Uint16384 operator ++(Uint16384 Arg) => Arg.Add(1);
        public static Uint16384 operator --(Uint16384 Arg) => Arg.Subtract(1);

        public override string ToString()
        {
            return base.ToString();
        }

        public static Uint16384 MaxValue { get => new Uint16384(new BitArray(16384, true)); }
        public static Uint16384 MinValue { get => new Uint16384(new BitArray(16384, false)); }
    }
}
