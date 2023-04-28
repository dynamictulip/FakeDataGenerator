namespace FakeDataGenerator.Helpers;

public static class Helper
{
    public static string GetDomain(string name)
    {
        return $"{name.ToLower().Replace(" ", "").Replace("'", "")}.co.uk";
    }
}