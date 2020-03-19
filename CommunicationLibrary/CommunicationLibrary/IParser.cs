namespace CommunicationLibrary
{
    public interface IParser
    {
        Message Parse(string messageString);
        string AsString(Message message);
    }
}