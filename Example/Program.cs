using System.Reflection;
using Example.ExampleDto;
using Example.ExampleGenerators;
using FakerLib;

var builder = new FakerBuilder();

builder
    .LoadFromAssembly(Assembly.Load(nameof(AdditionalGeneratorsPlugin)))
    .LoadFromAssembly(Assembly.Load(nameof(ListGeneratorPlugin)))
    .Add<UserDto, string, NameGenerator>(u => u.Name)
    .Add<UserDto, int>(u => u.Age, new AgeGenerator(20, 40))
    .Add<UserDto, Dictionary<string, string>, AdditionalInformationGenerator>(u => u.AdditionalInfo);

Faker faker = builder.Build();

var user = faker.Create<UserDto>();

Console.WriteLine(user);
