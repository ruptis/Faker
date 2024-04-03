using Faker;

var registry = new GeneratorsRegistry();
registry.Register(new StringGenerator());
registry.Register(new SimpleDtoGenerator());

var faker = new SimpleFaker(registry);

var simpleDto = faker.Create<SimpleDto>();

Console.WriteLine($"Name: {simpleDto.Name}");
Console.WriteLine($"Age: {simpleDto.Age}");

var complexDto = faker.Create<ComplexDto>();

Console.WriteLine($"SimpleDto.Name: {complexDto.SimpleDto.Name}");
Console.WriteLine($"SimpleDto.Age: {complexDto.SimpleDto.Age}");
Console.WriteLine($"Number: {complexDto.Number}");

var complexDtoWithConstructor = faker.Create<ComplexDtoWithConstructor>();

Console.WriteLine($"SimpleDto.Name: {complexDtoWithConstructor.SimpleDto.Name}");
Console.WriteLine($"SimpleDto.Age: {complexDtoWithConstructor.SimpleDto.Age}");
Console.WriteLine($"Number: {complexDtoWithConstructor.Number}");

namespace Faker
{
    class SimpleFaker(IGeneratorsRegistry generatorsRegistry) : IFaker
    {
        public T Create<T>() => generatorsRegistry.Get<T>().Generate(this);

        public object Create(Type type) => generatorsRegistry.Get(type).Generate(this);
    }
    
    class SimpleDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class ComplexDto
    {
        public SimpleDto SimpleDto { get; set; }
        public int Number { get; set; }
    }
    
    struct ComplexDtoWithConstructor(SimpleDto simpleDto, int number)
    {
        public SimpleDto SimpleDto { get; } = simpleDto;
        public int Number { get; } = number;
    }
    
    class ComplexDtoWithConstructor2(SimpleDto simpleDto)
    {
        public SimpleDto SimpleDto { get; set; } = simpleDto;
        public int Number { get; set; }
        public int Age;
    }
    
    class SimpleDtoGenerator : IGenerator<SimpleDto>
    {
        public SimpleDto Generate(IFaker faker) => new()
        {
            Name = faker.Create<string>(),
            Age = 23
        };
        
        object IGenerator.Generate(IFaker faker) => Generate(faker);
    }

    class StringGenerator : IGenerator<string>
    {
        public string Generate(IFaker faker) => "Hello, World!";

        object IGenerator.Generate(IFaker faker) => Generate(faker);
    }

    class IntGenerator : IGenerator<int>
    {
        public int Generate(IFaker faker) => 42;

        object IGenerator.Generate(IFaker faker) => Generate(faker);
    }

    class CustomIntGenerator : IGenerator<int>
    {
        public int Generate(IFaker faker) => 24;
        object IGenerator.Generate(IFaker faker) => Generate(faker);
    }

}
