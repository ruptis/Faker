using System.Linq.Expressions;
using System.Reflection;
namespace FakerLib;

public sealed class FakerBuilder
{
    private readonly FakerConfig _config = new();
    
    public Faker Build() => new(_config);

    public FakerBuilder Add<T, TG>() where TG : IGenerator<T>, new()
    {
        _config.Add(new TG());
        return this;
    }

    public FakerBuilder Add<T>(IGenerator<T> generator)
    {
        _config.Add(generator);
        return this;
    }

    public FakerBuilder Add(Type type, IGenerator generator)
    {
        _config.Add(type, generator);
        return this;
    }

    public FakerBuilder AddLazy(Type type, Type generatorType)
    {
        _config.AddLazy(type, generatorType);
        return this;
    }

    public FakerBuilder Add<T, TM, TG>(Expression<Func<T, TM>> expression) where TG : IGenerator<TM>, new()
    {
        _config.Add<T, TM, TG>(expression);
        return this;
    }

    public FakerBuilder Add<T, TM>(Expression<Func<T, TM>> expression, IGenerator<TM> generator)
    {
        _config.Add(expression, generator);
        return this;
    }

    public FakerBuilder LoadFromAssembly(Assembly assembly, string @namespace = "")
    { 
        _config.LoadFromAssembly(assembly, @namespace);
        return this;
    }
    
    public FakerBuilder SetCircularDependencyDepthLimit(int limit)
    {
        _config.CircularDependencyDepthLimit = limit;
        return this;
    }
}
