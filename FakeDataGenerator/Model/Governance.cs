namespace FakeDataGenerator.Model;

public class Governance
{
    public IEnumerable<Contact> Present { get; set; }
    public IEnumerable<Contact> Members { get; set; }
    public IEnumerable<Contact> Past { get; set; }
}