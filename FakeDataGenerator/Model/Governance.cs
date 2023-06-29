namespace FakeDataGenerator.Model;

public class Governance
{
    public int Turnover { get; set; }
    public IEnumerable<Contact> Present { get; set; }
    public IEnumerable<Contact> Members { get; set; }
    public IEnumerable<Contact> Past { get; set; }
    public IEnumerable<Contact> TrustManagement { get; set; }
    public IEnumerable<Contact> Trustees { get; set; }
    
    public IEnumerable<Contact> PastTwelveMonths { get; set; }
}
