namespace Example.ExampleDto;

public record GameDto(
    string Name,
    List<string> Genres,
    string Developer,
    DateTime ReleaseDate,
    decimal Price,
    Uri Website)
{
    public override string ToString() =>
        $"Game:" +
        $"\n  Name: {Name}" +
        $"\n  Genres: {string.Join(", ", Genres)}" +
        $"\n  Developer: {Developer}" +
        $"\n  ReleaseDate: {ReleaseDate}" +
        $"\n  Price: {Price}" +
        $"\n  Website: {Website}";
}
