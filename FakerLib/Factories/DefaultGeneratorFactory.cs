using System.Diagnostics.CodeAnalysis;
namespace FakerLib.Factories;

internal class DefaultGeneratorFactory
{
    private class DefaultClassGenerator<T> : IGenerator<T> where T : class, new()
    {
        public T Generate(IFaker faker) => new();
        object IGenerator.Generate(IFaker faker) => Generate(faker);
    }

    private class DefaultStructGenerator<T> : IGenerator<T> where T : struct
    {
        public T Generate(IFaker faker) => default;
        object IGenerator.Generate(IFaker faker) => Generate(faker);
    }
    
    public bool TryCreateDefaultGenerator(Type type, [MaybeNullWhen(false)] out IGenerator generator)
    {
        if (type.IsValueType)
        {
            Type baseGeneratorType = typeof(DefaultStructGenerator<>).MakeGenericType(type);
            generator = (IGenerator)Activator.CreateInstance(baseGeneratorType)!;
            return true;
        }
            
        if (type.IsClass && type.GetConstructor(Type.EmptyTypes) is not null)
        {
            Type baseGeneratorType = typeof(DefaultClassGenerator<>).MakeGenericType(type);
            generator = (IGenerator)Activator.CreateInstance(baseGeneratorType)!;
            return true;
        }

        generator = null;
        return false;
    }
}
