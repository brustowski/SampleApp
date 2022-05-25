using Autofac;
using FilingPortal.Parts.Recon.Domain.Entities;
using FilingPortal.Parts.Recon.Domain.Validators;
using FilingPortal.Parts.Recon.Web.DataSources;
using FilingPortal.Parts.Recon.Web.Validators;
using FilingPortal.PluginEngine.Autofac;

namespace FilingPortal.Parts.Recon.Web
{
    public class AutofacModule : PluginWebModule
    {
        /// <summary>
        /// Adds registrations to the container
        /// </summary>
        /// <param name="builder">The builder through which components can be registered.</param>
        protected override void LoadPluginTypes(ContainerBuilder builder)
        {
            builder.RegisterType<StatusFilterDataProvider<FtaReconStatus, int>>().AsImplementedInterfaces();
            builder.RegisterType<StatusFilterDataProvider<ValueReconStatus, int>>().AsImplementedInterfaces();
            builder.RegisterType<CreatedUserDataProvider<FtaRecon>>().AsImplementedInterfaces();
            builder.RegisterType<CreatedUserDataProvider<ValueRecon>>().AsImplementedInterfaces();
            builder.RegisterType<InboundRecordsValidator>().AsImplementedInterfaces();
            builder.RegisterType<CargoWiseRecordValidator>().AsImplementedInterfaces();
        }
    }
}
