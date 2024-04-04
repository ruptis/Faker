using System.Text;
namespace FakerLib.BaseGenerators;

internal class StringGenerator : IGenerator<string>
{
    private readonly Random _random = new();
    private readonly StringBuilder _builder = new();

    public string Generate(IFaker faker)
    {
        var length = _random.Next(1, 100);
        
        _builder.Clear();
        _builder.EnsureCapacity(length);
        
        for (var i = 0; i < length; i++)
            _builder.Append((char)_random.Next(32, 127));
        
        return _builder.ToString();
    }
    
    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
