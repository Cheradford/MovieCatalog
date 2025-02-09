using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Domain;
using MovieCatalog.Infrastructure;

namespace MovieCatalog.View.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController: ControllerBase
{
    private static ILogger<MovieController> _logger;
    private Manager Manager { get; set; }
    
    public MovieController(ILogger<MovieController> logger, Manager manager)
    {
        _logger = logger;
        Manager = manager;
    }
    
    [HttpGet("{id}")]
    public IActionResult GetFilmById(int id)
    {
        var film = Manager.GetFilmById(id);
        if(film == null) return NotFound();
        return Ok(film);
    }
    
    [HttpGet("films")]
    public IActionResult GetFilms([FromQuery]FilmFilter? filter)
    {
        var film = Manager.GetFilms(filter);
        if(film.Length == 0) return NotFound();
        return Ok(film);
    }

    [HttpPost]
    public IActionResult AddFilm(Film film)
    {
        if (!ModelState.IsValid) return BadRequest();
        var ent = Manager.AddFilm(film);
        return Ok(ent);
    }
    
    [HttpPut("{id}")]
    public IActionResult RedactFilm(int id, Film film)
    {
        if (!ModelState.IsValid) return BadRequest();
        var ent = Manager.RedactFilm(id, film);
        return Ok(ent);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteFilm(int id)
    {
        var result = Manager.DeleteFilm(id);
        if(!result) return Problem();
        return Ok();
    }
    
}