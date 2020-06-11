using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Big_Ints_.NET
{
    public sealed class Uint8192 : BigIntsBase<Uint8192>, IBigInts
    {
        public override int Length { get => 8192; }

        public Uint8192() : base() { }
        public Uint8192(string Bits) : base(Bits) { }
        public Uint8192(BitArray array) : base(array) { }
        public Uint8192(int Arg) : base(Arg) { }
        public Uint8192(ulong Arg) : base(Arg) { }
        public Uint8192(IBigInts.BigIntsBaseConstraints Arg) : base(Arg) { }


        public static Uint8192 operator ++(Uint8192 Arg) => Arg.Add(1);
        public static Uint8192 operator --(Uint8192 Arg) => Arg.Subtract(1);

        public override string ToString()
        {
            return base.ToString();
        }

        public static Uint8192 MaxValue { get => new Uint8192(new BitArray(8192, true)); }
        public static Uint8192 MinValue { get => new Uint8192(new BitArray(8192, false)); }
    }
}
