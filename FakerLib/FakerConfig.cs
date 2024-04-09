using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
namespace FakerLib;

public sealed class FakerConfig
{
    private readonly Dictionary<Type, IGenerator> _generators = new();
    private readonly Dictionary<Type, Type> _lazyGeneratorsTypes = new();
    private readonly Dictionary<Type, Dictionary<string, IGenerator>> _typeMemberGenerators = new();
    private int _circularDependencyDepthLimit = 10;

    internal IDictionary<Type, IGenerator> Generators => _generators;
    internal IReadOnlyDictionary<Type, Type> LazyGeneratorsTypes => _lazyGeneratorsTypes;
    internal int CircularDependencyDepthLimit
    {
        get => _circularDependencyDepthLimit;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Depth limit must be greater than 0");
            
            _circularDependencyDepthLimit = value;
        }
    }

    public FakerConfig() => LoadFromAssembly(Assembly.GetExecutingAssembly(), nameof(BaseGenerators));

    public void Add<T, TG>() where TG : IGenerator<T>, new() => Add(new TG());
    public void Add<T>(IGenerator<T> generator) => _generators[typeof(T)] = generator;
    public void Add(Type type, IGenerator generator) => _generators[type] = generator;
    public void AddLazy(Type type, Type generatorType)
    {
        if (type is { IsGenericTypeDefinition: false, IsGenericType: true })
            throw new ArgumentException("Type must be a generic type definition");

        _generators.Remove(type);
        _lazyGeneratorsTypes[type] = generatorType;
    }

    public void Add<T, TM, TG>(Expression<Func<T, TM>> expression) where TG : IGenerator<TM>, new() =>
        Add(expression, new TG());

    public void Add<T, TM>(Expression<Func<T, TM>> expression, IGenerator<TM> generator)
    {
        ExpressionType expressionType = expression.Body.NodeType;
        if (expressionType != ExpressionType.MemberAccess)
            throw new ArgumentException("Expression must be a member access expression");

        if (!_typeMemberGenerators.TryGetValue(typeof(T), out var generators))
        {
            generators = new Dictionary<string, IGenerator>();
            _typeMemberGenerators[typeof(T)] = generators;
        }

        var memberExpression = (MemberExpression)expression.Body;
        var memberName = memberExpression.Member.Name;
        generators[memberName] = generator;
    }

    public void LoadFromAssembly(Assembly assembly, string @namespace = "")
    {
        foreach (Type type in assembly.GetTypes())
        {
            if (!string.IsNullOrEmpty(@namespace) && type.Namespace?.EndsWith(@namespace) is false)
                continue;

            if (type.IsAbstract || type.IsInterface || type.GetConstructor(Type.EmptyTypes) is null)
                continue;

            Type? interfaceType = type.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IGenerator<>));

            if (interfaceType is null)
                continue;

            Type generatedType = interfaceType.GetGenericArguments()[0];

            if (type.IsGenericType)
                AddLazy(generatedType.IsGenericType ? generatedType.GetGenericTypeDefinition() :
                    generatedType.IsArray ? typeof(Array) :
                    generatedType, type);
            else
                Add(generatedType, (IGenerator)Activator.CreateInstance(type)!);
        }
    }

    internal bool TryGetMembersGenerators(Type type, [MaybeNullWhen(false)] out Dictionary<string, IGenerator> membersGenerators) =>
        _typeMemberGenerators.TryGetValue(type, out membersGenerators);
}
