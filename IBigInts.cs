using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Big_Ints_.NET
{
    

    public interface IBigInts
    {
        public abstract class BigIntsBaseConstraints
        {
            public BitArray Data { get; set; }

            public abstract int Length { get; }
        }
    }
}
