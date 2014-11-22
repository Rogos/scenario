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
        public DbSet<CharacterRelation> CharacterRelations { get; set; }
        public DbSet<Thread> Threads { get; set; }
        //public DbSet<ThreadCharacter> ThreadCharacters { get; set; }
        //public DbSet<ThreadParent> ThreadParents { get; set; }
        public DbSet<Voting> Votings { get; set; }
        public DbSet<Vote> Votes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<CharacterRelation>()
               .HasRequired(a => a.Character1)
               .WithMany()
               .HasForeignKey(u => u.Character1ID);

            modelBuilder.Entity<CharacterRelation>()
                .HasRequired(a => a.Character2)
                .WithMany()
                .HasForeignKey(u => u.Character2ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ThreadCharacter>()
                .HasRequired(t => t.Character)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ThreadParent>()
                .HasRequired(t => t.Parent)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Story>().HasMany(m => m.Threads).WithOptional(m => m.Story).WillCascadeOnDelete(false);

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
        }
    }
}