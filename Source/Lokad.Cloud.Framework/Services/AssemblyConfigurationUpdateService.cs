﻿#region Copyright (c) Lokad 2010
// This code is released under the terms of the new BSD licence.
// URL: http://www.lokad.com/
#endregion

using Lokad.Cloud.ServiceFabric;
using Lokad.Cloud.ServiceFabric.Runtime;
using Lokad.Cloud.Storage;

namespace Lokad.Cloud.Services
{
    /// <summary>
    /// Checks for updated assemblies or configuration and restarts the runtime if needed.
    /// </summary>
    [ScheduledServiceSettings(
           AutoStart = true,
           Description = "Checks for and applies assembly and configuration updates.",
           TriggerInterval = 60, // 1 execution every 1 minute
           ProcessingTimeoutSeconds = 300, // timeout after 5 minutes
           SchedulePerWorker = true)]
    public class AssemblyConfigurationUpdateService : ScheduledService
    {
        readonly AssemblyLoader _assemblyLoader;

        public AssemblyConfigurationUpdateService(IBlobStorageProvider storage)
        {
            // NOTE: we can't use the Blobs as provided by the base class
            // as this is not available at constructur time, but we want to reset
            // the status as soon as possible to avoid missing any changes

            _assemblyLoader = new AssemblyLoader(storage);
            _assemblyLoader.ResetUpdateStatus();
        }

        protected override void StartOnSchedule()
        {
            _assemblyLoader.CheckUpdate(false);
        }
    }
}
