using Microsoft.EntityFrameworkCore;
using CommunityEvents.Models;

namespace CommunityEvents.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Event> Events { get; set; }
    public DbSet<Venue> Venues { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<Registration> Registrations { get; set; }
    public DbSet<UserAccount> UserAccounts { get; set; }
    public DbSet<EventVenue> EventVenues { get; set; }
    public DbSet<EventActivity> EventActivities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // EventVenue many-to-many
        modelBuilder.Entity<EventVenue>()
            .HasKey(ev => new { ev.EventId, ev.VenueId });

        modelBuilder.Entity<EventVenue>()
            .HasOne(ev => ev.Event)
            .WithMany(e => e.EventVenues)
            .HasForeignKey(ev => ev.EventId);

        modelBuilder.Entity<EventVenue>()
            .HasOne(ev => ev.Venue)
            .WithMany(v => v.EventVenues)
            .HasForeignKey(ev => ev.VenueId);

        // EventActivity many-to-many
        modelBuilder.Entity<EventActivity>()
            .HasKey(ea => new { ea.EventId, ea.ActivityId });

        modelBuilder.Entity<EventActivity>()
            .HasOne(ea => ea.Event)
            .WithMany(e => e.EventActivities)
            .HasForeignKey(ea => ea.EventId);

        modelBuilder.Entity<EventActivity>()
            .HasOne(ea => ea.Activity)
            .WithMany(a => a.EventActivities)
            .HasForeignKey(ea => ea.ActivityId);

        // Registration
        modelBuilder.Entity<Registration>()
            .HasOne(r => r.Participant)
            .WithMany(p => p.Registrations)
            .HasForeignKey(r => r.ParticipantId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Registration>()
            .HasOne(r => r.Event)
            .WithMany(e => e.Registrations)
            .HasForeignKey(r => r.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique email for participants
        modelBuilder.Entity<Participant>()
            .HasIndex(p => p.Email)
            .IsUnique();
    }
}
