namespace scenario.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Characters", "AuthorId", c => c.Int());
            CreateIndex("dbo.Characters", "AuthorId");
            AddForeignKey("dbo.Characters", "AuthorId", "dbo.Users", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Characters", "AuthorId", "dbo.Users");
            DropIndex("dbo.Characters", new[] { "AuthorId" });
            DropColumn("dbo.Characters", "AuthorId");
        }
    }
}
