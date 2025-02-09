using System.Globalization;

namespace MovieCatalog.Domain;

public class Actor : ObjectId
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Birthday { 
        get => birthday.ToString("dd.MM.yyyy");
        set => birthday = DateOnly.Parse(value);
    }
    public bool isMale { get; set; }
    
    public List<Film> Films { get; set; } = new List<Film>();
    private DateOnly birthday { get; set; }
    
}