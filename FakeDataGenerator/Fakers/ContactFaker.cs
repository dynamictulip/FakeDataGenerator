using Bogus;
using FakeDataGenerator.Model;

namespace FakeDataGenerator.Fakers;

public class ContactFaker
{
    protected readonly Faker<Contact> _contactFaker;

    public ContactFaker(string trustName)
    {
        _contactFaker = new Faker<Contact>("en_GB")
            .RuleFor(c => c.Name, f => f.Person.FullName)
            .RuleFor(c => c.Telephone, f => f.Person.Phone)
            .RuleSet("isInternal",
                set => set.RuleFor(c => c.Email, f => $"{f.Person.FirstName}.{f.Person.LastName}@education.gov.uk"))
            .RuleSet("isExternal",
                set => set.RuleFor(c => c.Email,
                    f => $"{f.Person.FirstName}.{f.Person.LastName}@{Helper.GetDomain(trustName)}"));
    }

    public virtual Contact Generate(bool isInternal = false)
    {
        var ruleSets = $"default, {(isInternal ? "isInternal" : "isExternal")}";
        return _contactFaker.Generate(ruleSets);
    }

    public virtual IEnumerable<Contact> Generate(int num, bool isInternal = false)
    {
        var ruleSets = $"default, {(isInternal ? "isInternal" : "isExternal")}";
        return _contactFaker.Generate(num, ruleSets);
    }
}