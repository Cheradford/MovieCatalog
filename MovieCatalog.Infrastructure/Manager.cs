using MovieCatalog.Domain;

namespace MovieCatalog.Infrastructure;

public class Manager
{
    private List<Film> Films = new List<Film>();
    private List<Note> Notes = new List<Note>();
    private List<Review> Reviews = new List<Review>();

    #region Films
    public Film? GetFilmById(int id)
    {
        return Films.FirstOrDefault(f => f.Id == id);
    }

    public Film[] GetFilms(FilmFilter? filter)
    {
        var films = Films.AsQueryable();
        if (filter == null || (filter.SearchText == null && filter.BeginDate == null)) return films.ToArray();

        if (filter.SearchText != null)
        {
            films = films.Where(f => f.Title.Contains(filter.SearchText) || f.Description.Contains(filter.SearchText));
        }

        if (filter.BeginDate != null)
        {
            var bdate = DateOnly.Parse(filter.BeginDate);
            films = films.Where(f => f.ReleaseDate >= bdate);
        }

        if (filter.EndDate != null)
        {
            var enddate = DateOnly.Parse(filter.EndDate);
            films = films.Where(f => f.ReleaseDate <= enddate);
        }

        return films.ToArray();
    }

    public Film? AddFilm(Film film)
    {
        if (Films.Any(f => f.Id == film.Id || (f.Title == film.Title && f.Release == film.Release)))
            throw new ArgumentException($"Film with id: {film.Id} already exists.");
        var ent = Films.AddWithId(film);
        return ent;
    }
    public Film? RedactFilm(int id, Film update)
    {
        var ent = Films.FirstOrDefault(f => f.Id == id);
        if (ent == null) throw new NullReferenceException($"Film with id: {id} not found.");

        if(update.Title != null) ent.Title = update.Title;
        if(update.Description != null) ent.Description = update.Description;
        if(update.Release != null) ent.Release = update.Release;

        return ent;
    }

    public bool DeleteFilm(int id)
    {
        var ent = Films.FirstOrDefault(f => f.Id == id);
        if (ent == null) throw new NullReferenceException($"Film with id: {id} not found.");
        return Films.Remove(ent);
    }
    #endregion

    #region Notes

    public Note? GetNoteById(int id)
    {
        return Notes.FirstOrDefault(f => f.Id == id);
    }

    public Note[] GetNotes(NoteFilter? filter)
    {
        var notes = Notes.AsQueryable();
        if (filter == null) return notes.ToArray();

        if (filter.FilmId != null)
        {
            notes = notes.Where(n => n.FilmId == filter.FilmId);    
        }
        
        if (filter.SearchText != null)
        {
            notes = notes.Where(n => n.Text != null && n.Text.Contains(filter.SearchText));
        }

        if (filter.CreatedBegin != null)
        {
            var bdate = DateTime.Parse(filter.CreatedBegin);
            notes = notes.Where(n => n.Created >= bdate);
        }

        if (filter.CreatedEnd != null)
        {
            var enddate = DateTime.Parse(filter.CreatedEnd);
            notes = notes.Where(n => n.Created <= enddate);
        }

        if (filter.TimecodeBegin != null)
        {
            var timecodeBegin = TimeOnly.Parse(filter.TimecodeBegin);
            notes = notes.Where(n => n.timecode >= timecodeBegin);
        }
        
        if (filter.TimecodeEnd != null)
        {
            var timecodeEnd = TimeOnly.Parse(filter.TimecodeEnd);
            notes = notes.Where(n => n.timecode >= timecodeEnd);
        }

        return notes.ToArray();
    }

    public Note? AddNote(Note note)
    {
        if (Notes.Any(n => n.Id == note.Id || (n.Text == note.Text && n.timecode == note.timecode)))
            throw new ArgumentException($"Note with id: {note.Id} already exists.");
        var ent = Notes.AddWithId(note);
        return ent;
    }
    public Note? RedactNote(int id, Note update)
    {
        var ent = Notes.FirstOrDefault(f => f.Id == id);
        if (ent == null) throw new NullReferenceException($"Note with id: {id} not found.");

        if(update.Text != null) ent.Text = update.Text;
        if(update.Timecode != null) ent.Timecode = update.Timecode;
        if(update.Duration != null) ent.Duration = update.Duration;

        return ent;
    }

    public bool DeleteNote(int id)
    {
        var ent = Films.FirstOrDefault(f => f.Id == id);
        if (ent == null) throw new NullReferenceException($"Note with id: {id} not found.");
        return Films.Remove(ent);
    }
    
    #endregion
    
    #region Reviews

    public Review? GetReviewById(int id)
    {
        return Reviews.FirstOrDefault(f => f.Id == id);
    }
    
    public Review? GetReviewByFilmId(int id)
    {
        return Reviews.FirstOrDefault(f => f.FilmId == id);
    }
    
    public Review AddReview(int filmId, Review review)
    {
        if (Reviews.Any(r => r.FilmId == filmId))
            throw new ArgumentException($"Film with id: {review.Id} already exists.");
        review.FilmId = filmId;
        var ent = Reviews.AddWithId(review);
        return ent;
    }

    public Review RedactReview(int id, Review update)
    {
        var ent = Reviews.FirstOrDefault(r => r.Id == id);
        if (ent == null) throw new NullReferenceException($"Review with id: {id} not found.");
        if(ent.Rating != null) ent.Rating = update.Rating;
        if(ent.Status != null) ent.Status = update.Status;
        return ent;
    }

    public bool DeleteReview(int id)
    {
        var ent = Reviews.FirstOrDefault(r => r.Id == id);
        if (ent == null) throw new NullReferenceException($"Review with id: {id} not found.");
        return Reviews.Remove(ent);
    }
    #endregion
}