using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace HAC.Helpdesk.Services.Logging.WCF.StructureMap
{
	public class StructureMapServiceHostFactory : ServiceHostFactory
	{
		public StructureMapServiceHostFactory()
		{
			// AutoMapperConfig.Start();
		}

		protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
		{
			return new StructureMapServiceHost(serviceType, baseAddresses);
		}
	}
}