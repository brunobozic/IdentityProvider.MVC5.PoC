using System;

namespace Logging.WCF.Infrastructure.DomainBaseAbstractions
{
    public class SystemClock
    {
        private static DateTimeOffset? staticTime;

        public static DateTimeOffset Now => staticTime ?? DateTimeOffset.UtcNow;

        /// <summary>
        ///     This method is used by unit-tests to force a specific time.
        /// </summary>
        /// <param name="currentTime">The current time to return on calls to <see cref="Now" />.</param>
        internal static void SetTime(DateTimeOffset currentTime)
        {
            staticTime = currentTime;
        }
    }
}