using System.Text.Json.Serialization;

namespace MovieCatalog.Domain;

public class ObjectId
{
    [JsonIgnore]
    public int Id;
}