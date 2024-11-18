using AutoMapper;
using CommandService.Dtos;
using CommandService.models;

namespace CommandService.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        CreateMap<Platform, PlatformResponseDto>();
        CreateMap<CommandRequestDto, Command>();
        CreateMap<Command, CommandResponseDto>();

        CreateMap<PlatformPublishedDto, Platform>()
            .ForMember(x => x.ExternalId, x => x.MapFrom(y => y.Id));
    }
}
