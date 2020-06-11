using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Big_Ints_.NET
{
    public sealed class Uint256 : BigIntsBase<Uint256>, IBigInts
    {
        public override int Length { get => 256; }

        public Uint256() : base() { }
        public Uint256(string Bits) : base(Bits) { }
        public Uint256(BitArray array) : base(array) { }
        public Uint256(int Arg) : base(Arg) { }
        public Uint256(ulong Arg) : base(Arg) { }
        public Uint256(IBigInts.BigIntsBaseConstraints Arg) : base(Arg) { }


        public static Uint256 operator ++(Uint256 Arg) => Arg.Add(1);
        public static Uint256 operator --(Uint256 Arg) => Arg.Subtract(1);

        public override string ToString()
        {
            return base.ToString();
        }

        public static Uint256 MaxValue { get => new Uint256(new BitArray(256, true)); }
        public static Uint256 MinValue { get => new Uint256(new BitArray(256, false)); }
    }
}
