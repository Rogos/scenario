namespace scenario.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ThreadParents",
                c => new
                    {
                        ThreadId = c.Int(nullable: false),
                        ParentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ThreadId, t.ParentId })
                .ForeignKey("dbo.Threads", t => t.ThreadId)
                .ForeignKey("dbo.Threads", t => t.ParentId)
                .Index(t => t.ThreadId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.ThreadCharacters",
                c => new
                    {
                        ThreadId = c.Int(nullable: false),
                        CharacterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ThreadId, t.CharacterId })
                .ForeignKey("dbo.Threads", t => t.ThreadId, cascadeDelete: true)
                .ForeignKey("dbo.Characters", t => t.CharacterId, cascadeDelete: true)
                .Index(t => t.ThreadId)
                .Index(t => t.CharacterId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ThreadCharacters", new[] { "CharacterId" });
            DropIndex("dbo.ThreadCharacters", new[] { "ThreadId" });
            DropIndex("dbo.ThreadParents", new[] { "ParentId" });
            DropIndex("dbo.ThreadParents", new[] { "ThreadId" });
            DropForeignKey("dbo.ThreadCharacters", "CharacterId", "dbo.Characters");
            DropForeignKey("dbo.ThreadCharacters", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.ThreadParents", "ParentId", "dbo.Threads");
            DropForeignKey("dbo.ThreadParents", "ThreadId", "dbo.Threads");
            DropTable("dbo.ThreadCharacters");
            DropTable("dbo.ThreadParents");
        }
    }
}
