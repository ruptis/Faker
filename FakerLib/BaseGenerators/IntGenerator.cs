namespace FakerLib.BaseGenerators;

internal class IntGenerator : IGenerator<int>
{
    private readonly Random _random = new();
    
    public int Generate(IFaker faker)
    {
        return _random.Next();
    }

    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
