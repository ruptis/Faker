using FakerLib;
namespace AdditionalGeneratorsPlugin;

public class ArrayGenerator<T> : IGenerator<T[]>
{
    private readonly Random _random = new();

    public T[] Generate(IFaker faker)
    {
        var length = _random.Next(1, 10);
        var array = new T[length];
        for (var i = 0; i < length; i++)
            array[i] = faker.Create<T>();
        return array;
    }
    
    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
