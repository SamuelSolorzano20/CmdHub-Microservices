namespace CommandService.EventProcessing;

public interface IEventProcessor
{
    void ProccessEvent(string message);
}