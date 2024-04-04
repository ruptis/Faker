namespace FakerLib.BaseGenerators;

internal class DoubleGenerator : IGenerator<double>
{
    private readonly Random _random = new();

    public double Generate(IFaker faker) => _random.NextDouble();
    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
