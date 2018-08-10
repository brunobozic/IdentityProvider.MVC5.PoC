using IdentityProvider.Infrastructure.MVC5ActionFilters.PerformanceLog.Model;

namespace IdentityProvider.Infrastructure.MVC5ActionFilters.PerformanceLog.Provider
{
    public interface IPerformanceLogProvider
    {
        void Log(PerformanceLogTick t);
    }
}