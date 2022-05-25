﻿using Autofac;
using FilingPortal.PluginEngine.Autofac;

namespace FilingPortal.Parts.Isf.DataLayer
{
    public class AutofacModule : PluginDataLayerModule
    {
        /// <summary>Override to add registrations to the container.</summary>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWorkContext>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<PluginContextFactory>().AsImplementedInterfaces().InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
