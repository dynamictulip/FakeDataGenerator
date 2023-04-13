using FakeDataGenerator.Model;

namespace FakeDataGenerator.Fakers;

public class GovernorFaker : ContactFaker
{
    public GovernorFaker(string trustName, bool isCurrent) : base(trustName)
    {
        _contactFaker
            .RuleFor(c => c.DateAppointed, f => isCurrent ? f.Date.Past(2) : f.Date.Past(10))
            .RuleFor(c => c.TermEnd,
                (f, c) => isCurrent ? f.Date.Future(2) : f.Date.Between(c.DateAppointed, DateTime.Now));
    }

    public Contact Generate(string role)
    {
        var contact = base.Generate();
        contact.Role = role;
        return contact;
    }

    public IEnumerable<Contact> Generate(string role, int num)
    {
        return base.Generate(num).Select(c =>
        {
            c.Role = role;
            return c;
        });
    }

    public override IEnumerable<Contact> Generate(int num, bool isInternal = false)
    {
        throw new NotSupportedException();
    }

    public override Contact Generate(bool isInternal = false)
    {
        throw new NotSupportedException();
    }
}