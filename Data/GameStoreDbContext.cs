using GameStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data;

public class GameStoreDbContext : DbContext
{
    public GameStoreDbContext(DbContextOptions<GameStoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Platform> Platforms => Set<Platform>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<GameGenre> GameGenres => Set<GameGenre>();
    public DbSet<Publisher> Publishers => Set<Publisher>();
    public DbSet<GamePublisher> GamePublishers => Set<GamePublisher>();
    public DbSet<Developer> Developers => Set<Developer>();
    public DbSet<GameDeveloper> GameDevelopers => Set<GameDeveloper>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<GameImage> GameImages => Set<GameImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // GameGenre — составной ключ
        modelBuilder.Entity<GameGenre>()
            .HasKey(gg => new { gg.GameId, gg.GenreId });

        modelBuilder.Entity<GameGenre>()
            .HasOne(gg => gg.Game)
            .WithMany(g => g.GameGenres)
            .HasForeignKey(gg => gg.GameId);

        modelBuilder.Entity<GameGenre>()
            .HasOne(gg => gg.Genre)
            .WithMany(g => g.GameGenres)
            .HasForeignKey(gg => gg.GenreId);

        // GamePublisher — составной ключ
        modelBuilder.Entity<GamePublisher>()
            .HasKey(gp => new { gp.GameId, gp.PublisherId });

        modelBuilder.Entity<GamePublisher>()
            .HasOne(gp => gp.Game)
            .WithMany(g => g.GamePublishers)
            .HasForeignKey(gp => gp.GameId);

        modelBuilder.Entity<GamePublisher>()
            .HasOne(gp => gp.Publisher)
            .WithMany(p => p.GamePublishers)
            .HasForeignKey(gp => gp.PublisherId);

        // GameDeveloper — составной ключ
        modelBuilder.Entity<GameDeveloper>()
            .HasKey(gd => new { gd.GameId, gd.DeveloperId });

        modelBuilder.Entity<GameDeveloper>()
            .HasOne(gd => gd.Game)
            .WithMany(g => g.GameDevelopers)
            .HasForeignKey(gd => gd.GameId);

        modelBuilder.Entity<GameDeveloper>()
            .HasOne(gd => gd.Developer)
            .WithMany(d => d.GameDevelopers)
            .HasForeignKey(gd => gd.DeveloperId);

        // Product: уникальная пара (GameId, PlatformId)
        modelBuilder.Entity<Product>()
            .HasIndex(p => new { p.GameId, p.PlatformId })
            .IsUnique();

        // Product -> Game и Product -> Platform
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Game)
            .WithMany(g => g.Products)
            .HasForeignKey(p => p.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Platform)
            .WithMany(pl => pl.Products)
            .HasForeignKey(p => p.PlatformId)
            .OnDelete(DeleteBehavior.Restrict);

        // GameImage -> Game
        modelBuilder.Entity<GameImage>()
            .HasOne(gi => gi.Game)
            .WithMany(g => g.Images)
            .HasForeignKey(gi => gi.GameId)
            .OnDelete(DeleteBehavior.Cascade);

        // Money precision для SQLite не критично, но зададим
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");
    }
}