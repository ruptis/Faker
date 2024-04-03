namespace Faker;

public interface IGenerator
{
    object Generate(IFaker faker);
}

public interface IGenerator<out T> : IGenerator
{
    new T Generate(IFaker faker);
}

