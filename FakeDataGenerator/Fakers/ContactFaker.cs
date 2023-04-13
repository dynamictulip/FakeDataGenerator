using Bogus;
using FakeDataGenerator.Model;

namespace FakeDataGenerator.Fakers;

public class ContactFaker
{
    protected readonly Faker<Contact> _contactFaker;

    public ContactFaker(string trustName, bool isInternal = false)
    {
        var domain = isInternal ? "education.gov.uk" : $"{Helper.GetDomain(trustName)}";
        _contactFaker = new Faker<Contact>("en_GB")
            .RuleFor(c => c.Name, f => f.Person.FullName)
            .RuleFor(c => c.Email, f => $"{f.Person.FirstName}.{f.Person.LastName}@{domain}")
            .RuleFor(c => c.Telephone, f => f.Person.Phone);
    }

    public Contact Generate()
    {
        return _contactFaker.Generate();
    }

    public IEnumerable<Contact> Generate(int num)
    {
        return _contactFaker.Generate(num);
    }
}