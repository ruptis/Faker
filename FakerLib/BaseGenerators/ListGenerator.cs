namespace FakerLib.BaseGenerators;

internal class ListGenerator<T> : IGenerator<List<T>> 
{
    private readonly Random _random = new();
    
    public List<T> Generate(IFaker faker)
    {
        var count = _random.Next(1, 10);
        var list = new List<T>();
        for (var i = 0; i < count; i++)
            list.Add(faker.Create<T>());
        return list;
    }
    
    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
