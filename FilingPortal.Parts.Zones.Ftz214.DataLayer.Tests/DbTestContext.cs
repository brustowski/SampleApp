﻿using FilingPortal.DataLayer.Tests.Common;

namespace FilingPortal.Parts.Rail.Export.DataLayer.Tests
{
    public abstract  class DbTestContext : DbTestContextBase<PluginContext, UnitOfWorkContext>
    {
        protected override UnitOfWorkContext CreateUoW()
        {
            return new UnitOfWorkContext(new PluginContextFactory(), null);
        }
    }
}
