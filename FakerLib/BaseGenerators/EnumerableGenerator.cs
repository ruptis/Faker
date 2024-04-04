namespace FakerLib.BaseGenerators;

internal class EnumerableGenerator<T> : IGenerator<IEnumerable<T>>
{
    private readonly Random _random = new();
    
    public IEnumerable<T> Generate(IFaker faker)
    {
        var count = _random.Next(1, 10);
        for (var i = 0; i < count; i++)
            yield return faker.Create<T>();
    }

    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
