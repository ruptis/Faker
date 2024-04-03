using System.Reflection;
namespace Faker;

class BaseGenerator<T>(ConstructionInfo info, IReadOnlyDictionary<string, IGenerator> membersGenerators) : IGenerator<T>
    where T : notnull
{
    public T Generate(IFaker faker)
    {
        var parameters = ResolveParameters(info.Parameters, faker);
        var result = (T)info.Constructor.Invoke(parameters);

        foreach (PropertyInfo property in info.Properties)
            property.SetValue(result, ResolveMember(property.PropertyType, property.Name, faker));

        foreach (FieldInfo field in info.Fields)
            field.SetValue(result, ResolveMember(field.FieldType, field.Name, faker));

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
