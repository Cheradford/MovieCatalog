using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieCatalog.Domain;

public class Film : ObjectId
{

    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Release
    {
        get
        {
            if (ReleaseDate.HasValue)
            {
                return ReleaseDate.Value.ToString("dd.MM.yyyy");
            }

            return null;
        }
        set
        {
            if (value != null)
                ReleaseDate = DateOnly.Parse(value);
        }
    }
    [JsonIgnore]
    public DateOnly? ReleaseDate {get;set;}
}