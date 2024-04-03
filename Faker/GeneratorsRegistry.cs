using System.Diagnostics.CodeAnalysis;
namespace Faker;

internal sealed class GeneratorsRegistry : IGeneratorsRegistry
{
    private readonly Dictionary<Type, IGenerator> _generators = new();

    public void Register<T>(IGenerator<T> generator) => _generators[typeof(T)] = generator;
    public void Register(Type type, IGenerator generator)
    {
        if (type.IsGenericTypeDefinition)
            _generators[type.GetGenericTypeDefinition()] = generator;
        else
            _generators[type] = generator;
    }

    public IGenerator<T> Get<T>() => (IGenerator<T>)Get(typeof(T));

    public IGenerator Get(Type type)
    {
        if (_generators.TryGetValue(type, out IGenerator? generator))
            return generator;
        
        if (TryCreateBaseGenerator(type, out generator))
            _generators[type] = generator;

        if (generator is not null)
            return generator;
        
        // Provide for handling types that are not DTOs and for which there is no generator. Their presence should not cause runtime exceptions.
        throw new InvalidOperationException();
    }
    
    private bool TryCreateBaseGenerator(Type type, [MaybeNullWhen(false)] out IGenerator generator)
    {
        generator = null;
        
        if (!new ContructionInfoProvider().TryGet(type, out ConstructionInfo? constructionInfo))
            return false;
        
        var membersGenerators = new Dictionary<string, IGenerator>();
        
        generator = BaseGeneratorFactory.Create(type, constructionInfo, membersGenerators);
        return true;
    }

    public bool TryGet<T>(out IGenerator<T>? generator) =>
        _generators.TryGetValue(typeof(T), out IGenerator? value) ?
            (generator = (IGenerator<T>)value) != null :
            (generator = null) != null;

    public bool TryGet(Type type, out IGenerator? generator) => 
        _generators.TryGetValue(type, out generator);
}

internal static class BaseGeneratorFactory
{
    public static IGenerator Create(Type type, ConstructionInfo info, Dictionary<string, IGenerator> membersGenerators)
    {
        Type baseGeneratorType = typeof(BaseGenerator<>).MakeGenericType(type);
        return (IGenerator)Activator.CreateInstance(baseGeneratorType, info, membersGenerators)!;
    }
}
