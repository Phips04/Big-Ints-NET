using System.Collections;

namespace Big_Ints_.NET
{
    public sealed class Uint128 : BigIntsBase<Uint128>, IBigInts
    {
        public override int Length { get => 128; }

        public Uint128() : base() { }
        public Uint128(string Bits) : base(Bits) { }
        public Uint128(BitArray array) : base(array) { }
        public Uint128(int Arg) : base(Arg) { }
        public Uint128(ulong Arg) : base(Arg) { }


        public static Uint128 operator ++ (Uint128 Arg) => Arg.Add(1);
        public static Uint128 operator --(Uint128 Arg) => Arg.Subtract(1);

        public override string ToString()
        {
            return base.ToString();
        }

        public static Uint128 MaxValue { get => new Uint128(new BitArray(128, true)); }
        public static Uint128 MinValue { get => new Uint128(new BitArray(128, false)); }
    }
}
