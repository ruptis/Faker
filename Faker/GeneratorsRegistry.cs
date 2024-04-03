namespace Faker;

internal class GeneratorsRegistry : IGeneratorsRegistry
{
    private readonly Dictionary<Type, IGenerator> _generators = new();

    public void Register<T>(IGenerator<T> generator) => _generators[typeof(T)] = generator;

    public IGenerator<T> Get<T>() => (IGenerator<T>)_generators[typeof(T)];

    public IGenerator Get(Type type) => _generators[type];

    public bool TryGet<T>(out IGenerator<T>? generator) =>
        _generators.TryGetValue(typeof(T), out IGenerator? value) ?
            (generator = (IGenerator<T>)value) != null :
            (generator = null) != null;

    public bool TryGet(Type type, out IGenerator? generator) => 
        _generators.TryGetValue(type, out generator);
}
