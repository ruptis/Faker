namespace Example.ExampleDto;

public record UserDto(
    string Email,
    string Name,
    int Age,
    DateTime BirthDate,
    Uri Website,
    string PhoneNumber,
    IEnumerable<GameDto> Games,
    Dictionary<string, string> AdditionalInfo)
{
    public override string ToString() =>
        $"User:" +
        $"\n  Email: {Email}" +
        $"\n  Name: {Name}" +
        $"\n  Age: {Age}" +
        $"\n  BirthDate: {BirthDate}" +
        $"\n  Website: {Website}" +
        $"\n  PhoneNumber: {PhoneNumber}" +
        $"\n  Games:" +
        $"\n{string.Join("\n", Games.Select(game => $"    {game.ToString().Replace("\n", "\n    ")}"))}" +
        $"\n  AdditionalInfo:" +
        $"\n{string.Join("\n", AdditionalInfo.Select(pair => $"    {pair.Key}: {pair.Value}"))}";
}