using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using scenario.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace scenario.DAL
{
    public class StoryDBContext : DbContext
    {
        public DbSet<User> UserProfiles { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Voting> Votings { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<CharacterRelation> CharacterRelations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // relacje wiele do wielu

            modelBuilder.Entity<Character>()
             .HasMany(t => t.CharacterRelations)
             .WithRequired(t => t.Character1)
             .WillCascadeOnDelete(true);

            modelBuilder.Entity<Thread>()
             .HasMany(t => t.Parents)
             .WithMany(t => t.Children)
             .Map(t => t.MapLeftKey("ThreadId").MapRightKey("ParentId").ToTable("ThreadParents"));

            modelBuilder.Entity<Thread>()
             .HasMany(t => t.Characters)
             .WithMany(c => c.Threads)
             .Map(t => t.MapLeftKey("ThreadId").MapRightKey("CharacterId").ToTable("ThreadCharacters"));

            modelBuilder.Entity<Voting>()
             .HasMany(t => t.Threads)
             .WithMany(t => t.Votings)
             .Map(t => t.MapLeftKey("VotingId").MapRightKey("ThreadId").ToTable("VotingVariants"));

            base.OnModelCreating(modelBuilder);
        }
    }
}