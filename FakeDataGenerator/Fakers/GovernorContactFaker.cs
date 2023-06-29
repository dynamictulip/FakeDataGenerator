using FakeDataGenerator.Model;

namespace FakeDataGenerator.Fakers;

public class GovernorContactFaker : ContactFaker
{
    public GovernorContactFaker(bool isCurrent, string trustWebDomain)
    {
        _contactFaker
            .RuleFor(c => c.Email,
                f => $"{f.Person.FirstName}.{f.Person.LastName}@{trustWebDomain}")
            .RuleFor(c => c.DateAppointed, f => isCurrent ? f.Date.Past(2) : f.Date.Past(10))
            .RuleFor(c => c.TermEnd,
                (f, c) => isCurrent ? f.Date.Future(2) : f.Date.Between(c.DateAppointed, DateTime.Now))
            .RuleFor(c=> c.AppointmentType, f => f.PickRandom(Data.AppointmentType));
    }

    public Contact Generate(string role)
    {
        var contact = _contactFaker.Generate();
        contact.Role = role;
        return contact;
    }

    public IEnumerable<Contact> Generate(string role, int num)
    {
        return _contactFaker.Generate(num).Select(c =>
        {
            c.Role = role;
            return c;
        });
    }
}