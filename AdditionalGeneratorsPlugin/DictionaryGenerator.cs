using FakerLib;
namespace AdditionalGeneratorsPlugin;

public class DictionaryGenerator<TKey, TValue> : IGenerator<Dictionary<TKey, TValue>> where TKey : notnull
{
    private readonly Random _random = new();

    public Dictionary<TKey, TValue> Generate(IFaker faker)
    {
        var count = _random.Next(1, 10);
        var dictionary = new Dictionary<TKey, TValue>();
        for (var i = 0; i < count; i++)
            dictionary[faker.Create<TKey>()] = faker.Create<TValue>();
        return dictionary;
    }

    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
