namespace FakerLib.BaseGenerators;

internal class LongGenerator : IGenerator<long>
{
    private readonly Random _random = new();

    public long Generate(IFaker faker) => _random.NextInt64();

    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
