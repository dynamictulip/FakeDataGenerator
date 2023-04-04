namespace FakeDataGenerator;

public class Governance
{
    public string CompaniesHouseUrl { get; set; }
    public IList<Contact> Present { get; set; }
    public IList<Contact> Members { get; set; }
    public IList<Contact> Past { get; set; }
}