using FakerLib.Factories;
using FakerLib.Utils;
namespace FakerLib;

public sealed class Faker : IFaker
{
    private readonly CircularDependencyAnalyzer _circularDependencyAnalyzer;
    private readonly GeneratorsRegistry _generatorsRegistry;
    private readonly DefaultGeneratorFactory _defaultGeneratorFactory = new();
    private readonly DtoGeneratorFactory _dtoGeneratorFactory;

    public Faker(FakerConfig? config = null)
    {
        config ??= new FakerConfig();
        _circularDependencyAnalyzer = new CircularDependencyAnalyzer(config.CircularDependencyDepthLimit);
        _dtoGeneratorFactory = new DtoGeneratorFactory(config);
        _generatorsRegistry = new GeneratorsRegistry(config.Generators, config.LazyGeneratorsTypes);
    }

    public T Create<T>()
    {
        T result = GetGenerator<T>().Generate(this);
        _circularDependencyAnalyzer.Remove(typeof(T));
        return result;
    }

    public object Create(Type type)
    {
        var result = GetGenerator(type).Generate(this);
        _circularDependencyAnalyzer.Remove(type);
        return result;
    }

    private IGenerator<T> GetGenerator<T>() => (IGenerator<T>)GetGenerator(typeof(T));

    private IGenerator GetGenerator(Type type)
    {
        if (!_circularDependencyAnalyzer.Validate(type))
            throw new InvalidOperationException($"Circular dependency detected: {_circularDependencyAnalyzer}");

        if (_generatorsRegistry.TryGet(type, out IGenerator? generator))
            return generator;
        
        if (_generatorsRegistry.TryGetLazy(type, out generator))
        {
            _generatorsRegistry.Register(type, generator);
            return generator;
        }

        if (_dtoGeneratorFactory.TryCreateDtoGenerator(type, out generator))
        {
            _generatorsRegistry.Register(type, generator);
            return generator;
        }

        if (_defaultGeneratorFactory.TryCreateDefaultGenerator(type, out generator))
        {
            _generatorsRegistry.Register(type, generator);
            return generator;
        }
        
        throw new InvalidOperationException($"Generator for type {type} not found");
    }
}