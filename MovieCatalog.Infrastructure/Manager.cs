using MovieCatalog.Domain;

namespace MovieCatalog.Infrastructure;

public class Manager
{
    private List<Film> Films = new List<Film>
    {
        new Film {Id = 1, Title = "Начало", Description = "Захватывающий триллер", Release = "16.07.2010" },
        new Film {Id = 2, Title = "Матрица", Description = "Классика научной фантастики", Release = "31.03.1999" },
        new Film {Id = 3, Title = "Интерстеллар", Description = "Путешествие сквозь пространство и время", Release = "07.11.2014" },
        new Film {Id = 4, Title = "Темный рыцарь", Description = "Легендарная история о Бэтмене", Release = "18.07.2008" }
    };
    private List<Note> Notes = new List<Note>
    {
        new Note { Text = "Отличная сцена в конце!", Timecode = "02:30:15", FilmId = 1 },
        new Note { Text = "Интересный диалог здесь.", Timecode = "01:15:42", FilmId = 2 },
        new Note { Text = "Красивые кадры!", Timecode = "00:45:20", FilmId = 3 },
        new Note { Text = "Неожиданный поворот событий.", Timecode = "01:55:10", FilmId = 1 },
        new Note { Text = "Отличная игра актеров.", Timecode = "00:32:45", FilmId = 2 },
        new Note { Text = "Глубокий смысл сцены.", Timecode = "01:10:30", FilmId = 3 },
        new Note { Text = "Великолепный экшен!", Timecode = "01:45:50", FilmId = 4 }
    };
    private List<Review> Reviews = new List<Review>
    {
        new Review { FilmId = 1, Status = Status.Watched, Rating = 9 },
        new Review { FilmId = 2, Status = Status.Watching, Rating = 8 },
        new Review { FilmId = 3, Status = Status.WillBeWatched, Rating = null },
        new Review { FilmId = 4, Status = Status.Watched, Rating = 10 }
    };

    public Manager()
    {
        
    }
    
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