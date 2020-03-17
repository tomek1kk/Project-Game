namespace CommunicationLibrary
{
    public interface IParser
    {
        Message Parse(string messageString);
        string AsString<T>(Message<T> message) where T : MessagePayload;
    }
}