using Microsoft.EntityFrameworkCore;
using NoireServer.Database.Model;

namespace NoireServer.Database.Models.Contexts
{
    public class NoireContext: DbContext    
    {
        public NoireContext()
        {

        }
        public NoireContext(DbContextOptions<NoireContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Password=example;Database=noireDB");
        }
        public virtual DbSet<GameData> Games { get; set; }
        public virtual DbSet<PlayerData> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameData>(builder =>
            {
                builder.ToTable("games");
                builder.HasKey(i => i.Id);
                builder.Property(i => i.Id)
                       .ValueGeneratedOnAdd()
                       .HasColumnName("id")
                       .IsUnicode();
                builder.Property(f => f.PlayingField)
                       .HasColumnName("game_field")
                       .IsRequired();
                builder.Property(p => p.ProofDeck)
                       .IsRequired()
                       .HasColumnName("proof_deck");
                builder.Property(s => s.Status)
                       .IsRequired()
                       .HasColumnName("status");
                builder.Property(b => b.Bandit)
                       .HasColumnName("bandit");
                builder.Property(b => b.Inspector)
                       .HasColumnName("inspector");
                builder.Property(b => b.Win)
                       .HasColumnName("win");
            });

            modelBuilder.Entity<PlayerData>(builder =>
            {
                builder.ToTable("players");
                builder.HasKey(i => i.Id);
                builder.Property(i => i.Id)
                       .HasColumnName("id")
                       .ValueGeneratedOnAdd()
                       .IsUnicode();
                builder.Property(n => n.Name)
                       .HasColumnName("name");
                builder.Property(s => s.YourRole)
                       .IsRequired()
                       .HasColumnName("your_role");
                builder.Property(s => s.IdentityForInspector)
                       .HasColumnName("identity_for_inspector");
                builder.Property(s => s.IsYourMove)
                       .IsRequired()
                       .HasColumnName("is_your_move");

            });
        }
    }
}

