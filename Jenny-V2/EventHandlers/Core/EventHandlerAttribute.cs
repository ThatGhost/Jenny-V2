using Jenny_V2.Services;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class EventHandlerAttribute : Attribute
{
    public TextCommand EventType { get; }

    public EventHandlerAttribute(TextCommand eventType)
    {
        EventType = eventType;
    }
}

public interface IEventHandler
{
    void Handle(string text);
}
