namespace MovieCatalog.Domain;

public class Review : ObjectId
{
    public int FilmId { get; set; }
    public Status? Status { get; set; }
    public int? Rating
    {
        get => rating;
        set
        {
            if (value < 0 || value > 10) throw new ArgumentException($"Value must be between 0 and 10.");
            rating = value;
        }
    }

    private int? rating;
}

public enum Status
{
    None,
    WillBeWatched,
    Watching,
    Watched,
    Abandoned,
    
}