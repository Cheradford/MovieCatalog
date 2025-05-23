﻿using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Domain;
using MovieCatalog.Infrastructure;
using OpenTelemetry.Trace;
using Serilog;
namespace MovieCatalog.View.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController: ControllerBase
{
    private static ILogger<MovieController> _logger;
    private Manager Manager { get; set; }
    private readonly Tracer _tracer;
    public MovieController(ILogger<MovieController> logger, Manager manager, TracerProvider tracerProvider)
    {
        _logger = logger;
        Manager = manager;
        _tracer = tracerProvider.GetTracer("MovieController");
    }
    
    [HttpGet("{id}")]
    public IActionResult GetFilmById(int id)
    {
        using (var span = _tracer.StartActiveSpan("GetByFilmId"))
        {
            span.SetAttribute("id", id.ToString());
            
            if(id == 6) throw new Exception($"Illegal id={id}");
            
            var film = Manager.GetFilmById(id);
            if (film == null)
            {
                span.AddEvent("Film not founded");
                return NotFound();
            }
            span.AddEvent("Film founded");
            return Ok(film);
        }

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