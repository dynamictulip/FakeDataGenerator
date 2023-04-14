using Bogus;
using FakeDataGenerator.Model;

namespace FakeDataGenerator.Fakers;

public abstract class ContactFaker
{
    protected readonly Faker<Contact> _contactFaker;

    public ContactFaker()
    {
        _contactFaker = new Faker<Contact>("en_GB")
            .RuleFor(c => c.Name, f => f.Person.FullName)
            .RuleFor(c => c.Telephone, f => f.Person.Phone);
    }
}