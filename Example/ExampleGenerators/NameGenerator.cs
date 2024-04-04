using FakerLib;
namespace Example.ExampleGenerators;

public class NameGenerator : IGenerator<string>
{
    private readonly string[] _names = ["John", "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Grace", "Hank", "Ivy"];
    private readonly Random _random = new();

    public string Generate(IFaker faker) => _names[_random.Next(_names.Length)];

    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
