﻿using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace HAC.Helpdesk.Services.Logging.WCF.StructureMap
{
    public class StructureMapInstanceProvider : IInstanceProvider
    {
        private readonly Type _serviceType;

        public StructureMapInstanceProvider(Type serviceType)
        {
            _serviceType = serviceType;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            var whatDoIHave = Ioc.GetContainer().GetNestedContainer().WhatDoIHave();
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            var whatDoIHave = Ioc.GetContainer().GetNestedContainer().WhatDoIHave();
            return Ioc.GetContainer().GetNestedContainer().GetInstance(_serviceType);
        }
    }
}