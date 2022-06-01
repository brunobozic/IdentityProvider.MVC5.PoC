using Module.CrossCutting.MVCActionFilters.PerformanceLog.Model;

namespace Module.CrossCutting.MVCActionFilters.PerformanceLog.Provider
{
    public interface IPerformanceLogProvider
    {
        void Log(PerformanceLogTick t);
    }
}