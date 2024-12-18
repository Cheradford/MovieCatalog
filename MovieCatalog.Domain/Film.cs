namespace MovieCatalog.Domain;

public class Film : ObjectId
{
    public string Title;
    public string Description;
    public DateOnly ReleaseDate;
    public int Rating;
}