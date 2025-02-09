using Microsoft.AspNetCore.Mvc;
using MovieCatalog.Domain;
using MovieCatalog.Infrastructure;

namespace MovieCatalog.View.Controllers;

[ApiController]
[Route("[controller]")]
public class NoteController : ControllerBase
{
    private static ILogger<NoteController> _logger;
    private Manager Manager { get; set; }
    public NoteController(ILogger<NoteController> logger, Manager manager)
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
    
    [HttpGet("notes")]
    public IActionResult GetNoteById([FromQuery]NoteFilter? filter)
    {
        var note = Manager.GetNotes(filter);
        if(note.Length == 0) return NotFound();
        return Ok(note);
    }

    [HttpPost]
    public IActionResult AddNote(Note Note)
    {
        if (!ModelState.IsValid) return BadRequest();
        var ent = Manager.AddNote(Note);
        return Ok(ent);
    }
    
    [HttpPut("{id}")]
    public IActionResult RedactNote(int id, Note Note)
    {
        if (!ModelState.IsValid) return BadRequest();
        var ent = Manager.RedactNote(id, Note);
        return Ok(ent);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteNote(int id)
    {
        var result = Manager.DeleteNote(id);
        if(!result) return Problem();
        return Ok();
    }
}