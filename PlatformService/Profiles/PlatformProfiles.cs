using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles;

public class PlatformProfiles : Profile
{
    public PlatformProfiles()
    {
        CreateMap<Platform, PlatformResponseDto>();

        CreateMap<PlatformRequestDto, Platform>();
    }
}
