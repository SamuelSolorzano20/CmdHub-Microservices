using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : Controller
{
    private readonly IPlatformRepository _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    public PlatformsController(
        IPlatformRepository repository, 
        IMapper mapper,
        ICommandDataClient commandDataClient)
    {
        _repository = repository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PlatformResponseDto>> GetPlatforms() 
    {
        var platforms = _repository.GetAllPlatforms();
        return Ok(_mapper.Map<IEnumerable<PlatformResponseDto>>(platforms));
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformRequestDto> GetPlatformById(int id)
    {
        var platform = _repository.GetPlatformById(id);
        
        if (platform is not null)
        {
            return Ok(_mapper.Map<PlatformResponseDto>(platform));
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<PlatformResponseDto>> CreatePlatform(PlatformRequestDto req) 
    {
        var platform = _mapper.Map<Platform>(req);

        _repository.CreatePlatform(platform);
        _repository.SaveChanges();

        var response = _mapper.Map<PlatformResponseDto>(platform);

        try
        {
            await _commandDataClient.SendPlatformToCommand(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Couldn't send syncronously: {ex.Message}");
        }

        return CreatedAtRoute(nameof(GetPlatformById), new { response.Id }, response);
    }
}
