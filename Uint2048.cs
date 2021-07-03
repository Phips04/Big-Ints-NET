using System.Collections;

namespace Big_Ints_.NET
{
    public sealed class Uint2048 : BigIntsBase<Uint2048>, IBigInts
    {
        public override int Length { get => 2048; }

        public Uint2048() : base() { }
        public Uint2048(string Bits) : base(Bits) { }
        public Uint2048(BitArray array) : base(array) { }
        public Uint2048(int Arg) : base(Arg) { }
        public Uint2048(ulong Arg) : base(Arg) { }
        public Uint2048(IBigInts.BigIntsBaseConstraints Arg) : base(Arg) { }


        public static Uint2048 operator ++(Uint2048 Arg) => Arg.Add(1);
        public static Uint2048 operator --(Uint2048 Arg) => Arg.Subtract(1);

        public override string ToString()
        {
            return base.ToString();
        }

        public static Uint2048 MaxValue { get => new Uint2048(new BitArray(2048, true)); }
        public static Uint2048 MinValue { get => new Uint2048(new BitArray(2048, false)); }
    }
}