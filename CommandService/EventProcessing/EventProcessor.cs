using System.Text.Json;
using AutoMapper;
using CommandService.Dtos;

namespace CommandService.EventProcessing;

public class EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper) : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;
    private readonly IMapper _mapper = mapper;

    public void ProccessEvent(string message)
    {
        throw new NotImplementedException();
    }

    private EventType DtermineEvent(string notificationMessage)
    {
        Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        switch (eventType!.Event)
        {
            case "Platform_Published":
                Console.WriteLine("--> Platform published event detected");
                return EventType.PlatformPublished;
            default:
                Console.WriteLine("--> Could not determine event type");
                return EventType.Undetermined;
        }
    }
}

enum EventType 
{
    PlatformPublished,
    Undetermined
}