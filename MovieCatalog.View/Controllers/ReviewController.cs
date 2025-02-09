using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Domain;
using MovieCatalog.Infrastructure;

namespace MovieCatalog.View.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    private static ILogger<ReviewController> _logger;
    private Manager Manager { get; set; }
    
    public ReviewController(ILogger<ReviewController> logger, Manager manager)
    {
        _logger = logger;
        Manager = manager;
    }
    
    [HttpGet("{id}")]
    public IActionResult GetNoteById(int id)
    {
        var note = Manager.GetNoteById(id);
        if(note == null) return NotFound();
        return Ok(note);
    }

    [HttpPost("/set/{filmId}")]
    public IActionResult AddReview(int filmId, Review review)
    {
        if (!ModelState.IsValid) return BadRequest();
        var ent = Manager.AddReview(filmId, review);
        return Ok(ent);
    }
    
    [HttpPut]
    public IActionResult RedactReview(int id, Review review)
    {
        if (!ModelState.IsValid) return BadRequest();
        var ent = Manager.RedactReview(id, review);
        return Ok(ent);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteNote(int id)
    {
        var result = Manager.DeleteReview(id);
        if(!result) return Problem();
        return Ok();
    }
}