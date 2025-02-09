using System.Text.Json.Serialization;

namespace MovieCatalog.Domain;

public class Note: ObjectId 
{
   public string? Text { get; set; }
   public DateTime? Created { get; set; } = DateTime.Now;
   public string? Timecode {
      get
      {
         if(!timecode.HasValue) return null;
         return timecode.Value.ToString("HH:mm:ss");
      }
      set
      {
         if (value == null)
         {
            timecode = null;
            return;
         }
         timecode = TimeOnly.Parse(value);
      }
   }
   public int? Duration { get; set; }
   
   [JsonIgnore] public TimeOnly? timecode { get; set; }
   [JsonIgnore] public int FilmId { get; set; }
}