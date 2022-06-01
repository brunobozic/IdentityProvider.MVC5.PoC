using System;
using System.Threading;

namespace Module.CrossCutting.LatestAdditions
{
    public static class RandomNumberGenerator
    {
        private static int Seed = Environment.TickCount;

        private static readonly ThreadLocal<Random> randomWrapper =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref Seed)));

        public static Random GetThreadRandom()
        {
            return randomWrapper.Value;
        }
    }
}