namespace CommunicationLibrary
{
    public interface IParser
    {
        Message Parse(string messageString);
        string AsString<T>(T message) where T : Message;
    }
}