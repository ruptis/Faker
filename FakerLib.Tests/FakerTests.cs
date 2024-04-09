namespace FakerLib.Tests;

public class SimpleClass
{
    public int IntProperty { get; set; }
    public string StringProperty { get; set; }
}

public class Tests
{
    [Test]
    public void NoDtoTest()
    {
        var faker = new Faker();
        var simpleClass = faker.Create<SimpleClass>();
        Assert.That(simpleClass, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(simpleClass.IntProperty, Is.EqualTo(default(int)));
            Assert.That(simpleClass.StringProperty, Is.EqualTo(default(string)));
        });
    }
}
