
using CipherBank.AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CipherBank.AuthService.Infrastructure.Persistence
{
    public class AuthDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Role> Roles { get; set; } = default!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>
            {
                b.ToTable("Users");
                b.HasKey(x => x.Id);
                b.Property(x => x.UserName).IsRequired().HasMaxLength(128);
                b.Property(x => x.Email).IsRequired().HasMaxLength(256);
                b.Property(x => x.PasswordHash).IsRequired().HasMaxLength(512);
                b.Property(x => x.IsLocked).HasDefaultValue(false);
                b.Property(x => x.MFAEnabled).HasDefaultValue(false);
                b.HasIndex(x => x.UserName).IsUnique();
                b.HasIndex(x => x.Email).IsUnique();

                b.HasMany<RefreshToken>()
                 .WithOne()
                 .HasForeignKey(rt => rt.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Role>(b =>
            {
                b.ToTable("Roles");
                b.HasKey(x => x.Id);
                b.Property(x => x.Name).IsRequired().HasMaxLength(64);
                b.HasIndex(x => x.Name).IsUnique();

                b.HasData(
                    new Role(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Admin"),
                    new Role(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Teller"),
                    new Role(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Customer"),
                    new Role(Guid.Parse("44444444-4444-4444-4444-444444444444"), "Auditor"),
                    new Role(Guid.Parse("55555555-5555-5555-5555-555555555555"), "Support")
                );
            });

            // Shadow join table configuration

            modelBuilder.Entity<User>()
                    .HasMany(u => u.Roles)
                    .WithMany(r => r.Users)
                    .UsingEntity(j => j.ToTable("UserRoles"));


            modelBuilder.Entity<RefreshToken>(b =>
            {
                b.ToTable("RefreshTokens");
                b.HasKey(x => x.Id);
                b.Property(x => x.Token).IsRequired().HasMaxLength(512);
                b.Property(x => x.UserId).IsRequired();
                b.Property(x => x.Expiry).IsRequired();
                b.Property(x => x.IsRevoked).HasDefaultValue(false);
                b.Property(x => x.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                b.Property(x => x.ReplacedByToken).HasMaxLength(512);
                b.HasIndex(x => x.Token).IsUnique();
                b.HasIndex(x => x.UserId);
                b.HasIndex(x => x.Expiry);
            });
        }
    }
}
