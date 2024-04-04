using Example.ExampleDto;
using FakerLib;
namespace Example.ExampleGenerators;

public class GameDtoGenerator : IGenerator<GameDto>
{
    private readonly GameDto[] _games =
    [
        new GameDto("Cyberpunk 2077", new List<string> { "Action", "RPG" }, "CD Projekt", new DateTime(2020, 12, 10), 59.99m, new Uri("https://www.cyberpunk.net")),
        new GameDto("The Witcher 3: Wild Hunt", new List<string> { "Action", "RPG" }, "CD Projekt", new DateTime(2015, 5, 19), 39.99m, new Uri("https://thewitcher.com")),
        new GameDto("The Elder Scrolls V: Skyrim", new List<string> { "Action", "RPG" }, "Bethesda", new DateTime(2011, 11, 11), 19.99m, new Uri("https://elderscrolls.bethesda.net")),
        new GameDto("Grand Theft Auto V", new List<string> { "Action", "Adventure" }, "Rockstar Games", new DateTime(2013, 9, 17), 29.99m, new Uri("https://www.rockstargames.com")),
        new GameDto("Red Dead Redemption 2", new List<string> { "Action", "Adventure" }, "Rockstar Games", new DateTime(2018, 10, 26), 39.99m, new Uri("https://www.rockstargames.com"))
    ];
    private readonly Random _random = new();
    
    public GameDto Generate(IFaker faker) => _games[_random.Next(_games.Length)];
    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
