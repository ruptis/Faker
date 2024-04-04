namespace FakerLib.BaseGenerators;

internal class DateTimeGenerator : IGenerator<DateTime>
{
    private readonly Random _random = new();
    private readonly DateTime _minDate = new(1900, 1, 1);
    private readonly DateTime _maxDate = new(2100, 1, 1);
    private readonly double _range;
    
    public DateTimeGenerator() => _range = (_maxDate - _minDate).TotalSeconds;

    public DateTime Generate(IFaker faker)
    {
        var randomSeconds = _random.NextDouble() * _range;
        return _minDate.AddSeconds(randomSeconds);
    }
    
    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
