using CommandService.models;

namespace CommandService.Data;

public class CommandRepository(AppDbContext context) : ICommandRepository
{
    private readonly AppDbContext _context = context;

    public void CreateCommand(int platformId, Command command)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        command.PlatformId = platformId;
        _context.Commands.Add(command);
    }

    public void CreatePlatform(Platform platform)
    {
        if (platform is null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        _context.Platforms.Add(platform);
    }

    public bool ExternalPlatformExists(int externalPlatformId)
    {
        return _context.Platforms.Any(x => x.ExternalId == externalPlatformId);
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        return _context.Platforms.ToList();
    }

    public Command GetCommand(int platformId, int commandId)
    {
        return _context.Commands
            .Where(x => x.PlatformId == platformId && x.Id == commandId)
            .FirstOrDefault()!;
    }

    public IEnumerable<Command> GetCommandsForPlatform(int platformId)
    {
        return _context.Commands
            .Where(x => x.PlatformId == platformId)
            .OrderBy(x => x.Platform!.Name);
    }

    public bool PlatformExists(int platformId)
    {
        return _context.Platforms.Any(x => x.Id == platformId);
    }

    public bool SaveChanges()
    {
        return _context.SaveChanges() >= 0;
    }
}
