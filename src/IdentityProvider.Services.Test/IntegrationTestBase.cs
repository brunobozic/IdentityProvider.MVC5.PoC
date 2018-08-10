using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HAC.Helpdesk.Infrastructure.ConfigurationFront.Interface;
using HAC.Helpdesk.Infrastructure.DatabaseLog;
using HAC.Helpdesk.Infrastructure.LoggingFront.Interface;
using HAC.Helpdesk.SimpleMembership.Repository.EF.DataContexts;
using HAC.Helpdesk.SimpleMembership.Repository.EF.UnitOfWork;
using HAC.Helpdesk.SimpleMembership.Services.Test.StructureMap;

namespace HAC.Helpdesk.SimpleMembership.Services.Test
{
    [ExcludeFromCodeCoverage]
    public class IntegrationTestBase
    {
        private readonly List<string> _createdTestUsers = new List<string>();
        public ISimpleMembershipDataContextAsync _dataContextAsync;
        public IConfigurationRepository _fakeConfigurationRepository;
        public IContextService _fakeContextService;
        public IWcfAppenderService _fakewcfAppenderService;
        public ILoggingService _loggingService;

        public ISimpleMembershipUnitOfWorkAsync _uow;

        internal IntegrationTestBase()
        {
            var fact = new StructureMapServiceHostFactory();
            // AutoMapperBootStrapper.ConfigureAutoMapper();
        }

        public static object GetInstanceField(Type type, object instance, string fieldName)
        {
            const BindingFlags bindFlags =
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            var field = type.GetField(fieldName, bindFlags);
            return field != null ? field.GetValue(instance) : null;
        }
    }
}