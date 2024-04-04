namespace FakerLib.BaseGenerators;

internal class BoolGenerator : IGenerator<bool>
{
    private readonly Random _random = new();
    
    public bool Generate(IFaker faker) => _random.Next(0, 2) == 1;
    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
