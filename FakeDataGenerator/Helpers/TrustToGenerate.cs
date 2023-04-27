namespace FakeDataGenerator.Helpers;

public class TrustToGenerate
{
    public TrustToGenerate(string name, TrustType trustType = TrustType.MultiAcademyTrust, params string[] schools)
    {
        Name = name;
        TrustType = trustType;
        Schools = schools;
    }

    public string Name { get; }
    public TrustType TrustType { get; }
    public string[] Schools { get; }
}