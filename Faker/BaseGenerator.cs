using System.Reflection;
namespace Faker;

class BaseGenerator<T>(ConstructionInfo info, IReadOnlyDictionary<string, IGenerator?> membersGenerators) : IGenerator<T>
    where T : notnull
{
    public T Generate(IFaker faker)
    {
        Type type = typeof(T);
        var parameters = ResolveParameters(info.Parameters, faker);
        var result = (T)info.Constructor.Invoke(parameters);
        
        foreach (PropertyInfo property in info.Properties)
            property.SetValue(result, ResolveParameter(property, faker));
    }
    
    object IGenerator.Generate(IFaker faker) => Generate(faker);
    
    private object[] ResolveParameters(IEnumerable<ParameterInfo> parameters, IFaker faker) => 
        parameters.Select(p => ResolveParameter(p, faker)).ToArray();

    private object ResolveParameter(ParameterInfo parameter, IFaker faker) => 
        membersGenerators[parameter.Name!]?.Generate(faker) ?? faker.Create(parameter.ParameterType);
}
