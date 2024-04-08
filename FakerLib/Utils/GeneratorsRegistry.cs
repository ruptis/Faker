using System.Diagnostics.CodeAnalysis;
namespace FakerLib.Utils;

internal sealed class GeneratorsRegistry(IDictionary<Type, IGenerator> generators, IReadOnlyDictionary<Type, Type> lazyGeneratorsTypes)
{
    public void Register(Type type, IGenerator generator) => generators[type] = generator;

    public bool TryGet(Type type, [MaybeNullWhen(false)] out IGenerator generator) => generators.TryGetValue(type, out generator);

    public bool TryGetLazy(Type type, [MaybeNullWhen(false)] out IGenerator generator)
    {
        var genericArguments = type.GetGenericArguments();
        if (type.IsGenericType)
        {
            type = type.GetGenericTypeDefinition();
        }
        else if (type.IsArray)
        {
            genericArguments = [type.GetElementType()];
            type = typeof(Array);
        }

        if (lazyGeneratorsTypes.TryGetValue(type, out Type? lazyGeneratorType))
        {
            Type baseGeneratorType = lazyGeneratorType.IsGenericType ?
                genericArguments.Length == 0 ?
                    lazyGeneratorType.MakeGenericType(type) :
                    lazyGeneratorType.MakeGenericType(genericArguments) :
                lazyGeneratorType.IsArray ?
                    lazyGeneratorType.MakeGenericType(genericArguments) :
                    lazyGeneratorType;
            generator = (IGenerator)Activator.CreateInstance(baseGeneratorType)!;
            return true;
        }

        generator = null;
        return false;
    }
}
