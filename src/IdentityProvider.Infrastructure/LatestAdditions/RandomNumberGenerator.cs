using System;
using System.Threading;

namespace IdentityProvider.Infrastructure.LatestAdditions
{
    public static class RandomNumberGenerator
    {
        private static int Seed = Environment.TickCount;

        private static ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref Seed)));

        public static Random GetThreadRandom()
        {
            return randomWrapper.Value;
        }

    }
}
