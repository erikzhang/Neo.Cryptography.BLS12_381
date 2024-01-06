namespace Neo.Cryptography.BLS12_381;

interface INumber<T> where T : unmanaged, INumber<T>
{
    //static abstract int Size { get; }
    //static abstract ref readonly T Zero { get; }
    //static abstract ref readonly T One { get; }

    //static abstract T operator -(in T x);
    //static abstract T operator +(in T x, in T y);
    //static abstract T operator -(in T x, in T y);
    //static abstract T operator *(in T x, in T y);

    T Negate();
    T Sum(in T value);
    T Subtract(in T value);
    T Multiply(in T value);

    abstract T Square();
}

static class NumberExtensions
{
    private static T PowVartime<T>(T one, T self, ulong[] by) where T : unmanaged, INumber<T>
    {
        // Although this is labeled "vartime", it is only
        // variable time with respect to the exponent.
        var res = one;
        for (int j = by.Length - 1; j >= 0; j--)
        {
            for (int i = 63; i >= 0; i--)
            {
                res = res.Square();
                if (((by[j] >> i) & 1) == 1)
                {
                    res = res.Multiply(self);
                }
            }
        }
        return res;
    }

    public static Fp PowVartime(this Fp self, ulong[] by)
    {
        return PowVartime(Fp.One, self, by);
    }

    public static Fp2 PowVartime(this Fp2 self, ulong[] by)
    {
        return PowVartime(Fp2.One, self, by);
    }

    public static Scalar PowVartime(this Scalar self, ulong[] by)
    {
        return PowVartime(Scalar.One, self, by);
    }
}
