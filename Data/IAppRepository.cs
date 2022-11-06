using UkrainianLyrics.Shared.Models;
using UkrainianLyrics.Shared.Payloads;

namespace UkrainianLyrics.WebAPI.Data;

public interface IAppRepository
{
    public IQueryable<Composition> Compositions { get; }

    public IQueryable<Author> Authors { get; }

    public Composition GetComposition(Guid id);

    public Task<Composition> GetCompositionAsync(Guid id);

    public Author GetAuthor(Guid id);

    public Task<Author> GetAuthorAsync(Guid id);

    public Composition CreateComposition(CompositionCreatePayload payload);

    public Task<Composition> CreateCompositionAsync(CompositionCreatePayload payload);

    public Author CreateAuthor(AuthorCreatePayload payload);

    public Task<Author> CreateAuthorAsync(AuthorCreatePayload payload);

    public Composition ModifyComposition(Guid id, CompositionModifyPayload payload);

    public Task<Composition> ModifyCompositionAsync(Guid id, CompositionModifyPayload payload);

    public Author ModifyAuthor(Guid id, AuthorModifyPayload payload);

    public Task<Author> ModifyAuthorAsync(Guid id, AuthorModifyPayload payload);

    public Composition DeleteComposition(Guid id);

    public Task<Composition> DeleteCompositionAsync(Guid id);

    public Composition DeleteComposition(Composition composition);

    public Task<Composition> DeleteCompositionAsync(Composition composition);

    public Author DeleteAuthor(Guid id);

    public Task<Author> DeleteAuthorAsync(Guid id);

    public Author DeleteAuthor(Author author);

    public Task<Author> DeleteAuthorAsync(Author author);
}