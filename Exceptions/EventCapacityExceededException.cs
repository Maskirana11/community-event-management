namespace CommunityEvents.Exceptions;

public class EventCapacityExceededException : Exception
{
    public EventCapacityExceededException() 
        : base("The event has reached its maximum capacity.") { }

    public EventCapacityExceededException(string message) 
        : base(message) { }
        
    public EventCapacityExceededException(string message, Exception inner) 
        : base(message, inner) { }
}
