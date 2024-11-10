using AutoMapper;
using CommandService.Data;
using CommandService.Dtos;
using CommandService.models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers;

[Route("api/c/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepository _repository;
    private readonly IMapper _mapper;
    public CommandsController(ICommandRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandResponseDto>> GetCommandsForPlatform(int platformId)
    {
        if (!_repository.PlatformExists(platformId))
        {
            return NotFound();
        }

        var commands = _repository.GetCommandsForPlatform(platformId);
        
        return Ok(_mapper.Map<IEnumerable<CommandResponseDto>>(commands));
    }

    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
    public ActionResult<CommandResponseDto> GetCommandForPlatform(int platformId, int commandId)
    {
        if (!_repository.PlatformExists(platformId))
        {
            return NotFound();
        }

        var command = _repository.GetCommand(platformId, commandId);

        if (command is null)
        {
            return NotFound();
        }

        return Ok(_mapper.Map<CommandResponseDto>(command));
    }

   [HttpPost]
   public ActionResult<CommandResponseDto> CreateCommandForPlatform(int platformId, CommandRequestDto dto)
   {
        if (!_repository.PlatformExists(platformId))
        {
            return NotFound();
        }

        var command = _mapper.Map<Command>(dto);

        _repository.CreateCommand(platformId, command);
        _repository.SaveChanges();

        var commandResponseDto = _mapper.Map<CommandResponseDto>(command);

        return CreatedAtRoute(nameof(GetCommandForPlatform),
            new { platformId, commandId = commandResponseDto.Id }, commandResponseDto);
   } 

}
