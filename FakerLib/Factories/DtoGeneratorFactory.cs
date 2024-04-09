using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FakerLib.Construction;
namespace FakerLib.Factories;

internal class DtoGeneratorFactory(FakerConfig config)
{
    private class DtoGenerator<T>(ConstructionInfo info, IReadOnlyDictionary<string, IGenerator> membersGenerators) : IGenerator<T>
        where T : notnull
    {
        public T Generate(IFaker faker)
        {
            var parameters = ResolveParameters(info.Parameters, faker);
            var result = (T)info.Constructor.Invoke(parameters);

            foreach (PropertyInfo property in info.Properties)
            {
                try
                {
                    property.SetValue(result, ResolveMember(property.PropertyType, property.Name, faker));
                }
                catch
                {
                   // ignored
                }
            }

            foreach (FieldInfo field in info.Fields)
            {
                try
                {
                    field.SetValue(result, ResolveMember(field.FieldType, field.Name, faker));
                }
                catch
                {
                    // ignored
                }
            }

            return result;
        }

        object IGenerator.Generate(IFaker faker) => Generate(faker);

        private object[] ResolveParameters(IEnumerable<ContructorParameterInfo> parameters, IFaker faker) =>
            parameters.Select(p => ResolveMember(p.Type, p.MemberName, faker)).ToArray();

        private object ResolveMember(Type type, string name, IFaker faker) =>
            membersGenerators.TryGetValue(name, out IGenerator? generator)
                ? generator.Generate(faker)
                : faker.Create(type);
    }
    
    private readonly ContructionInfoProvider _contructionInfoProvider = new();

    public bool TryCreateDtoGenerator(Type type, [MaybeNullWhen(false)] out IGenerator generator)
    {
        generator = null;

        if (!IsDto(type))
            return false;

        if (!_contructionInfoProvider.TryGet(type, out ConstructionInfo? constructionInfo))
            return false;

        if (!config.TryGetMembersGenerators(type, out var membersGenerators))
            membersGenerators = new Dictionary<string, IGenerator>();

        generator = Create(type, constructionInfo, membersGenerators);
        return true;
    }

    private static IGenerator Create(Type type, ConstructionInfo info, Dictionary<string, IGenerator> membersGenerators)
    {
        Type baseGeneratorType = typeof(DtoGenerator<>).MakeGenericType(type);
        return (IGenerator)Activator.CreateInstance(baseGeneratorType, info, membersGenerators)!;
    }
    
    private static bool IsDto(Type type) =>
        type is { IsInterface: false, IsAbstract: false, IsPrimitive: false } &&
        Nullable.GetUnderlyingType(type) is null &&
        (type.GetCustomAttributes(typeof(DtoAttribute), false).Length != 0 ||
            type.Name.EndsWith("to", StringComparison.OrdinalIgnoreCase));
}
