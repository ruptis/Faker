namespace Faker;

public interface IGeneratorsRegistry
{
    void Register<T>(IGenerator<T> generator);
    IGenerator<T> Get<T>();
    IGenerator Get(Type type);
    
    bool TryGet<T>(out IGenerator<T>? generator);
    bool TryGet(Type type, out IGenerator? generator);
}
