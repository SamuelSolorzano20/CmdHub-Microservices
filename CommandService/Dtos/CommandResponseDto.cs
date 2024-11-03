namespace CommandService.Dtos;

public class CommandResponseDto
{
    public int Id { get; set; }
    public string? HowTo { get; set; }
    public string? CommandLine { get; set; }
    public int PlatformId { get; set; }
}
