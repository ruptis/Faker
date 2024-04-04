namespace FakerLib.BaseGenerators;

internal class ByteGenerator : IGenerator<byte>
{
    private readonly Random _random = new();

    public byte Generate(IFaker faker) => (byte)_random.Next();
    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
