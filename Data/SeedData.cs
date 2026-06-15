using CommunityEvents.Models;
using Microsoft.EntityFrameworkCore;

namespace CommunityEvents.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

        await context.Database.MigrateAsync();

        if (context.Events.Any()) return; // Already seeded

        // Venues
        var venues = new List<Venue>
        {
            new() { Name = "City Convention Center", Address = "123 Main St", City = "Metropolis", Capacity = 500, Description = "Premier convention facility in the heart of the city.", ContactPhone = "555-0100", ContactEmail = "info@cityconvention.com" },
            new() { Name = "Community Hall", Address = "45 Park Ave", City = "Metropolis", Capacity = 150, Description = "Cozy community hall for smaller gatherings.", ContactPhone = "555-0200" },
            new() { Name = "Tech Hub Auditorium", Address = "789 Innovation Blvd", City = "Silicon Quarter", Capacity = 300, Description = "Modern auditorium in the tech district.", ContactEmail = "events@techhub.io" },
            new() { Name = "Riverside Pavilion", Address = "1 River Walk", City = "Metropolis", Capacity = 200, Description = "Beautiful outdoor pavilion by the river." },
            new() { Name = "Green Park Amphitheater", Address = "Green Park", City = "Metropolis", Capacity = 1000, Description = "Open-air amphitheater perfect for large events." }
        };
        context.Venues.AddRange(venues);
        await context.SaveChangesAsync();

        // Activities
        var activities = new List<Activity>
        {
            new() { Name = "Keynote Speech", Description = "Opening keynote by industry leaders.", Type = "Talk", DurationMinutes = 60 },
            new() { Name = "Web Dev Workshop", Description = "Hands-on ASP.NET Core development workshop.", Type = "Workshop", DurationMinutes = 120 },
            new() { Name = "Networking Session", Description = "Connect with fellow community members.", Type = "Networking", DurationMinutes = 45 },
            new() { Name = "Board Games Night", Description = "Fun board games for all ages.", Type = "Game", DurationMinutes = 180 },
            new() { Name = "Photography Walk", Description = "Guided photography tour around the city.", Type = "Tour", DurationMinutes = 90 },
            new() { Name = "Panel Discussion", Description = "Expert panel on community development.", Type = "Talk", DurationMinutes = 75 },
            new() { Name = "Yoga & Wellness", Description = "Morning yoga session for mindfulness.", Type = "Workshop", DurationMinutes = 60 },
            new() { Name = "Hackathon", Description = "24-hour coding challenge.", Type = "Competition", DurationMinutes = 1440 },
            new() { Name = "Art Exhibition", Description = "Local artists showcase their work.", Type = "Exhibition", DurationMinutes = 240 },
            new() { Name = "Cook-off Competition", Description = "Community cooking challenge.", Type = "Game", DurationMinutes = 120 }
        };
        context.Activities.AddRange(activities);
        await context.SaveChangesAsync();

        // Events
        var now = DateTime.Today;
        var events = new List<Event>
        {
            new() { Name = "Tech Summit 2026", Description = "Annual technology conference featuring cutting-edge talks, workshops, and networking opportunities for developers and tech enthusiasts.", Date = now.AddDays(10), Time = new TimeSpan(9, 0, 0), EventType = "Conference", MaxCapacity = 300, ImageUrl = "/images/event-tech.jpg" },
            new() { Name = "Community Wellness Day", Description = "A day dedicated to health, mindfulness, and community well-being. Join us for yoga, wellness workshops, and healthy food demonstrations.", Date = now.AddDays(5), Time = new TimeSpan(8, 0, 0), EventType = "Wellness", MaxCapacity = 150, ImageUrl = "/images/event-wellness.jpg" },
            new() { Name = "Art & Culture Festival", Description = "Celebrate local art and culture with exhibitions, live performances, and interactive workshops. Open to all ages!", Date = now.AddDays(15), Time = new TimeSpan(10, 0, 0), EventType = "Festival", MaxCapacity = 500, ImageUrl = "/images/event-art.jpg" },
            new() { Name = "Hackathon Spring 2026", Description = "48-hour coding marathon open to all skill levels. Build innovative solutions for real community problems.", Date = now.AddDays(20), Time = new TimeSpan(8, 0, 0), EventType = "Competition", MaxCapacity = 200, ImageUrl = "/images/event-hack.jpg" },
            new() { Name = "Networking Mixer", Description = "Monthly professionals networking event. Connect, collaborate, and grow your network in a relaxed environment.", Date = now.AddDays(3), Time = new TimeSpan(18, 0, 0), EventType = "Networking", MaxCapacity = 100, ImageUrl = "/images/event-network.jpg" },
            new() { Name = "Photography Walk", Description = "Explore the city through a lens! Join professional photographers on a guided photo walk through iconic spots.", Date = now.AddDays(7), Time = new TimeSpan(7, 30, 0), EventType = "Tour", MaxCapacity = 30, ImageUrl = "/images/event-photo.jpg" }
        };
        context.Events.AddRange(events);
        await context.SaveChangesAsync();

        // EventVenues
        context.EventVenues.AddRange(
            new EventVenue { EventId = events[0].Id, VenueId = venues[2].Id },
            new EventVenue { EventId = events[1].Id, VenueId = venues[1].Id },
            new EventVenue { EventId = events[2].Id, VenueId = venues[4].Id },
            new EventVenue { EventId = events[3].Id, VenueId = venues[2].Id },
            new EventVenue { EventId = events[4].Id, VenueId = venues[1].Id },
            new EventVenue { EventId = events[5].Id, VenueId = venues[3].Id }
        );

        // EventActivities
        context.EventActivities.AddRange(
            new EventActivity { EventId = events[0].Id, ActivityId = activities[0].Id },
            new EventActivity { EventId = events[0].Id, ActivityId = activities[1].Id },
            new EventActivity { EventId = events[0].Id, ActivityId = activities[2].Id },
            new EventActivity { EventId = events[1].Id, ActivityId = activities[6].Id },
            new EventActivity { EventId = events[1].Id, ActivityId = activities[2].Id },
            new EventActivity { EventId = events[2].Id, ActivityId = activities[8].Id },
            new EventActivity { EventId = events[2].Id, ActivityId = activities[5].Id },
            new EventActivity { EventId = events[3].Id, ActivityId = activities[7].Id },
            new EventActivity { EventId = events[4].Id, ActivityId = activities[2].Id },
            new EventActivity { EventId = events[5].Id, ActivityId = activities[4].Id }
        );

        // Participants
        var participants = new List<Participant>
        {
            new() { Name = "Alice Johnson", Email = "alice@example.com", Phone = "555-1001", Organization = "TechCorp Inc." },
            new() { Name = "Bob Martinez", Email = "bob@example.com", Phone = "555-1002", Organization = "StartupXYZ" },
            new() { Name = "Carol Chen", Email = "carol@example.com", Phone = "555-1003", Organization = "Community Hub" },
            new() { Name = "David Lee", Email = "david@example.com", Phone = "555-1004" },
            new() { Name = "Emma Wilson", Email = "emma@example.com", Organization = "Design Studio" }
        };
        context.Participants.AddRange(participants);
        await context.SaveChangesAsync();

        // Registrations
        context.Registrations.AddRange(
            new Registration { ParticipantId = participants[0].Id, EventId = events[0].Id, Status = RegistrationStatus.Confirmed, RegisteredAt = DateTime.UtcNow.AddDays(-5) },
            new Registration { ParticipantId = participants[0].Id, EventId = events[4].Id, Status = RegistrationStatus.Confirmed, RegisteredAt = DateTime.UtcNow.AddDays(-2) },
            new Registration { ParticipantId = participants[1].Id, EventId = events[0].Id, Status = RegistrationStatus.Pending, RegisteredAt = DateTime.UtcNow.AddDays(-3) },
            new Registration { ParticipantId = participants[1].Id, EventId = events[3].Id, Status = RegistrationStatus.Confirmed, RegisteredAt = DateTime.UtcNow.AddDays(-1) },
            new Registration { ParticipantId = participants[2].Id, EventId = events[1].Id, Status = RegistrationStatus.Confirmed, RegisteredAt = DateTime.UtcNow.AddDays(-4) },
            new Registration { ParticipantId = participants[3].Id, EventId = events[2].Id, Status = RegistrationStatus.Pending, RegisteredAt = DateTime.UtcNow.AddDays(-1) },
            new Registration { ParticipantId = participants[4].Id, EventId = events[2].Id, Status = RegistrationStatus.Confirmed, RegisteredAt = DateTime.UtcNow.AddDays(-2) }
        );

        await context.SaveChangesAsync();
    }
}
