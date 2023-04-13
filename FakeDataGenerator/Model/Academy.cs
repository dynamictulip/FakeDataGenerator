namespace FakeDataGenerator.Model;

public class Academy
{
    public string Name { get; set; }
    public string LocalAuthority { get; set; }
    public string Phase { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public int Capacity { get; set; }
    public int PupilNumbers { get; set; }
    public DateTime DateJoined { get; set; }
    public string CurrentOfstedRating { get; set; }
    public DateTime CurrentOfstedRatingDate { get; set; }
    public string PreviousOfstedRating { get; set; }
    public DateTime PreviousOfstedRatingDate { get; set; }
}