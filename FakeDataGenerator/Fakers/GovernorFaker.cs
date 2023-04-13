using Bogus;
using FakeDataGenerator.Model;

namespace FakeDataGenerator.Fakers;

public class GovernorFaker : ContactFaker
{
    public GovernorFaker(Faker generalFaker, string trustName, string role, bool isCurrent) : base(trustName)
    {
        var dateAppointed = isCurrent ? generalFaker.Date.Past(2) : generalFaker.Date.Past(10);
        _contactFaker.RuleFor(c => c.Role, role)
            .RuleFor(c => c.DateAppointed, dateAppointed)
            .RuleFor(c => c.TermEnd,
                f => isCurrent ? generalFaker.Date.Future(2) : generalFaker.Date.Between(dateAppointed, DateTime.Now));
    }
}