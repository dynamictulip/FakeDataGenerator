namespace FakeDataGenerator.Helpers;

public class TrustToGenerate
{
    public TrustToGenerate(string name, TrustType trustType = TrustType.MultiAcademyTrust, params string[] schools)
    {
        if (trustType == TrustType.SingleAcademyTrust && schools.Length != 1)
            throw new ArgumentException("Single academy trusts must have one school specified");
        
        Name = name;
        TrustType = trustType;
        Schools = schools;
    }

    public string Name { get; }
    public TrustType TrustType { get; }
    public string[] Schools { get; }
}