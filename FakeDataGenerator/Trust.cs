namespace FakeDataGenerator;

public class Trust
{
    public string Name { get; set; }
    public string Uid { get; set; }
    public string TrustType { get; set; }

    public TrustDetails TrustDetails { get; } = new TrustDetails();
}