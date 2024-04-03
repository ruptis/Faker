using System.Reflection;
namespace Faker;

public sealed class ContructionInfoProvider : IContructionInfoProvider
{
    private readonly Dictionary<Type, ConstructionInfo> _cache = new();

    public ConstructionInfo Get(Type type)
    {
        if (_cache.TryGetValue(type, out ConstructionInfo? info))
            return info;

        ConstructorInfo? constructor = GetConstructor(type);
        if (constructor is null)
            throw new InvalidOperationException($"Type {type} does not have a constructor.");

        var properties = type.GetProperties().Where(p => p.CanWrite).ToList();
        var fields = type.GetFields().ToList();

        foreach (var member in GetConstructorInitializedMembers(constructor))
        {
            PropertyInfo? property = properties.FirstOrDefault(p => p.Name == member);
            if (property is not null)
                properties.Remove(property);
            else
            {
                FieldInfo? field = fields.FirstOrDefault(f => f.Name == member);
                if (field is not null)
                    fields.Remove(field);
            }
        }

        info = new ConstructionInfo(type, constructor, constructor.GetParameters().ToList(), properties, fields);
        _cache[type] = info;

        return info;
    }

    private static ConstructorInfo? GetConstructor(Type type)
    {
        var constructors = type.GetConstructors();
        return constructors.Length switch
        {
            0 => null,
            1 => constructors[0],
            _ => constructors.OrderByDescending(c => c.GetParameters().Length).First()
        };
    }

    private static IEnumerable<string?> GetConstructorInitializedMembers(MethodBase constructor) =>
        constructor.GetParameters().Select(p => p.Name?[0].ToString().ToUpper() + p.Name?[1..]);
}
