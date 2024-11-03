using AutoMapper;
using CommandService.models;

namespace CommandService.Dtos;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        CreateMap<Platform, PlatformResponseDto>();
        CreateMap<CommandRequestDto, Command>();
        CreateMap<Command, CommandResponseDto>();
    }
}
