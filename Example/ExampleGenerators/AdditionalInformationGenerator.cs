using FakerLib;
namespace Example.ExampleGenerators;

public class AdditionalInformationGenerator : IGenerator<Dictionary<string, string>>
{
    private readonly Dictionary<string, List<string>> _additionalInformation = new()
    {
        ["Hobbies"] = ["Reading", "Swimming", "Cycling", "Running", "Cooking", "Gardening", "Painting", "Sculpting", "Singing", "Dancing"],
        ["Languages"] = ["English", "Spanish", "French", "German", "Italian", "Portuguese", "Russian", "Chinese", "Japanese", "Korean"],
        ["Skills"] = ["Programming", "Designing", "Writing", "Teaching", "Marketing", "Selling", "Managing", "Analyzing", "Planning", "Organizing"]
    };
    private readonly Random _random = new();
    
    public Dictionary<string, string> Generate(IFaker faker)
    {
        var count = _random.Next(1, 4);
        var additionalInformation = new Dictionary<string, string>();
        for (var i = 0; i < count; i++)
        {
            var key = _additionalInformation.Keys.ElementAt(_random.Next(_additionalInformation.Count));
            var value = _additionalInformation[key][_random.Next(_additionalInformation[key].Count)];
            additionalInformation[key] = value;
        }
        return additionalInformation;
    }
    
    object IGenerator.Generate(IFaker faker) => Generate(faker);
}
