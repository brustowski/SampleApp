// Generated Time: 02/07/2020 12:34:40
// Generated By: aikravchenko

namespace FilingPortal.Parts.Inbond.DataLayer.Migrations
{
    using FilingPortal.Parts.Common.DataLayer.Base;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cbdev_2965_add_marks_remarks_templates : FpMigration
    {
        public override void Up()
        {
            CreateTable(
                "inbond.handbook_marks_remarks_template",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        entry_type = c.String(nullable: false, maxLength: 128, unicode: false),
                        template_type = c.String(nullable: false, maxLength: 128, unicode: false),
                        description_template = c.String(nullable: false, maxLength: 1000, unicode: false),
                        marks_numbers_template = c.String(nullable: false, maxLength: 500, unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .Index(t => new { t.entry_type, t.template_type }, unique: true);

            ExecuteSqlFileUp();
        }
        
        public override void Down()
        {
            DropIndex("inbond.handbook_marks_remarks_template", new[] { "entry_type", "template_type" });
            DropTable("inbond.handbook_marks_remarks_template");
        }
    }
}