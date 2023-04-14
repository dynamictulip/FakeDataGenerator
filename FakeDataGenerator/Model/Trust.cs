namespace FakeDataGenerator.Model;

public class Trust
{
    public string Name { get; set; }
    public string Uid { get; set; }
    public string TrustType => "Multi-academy trust";

    public TrustDetails TrustDetails { get; set; }
    public Governance Governance { get; set; }
    
    public AcademiesInTrust AcademiesInTrust { get; set; }
}