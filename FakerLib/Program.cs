using FakerLib;

var builder = new FakerBuilder();

builder.Add<SimpleDto, string, CustomStringGenerator>(dto => dto.Name);

Faker faker = builder.Build();

var simpleDto = faker.Create<SimpleDto>();
var simpleDto2 = faker.Create<SimpleDto>();
var simpleDto3 = faker.Create<SimpleDto>();

var complexDto = faker.Create<ComplexDto>();

Console.WriteLine($"Name: {simpleDto.Name}, Age: {simpleDto.Age}");
foreach (int number in simpleDto.Numbers)
    Console.WriteLine(number);

Console.WriteLine(complexDto);
foreach (var pair in complexDto.Dict)
    Console.WriteLine($"{pair.Key}: {pair.Value}");

namespace FakerLib
{
    record SimpleDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public IEnumerable<int> Numbers { get; set; }
    }
    
    record ComplexDto
    {
        public SimpleDto SimpleDto { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public IEnumerable<SimpleDto> SimpleDtos { get; set; }
        public IEnumerable<int> Numbers { get; set; }
        public IEnumerable<string> Strings { get; set; }
        
        public Dictionary<int, string> Dict { get; set; }
    }
    
    class CustomStringGenerator : IGenerator<string>
    {
        public string Generate(IFaker faker) => "Hello, World!";
        object IGenerator.Generate(IFaker faker) => Generate(faker);
    }

    class TypeNameGenerator<T> : IGenerator<string>
    {
        public string Generate(IFaker faker) => typeof(T).Name;
        object IGenerator.Generate(IFaker faker) => Generate(faker);
    }

    class CustomNameGenerator(string name) : IGenerator<string>
    {
        public string Generate(IFaker faker) => $"Hello, {name}!";
        object IGenerator.Generate(IFaker faker) => Generate(faker);
    }
    
    class SimpleDtoGenerator : IGenerator<SimpleDto>
    {
        public SimpleDto Generate(IFaker faker) => new()
        {
            Name = "John",
            Age = 23,
            Numbers = faker.Create<IEnumerable<int>>()
        };

        object IGenerator.Generate(IFaker faker) => Generate(faker);
    }
}
