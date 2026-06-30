namespace CommunityEvents.Exceptions;

public class ParticipantAlreadyRegisteredException : Exception
{
    public ParticipantAlreadyRegisteredException() 
        : base("The participant is already registered for this event.") { }

    public ParticipantAlreadyRegisteredException(string message) 
        : base(message) { }
        
    public ParticipantAlreadyRegisteredException(string message, Exception inner) 
        : base(message, inner) { }
}
