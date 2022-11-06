using Microsoft.AspNetCore.Mvc;
using UkrainianLyrics.Shared;
using UkrainianLyrics.Shared.Models;
using UkrainianLyrics.Shared.Payloads;
using UkrainianLyrics.WebAPI.Data;

namespace UkrainianLyrics.WebAPI.Controllers;

[ApiController]
[Route("")]
public class APIController : ControllerBase
{
    private readonly IAppRepository repository;
    private readonly LuceneSearchEngine searchEngine;

    public APIController(IAppRepository repository, LuceneSearchEngine searchEngine)
    {
        this.repository = repository;
        this.searchEngine = searchEngine;
    }

    [HttpGet("compositions")]
    public IActionResult GetCompositons()
        => Ok(Mapper.MapCompositions(repository.Compositions));

    [HttpGet("composition")]
    public async Task<IActionResult> GetComposition([FromHeader] Guid id)
        => Ok(Mapper.MapComposition(await repository.GetCompositionAsync(id)));

    [HttpGet("authors")]
    public IActionResult GetAuthors()
        => Ok(Mapper.MapAuthors(repository.Authors));

    [HttpGet("author")]
    public async Task<IActionResult> GetAuthor([FromHeader] Guid id)
        => Ok(Mapper.MapAuthor(await repository.GetAuthorAsync(id)));

    [HttpPost("composition")]
    public async Task<IActionResult> CreateComposition([FromBody] CompositionCreatePayload payload)
        => Ok(Mapper.MapComposition(await repository.CreateCompositionAsync(payload)));

    [HttpPost("author")]
    public async Task<IActionResult> CreateAuthor([FromBody] AuthorCreatePayload payload)
        => Ok(Mapper.MapAuthor(await repository.CreateAuthorAsync(payload)));

    [HttpPatch("composition")]
    public async Task<IActionResult> ModifyComposition([FromHeader] Guid id, [FromBody] CompositionModifyPayload payload)
        => Ok(Mapper.MapComposition(await repository.ModifyCompositionAsync(id, payload)));

    [HttpPatch("author")]
    public async Task<IActionResult> ModifyAuthor([FromHeader] Guid id, [FromBody] AuthorModifyPayload payload)
        => Ok(Mapper.MapAuthor(await repository.ModifyAuthorAsync(id, payload)));

    [HttpDelete("composition")]
    public async Task<IActionResult> DeleteComposition([FromHeader] Guid id)
        => Ok(Mapper.MapComposition(await repository.DeleteCompositionAsync(id)));

    [HttpDelete("author")]
    public async Task<IActionResult> DeleteAuthor([FromHeader] Guid id)
        => Ok(Mapper.MapAuthor(await repository.DeleteAuthorAsync(id)));

    [HttpGet("writings")]
    public async Task<IActionResult> GetAuthorsWritings([FromHeader] Guid authorId)
        => Ok(Mapper.MapCompositions((await repository.GetAuthorAsync(authorId)).Writings));

    [HttpGet("search")]
    public IActionResult Search([FromHeader] string searchTerms)
    {
        IList<object> results = new List<object>();

        foreach (object model in searchEngine.TextSearch(searchTerms))
        {
            if (model is Composition)
                results.Add(Mapper.MapComposition((Composition)model));
            else if (model is Author)
                results.Add(Mapper.MapAuthor((Author)model));
        }

        return Ok(results);
    }
}