using FakerLib;
namespace AdditionalGeneratorsPlugin;

public class UriGenerator : IGenerator<Uri>
{
    private readonly Random _random = new();
    
    private readonly string[] _schemes = ["http", "https"];
    private readonly string[] _hosts = ["google.com", "yandex.ru", "bing.com"];
    private readonly string[] _paths = ["/", "/search", "/images", "/news"];

    public Uri Generate(IFaker faker)
    {
        var scheme = _schemes[_random.Next(_schemes.Length)];
        var host = _hosts[_random.Next(_hosts.Length)];
        var path = _paths[_random.Next(_paths.Length)];
        return new Uri($"{scheme}://{host}{path}");
    }
    
    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
