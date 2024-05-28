using DatingApp.Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Backend.Infrastructure.Persistence;

public class DatingAppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<UserLike> Likes { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserLike>()
            .HasKey(k => new { k.SourceUserId, k.TargetUserId });

        builder.Entity<UserLike>()
            .HasOne(l => l.SourceUser)
            .WithMany(u => u.LikedUsers)
            .HasForeignKey(l => l.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserLike>()
            .HasOne(l => l.TargetUser)
            .WithMany(u => u.LikedByUsers)
            .HasForeignKey(l => l.TargetUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Message>()
            .HasOne(m => m.Recipient)
            .WithMany(r => r.MessagesReceived)
            .HasForeignKey(m => m.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(s => s.MessagesSent)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}