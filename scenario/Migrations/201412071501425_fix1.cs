namespace scenario.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Threads", "StoryId", "dbo.Stories");
            DropForeignKey("dbo.Votes", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.Votes", "UserId", "dbo.Users");
            DropIndex("dbo.Threads", new[] { "StoryId" });
            DropIndex("dbo.Votes", new[] { "ThreadId" });
            DropIndex("dbo.Votes", new[] { "UserId" });
            DropIndex("dbo.CharacterRelations", new[] { "Character2ID" });
            AlterColumn("dbo.Stories", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Threads", "StoryId", c => c.Int(nullable: false));
            AlterColumn("dbo.Threads", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Characters", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Votes", "ThreadId", c => c.Int());
            AlterColumn("dbo.Votes", "UserId", c => c.Int());
            AlterColumn("dbo.CharacterRelations", "Character2ID", c => c.Int());
            CreateIndex("dbo.CharacterRelations", "Character2ID");
            CreateIndex("dbo.Threads", "StoryId");
            CreateIndex("dbo.Votes", "ThreadId");
            CreateIndex("dbo.Votes", "UserId");
            AddForeignKey("dbo.Threads", "StoryId", "dbo.Stories", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Votes", "ThreadId", "dbo.Threads", "ID");
            AddForeignKey("dbo.Votes", "UserId", "dbo.Users", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votes", "UserId", "dbo.Users");
            DropForeignKey("dbo.Votes", "ThreadId", "dbo.Threads");
            DropForeignKey("dbo.Threads", "StoryId", "dbo.Stories");
            DropIndex("dbo.Votes", new[] { "UserId" });
            DropIndex("dbo.Votes", new[] { "ThreadId" });
            DropIndex("dbo.Threads", new[] { "StoryId" });
            DropIndex("dbo.CharacterRelations", new[] { "Character2ID" });
            AlterColumn("dbo.CharacterRelations", "Character2ID", c => c.Int(nullable: false));
            AlterColumn("dbo.Votes", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Votes", "ThreadId", c => c.Int(nullable: false));
            AlterColumn("dbo.Characters", "Name", c => c.String());
            AlterColumn("dbo.Threads", "Title", c => c.String());
            AlterColumn("dbo.Threads", "StoryId", c => c.Int());
            AlterColumn("dbo.Stories", "Title", c => c.String());
            CreateIndex("dbo.CharacterRelations", "Character2ID");
            CreateIndex("dbo.Votes", "UserId");
            CreateIndex("dbo.Votes", "ThreadId");
            CreateIndex("dbo.Threads", "StoryId");
            AddForeignKey("dbo.Votes", "UserId", "dbo.Users", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Votes", "ThreadId", "dbo.Threads", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Threads", "StoryId", "dbo.Stories", "ID");
        }
    }
}
