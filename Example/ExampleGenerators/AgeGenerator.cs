using FakerLib;
namespace Example.ExampleGenerators;

public class AgeGenerator(int minAge, int maxAge) : IGenerator<int>
{
    private readonly Random _random = new();

    public int Generate(IFaker faker) => _random.Next(minAge, maxAge);

    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
