namespace FakerLib.BaseGenerators;

internal class ShortGenerator : IGenerator<short>
{
    private readonly Random _random = new();

    public short Generate(IFaker faker) => (short)_random.Next();

    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
