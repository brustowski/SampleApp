// <auto-generated />
// Generated Time: 01/15/2020 18:24:57
// Generated By: AIKravchenko

namespace FilingPortal.Parts.Inbond.DataLayer.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    using FilingPortal.Parts.Common.DataLayer.Base;
    
    [GeneratedCode("EntityFramework.Migrations", "6.2.0-61023")]
    public sealed partial class cbdev_2914_hide_deleted_records_from_view : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(cbdev_2914_hide_deleted_records_from_view));
        
        string IMigrationMetadata.Id
        {
            get { return "202001151524579_cbdev_2914_hide_deleted_records_from_view"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
