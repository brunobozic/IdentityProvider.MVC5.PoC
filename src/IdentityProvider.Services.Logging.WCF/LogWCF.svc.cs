using System.ServiceModel;
using HAC.Helpdesk.Services.Logging.WCF.StructureMap;
using IdentityProvider.Infrastructure.DatabaseLog;

namespace HAC.Helpdesk.Services.Logging.WCF
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
	public class LogWcfService : ILogWcf
	{
		private readonly IDbLogService _service;

		public LogWcfService(IDbLogService dbLogService)
		{
			_service = dbLogService;
		}

		public LogWcfService()
		{
			_service = Ioc.GetContainer().GetInstance<IDbLogService>();
		}

		public void AppendToLog(LogToDatabaseRequest request)
		{
			_service.LogToDatabase(request.LoggingEventDto);
		}
	}
}