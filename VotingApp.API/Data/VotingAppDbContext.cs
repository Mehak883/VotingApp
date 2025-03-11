using Microsoft.EntityFrameworkCore;
using VotingApp.API.Models;

namespace VotingApp.API.Data
{
    public class VotingAppDbContext:DbContext
    {
        public VotingAppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<State> States { get; set; }
        public DbSet<Party> Parties { get; set; }
        public DbSet<Voter> Voters { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<AuthUser> AuthUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Voter)
                .WithMany()
                .HasForeignKey(v => v.VoterId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Candidate)
                .WithMany()
                .HasForeignKey(v => v.CandidateId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Vote>()
                .HasIndex(v => v.VoterId)
                .IsUnique();

            modelBuilder.Entity<Candidate>()
                .HasIndex(c => new { c.StateId, c.PartyId })
                .IsUnique();

            modelBuilder.Entity<State>()
                .HasIndex(s => s.Name)
                .IsUnique();
            modelBuilder.Entity<Party>()
               .HasIndex(p => p.Name)
               .IsUnique();
            modelBuilder.Entity<Party>()
              .HasIndex(p => p.Symbol)
              .IsUnique();
            modelBuilder.Entity<Party>()
               .HasIndex(p => new { p.Name, p.Symbol })
               .IsUnique();
        }

    }
}
