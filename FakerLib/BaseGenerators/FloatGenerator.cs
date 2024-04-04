namespace FakerLib.BaseGenerators;

internal class FloatGenerator : IGenerator<float>
{
    private readonly Random _random = new();

    public float Generate(IFaker faker) => _random.NextSingle();
    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
