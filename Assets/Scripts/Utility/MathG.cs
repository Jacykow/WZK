using System;

public struct MathG
{
    public static int GcdExtended(int a, int b, out int x, out int y)
    {
        if (a == 0)
        {
            x = 0;
            y = 1;
            return b;
        }

        int gcd = GcdExtended(b % a, a, out int x1, out int y1);

        x = y1 - (b / a) * x1;
        y = x1;

        return gcd;
    }

    public static int ModInverse(int b, int m)
    {
        int g = GcdExtended(b, m, out int x, out int _);
        if (g != 1) return -1;
        return Mod(x, m);
    }

    public static int ModDivide(int a, int b, int m)
    {
        if (b < 0)
        {
            a *= -1;
            b *= -1;
        }
        a = Mod(a, m);

        int inv = ModInverse(b, m);
        if (inv == -1)
            throw new ArgumentException("Division not defined");
        else
            return Mod(inv * a, m);
    }

    public static int Mod(int a, int m)
    {
        return ((a % m) + m) % m;
    }

    public static int ModPow(int a, int b, int m)
    {
        a %= m;
        int result = 1;
        while (b > 0)
        {
            if ((b & 1) > 0) result = (result * a) % m;
            a = (a * a) % m;
            b >>= 1;
        }
        return result;
    }

    public static int NextPrime(int start)
    {
        while (!IsPrime(start))
        {
            start++;
        }
        return start;
    }

    public static bool IsPrime(int number)
    {
        for (int i = 2; i * i <= number; i++)
        {
            if (number % i == 0)
            {
                return false;
            }
        }
        return true;
    }
}
