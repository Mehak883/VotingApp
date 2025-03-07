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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vote>()
                .HasIndex(v => v.VoterId)
                .IsUnique();

            modelBuilder.Entity<Candidate>()
                .HasIndex(c => new { c.StateId, c.PartyId })
                .IsUnique();
        }

    }
}
