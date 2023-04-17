using FakeDataGenerator.Model;

namespace FakeDataGenerator.Fakers;

public class InternalContactFaker : ContactFaker
{
    public InternalContactFaker()
    {
        _contactFaker.RuleFor(c => c.Email, f => $"{f.Person.FirstName}.{f.Person.LastName}@education.gov.uk");
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