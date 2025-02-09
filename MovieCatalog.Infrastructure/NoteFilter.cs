namespace MovieCatalog.Infrastructure;

public class NoteFilter
{
    public int? FilmId { get; set; }
    public string? SearchText { get; set; }
    public string? CreatedBegin { get; set; }
    public string? CreatedEnd { get; set; }
    public string? TimecodeBegin { get; set; }
    public string? TimecodeEnd { get; set; }
}