namespace Colony.Engine.Generation;

internal static class ProceduralNoise
{
    public static double Fractal2D(double x, double y, double frequency, int octaves, int seed)
    {
        return FractalNoise(() => Sample2D(x, y, frequency, octaves, seed));
    }

    public static double Fractal3D(double x, double y, double z, double frequency, int octaves, int seed)
    {
        return FractalNoise(() => Sample3D(x, y, z, frequency, octaves, seed));
    }

    private static double FractalNoise(Func<double> sampler)
    {
        return sampler();
    }

    private static double Sample2D(double x, double y, double frequency, int octaves, int seed)
    {
        var amplitude = 1d;
        var persistence = 0.5d;
        var lacunarity = 2d;
        var sum = 0d;
        var normalization = 0d;
        var currentFrequency = frequency;

        for (var octave = 0; octave < octaves; octave++)
        {
            sum += ValueNoise2D(x * currentFrequency, y * currentFrequency, seed + (octave * 7919)) * amplitude;
            normalization += amplitude;
            amplitude *= persistence;
            currentFrequency *= lacunarity;
        }

        return normalization == 0d ? 0d : sum / normalization;
    }

    private static double Sample3D(double x, double y, double z, double frequency, int octaves, int seed)
    {
        var amplitude = 1d;
        var persistence = 0.5d;
        var lacunarity = 2d;
        var sum = 0d;
        var normalization = 0d;
        var currentFrequency = frequency;

        for (var octave = 0; octave < octaves; octave++)
        {
            sum += ValueNoise3D(x * currentFrequency,
                                y * currentFrequency,
                                z * currentFrequency,
                                seed + (octave * 7919)) * amplitude;
            normalization += amplitude;
            amplitude *= persistence;
            currentFrequency *= lacunarity;
        }

        return normalization == 0d ? 0d : sum / normalization;
    }

    private static double ValueNoise2D(double x, double y, int seed)
    {
        var x0 = (int)Math.Floor(x);
        var y0 = (int)Math.Floor(y);
        var x1 = x0 + 1;
        var y1 = y0 + 1;

        var tx = Fade(x - x0);
        var ty = Fade(y - y0);

        var v00 = HashToSignedUnit(x0, y0, 0, seed);
        var v10 = HashToSignedUnit(x1, y0, 0, seed);
        var v01 = HashToSignedUnit(x0, y1, 0, seed);
        var v11 = HashToSignedUnit(x1, y1, 0, seed);

        var ix0 = Lerp(v00, v10, tx);
        var ix1 = Lerp(v01, v11, tx);

        return Lerp(ix0, ix1, ty);
    }

    private static double ValueNoise3D(double x, double y, double z, int seed)
    {
        var x0 = (int)Math.Floor(x);
        var y0 = (int)Math.Floor(y);
        var z0 = (int)Math.Floor(z);
        var x1 = x0 + 1;
        var y1 = y0 + 1;
        var z1 = z0 + 1;

        var tx = Fade(x - x0);
        var ty = Fade(y - y0);
        var tz = Fade(z - z0);

        var c000 = HashToSignedUnit(x0, y0, z0, seed);
        var c100 = HashToSignedUnit(x1, y0, z0, seed);
        var c010 = HashToSignedUnit(x0, y1, z0, seed);
        var c110 = HashToSignedUnit(x1, y1, z0, seed);
        var c001 = HashToSignedUnit(x0, y0, z1, seed);
        var c101 = HashToSignedUnit(x1, y0, z1, seed);
        var c011 = HashToSignedUnit(x0, y1, z1, seed);
        var c111 = HashToSignedUnit(x1, y1, z1, seed);

        var x00 = Lerp(c000, c100, tx);
        var x10 = Lerp(c010, c110, tx);
        var x01 = Lerp(c001, c101, tx);
        var x11 = Lerp(c011, c111, tx);
        var y0Blend = Lerp(x00, x10, ty);
        var y1Blend = Lerp(x01, x11, ty);

        return Lerp(y0Blend, y1Blend, tz);
    }

    private static double HashToSignedUnit(int x, int y, int z, int seed)
    {
        unchecked
        {
            var hash = (uint)seed;
            hash ^= (uint)x * 374761393u;
            hash ^= (uint)y * 668265263u;
            hash ^= (uint)z * 2246822519u;
            hash = (hash ^ (hash >> 13)) * 1274126177u;
            hash ^= hash >> 16;

            return ((hash / (double)uint.MaxValue) * 2d) - 1d;
        }
    }

    private static double Fade(double value)
    {
        return value * value * value * (value * ((value * 6d) - 15d) + 10d);
    }

    private static double Lerp(double from, double to, double factor)
    {
        return from + ((to - from) * factor);
    }
}
