namespace IdentityProvider.Infrastructure.Caching
{
    public interface ISessionCacheProvider
    {
        /// <summary>
        ///     Returns some cached object from memory. If is not putted into session is returned from the application context
        /// </summary>
        /// <typeparam name="T">the type of cached object</typeparam>
        /// <param name="cacheId">cache key</param>
        /// <returns></returns>
        T GetForUser<T>(string cacheId);

        /// <summary>
        ///     Saved some object into the session context for one particular user
        /// </summary>
        /// <typeparam name="T">the type of cached object</typeparam>
        /// <param name="item">object being cached</param>
        /// <param name="cacheId">cache key</param>
        /// <returns>returns the cached object</returns>
        void SaveForUser<T>(T item, string cacheId);

        /// <summary>
        ///     For the given cache id we are removing cached object from context
        /// </summary>
        /// <param name="cacheId">cache key</param>
        void ClearForUser(string cacheId);

        /// <summary>
        ///     Returns some cached object from memory. If is not putted into session is returned from the application context
        /// </summary>
        /// <typeparam name="T">the type of cached object</typeparam>
        /// <param name="cacheId">cache key</param>
        /// <returns></returns>
        T GetForApplication<T>(string cacheId);

        /// <summary>
        ///     Saved some object into the application context for all logged users
        /// </summary>
        /// <typeparam name="T">the type of cached object</typeparam>
        /// <param name="item">object being cached</param>
        /// <param name="cacheId">cache key</param>
        /// <returns>returns the cached object</returns>
        void SaveForApplication<T>(T item, string cacheId = "");

        /// <summary>
        ///     For the given cache id we are removing cached object from context
        /// </summary>
        /// <param name="cacheId">cache key</param>
        void ClearForApplication(string cacheId);
    }
}