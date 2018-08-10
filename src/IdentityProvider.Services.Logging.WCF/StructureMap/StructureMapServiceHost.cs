using System;
using System.ServiceModel;

namespace HAC.Helpdesk.Services.Logging.WCF.StructureMap
{
	public class StructureMapServiceHost : ServiceHost
	{
		public StructureMapServiceHost()
		{
		}

		public StructureMapServiceHost(Type serviceType, params Uri[] baseAddresses)
			: base(serviceType, baseAddresses)
		{
		}

		protected override void OnOpening()
		{
			Description.Behaviors.Add(new StructureMapServiceBehavior());
			base.OnOpening();
		}

		protected override void OnClosed()
		{
			base.OnClosed();
		}
	}
}