using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage;
using UkrainianLyrics.Shared.Models;
using UkrainianLyrics.Shared.Payloads;

namespace UkrainianLyrics.WebAPI.Data;

public class AppRepository : IAppRepository
{
    private readonly AppDbContext dbContext;

    public AppRepository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public IQueryable<Composition> Compositions => dbContext.Compositons;

    public IQueryable<Author> Authors => dbContext.Authors;

    public Author CreateAuthor(AuthorCreatePayload payload)
    {
        var model = new Author { Name = payload.Name, Description = payload.Description, ImageUri = payload.ImageUri };

        dbContext.Authors.Add(model);
        dbContext.SaveChanges();

        return model;
    }

    public async Task<Author> CreateAuthorAsync(AuthorCreatePayload payload)
    {
        var model = new Author { Name = payload.Name, Description = payload.Description, ImageUri = payload.ImageUri };

        await dbContext.Authors.AddAsync(model);
        await dbContext.SaveChangesAsync();

        return model;
    }

    public Composition CreateComposition(CompositionCreatePayload payload)
    {
        var author = GetAuthor(payload.AuthorId);

        var model = new Composition { Name = payload.Name, Author = author, Description = payload.Description };

        author.Writings.Add(model);

        dbContext.Compositons.Add(model);
        dbContext.SaveChanges();

        return model;
    }

    public async Task<Composition> CreateCompositionAsync(CompositionCreatePayload payload)
    {
        var author = await GetAuthorAsync(payload.AuthorId);

        var model = new Composition { Name = payload.Name, Author = author, Description = payload.Description };

        author.Writings.Add(model);

        await dbContext.Compositons.AddAsync(model);
        await dbContext.SaveChangesAsync();

        return model;
    }

    public Author DeleteAuthor(Guid id)
    {
        var model = GetAuthor(id);

        dbContext.Authors.Remove(model);
        dbContext.SaveChanges();

        return model;
    }

    public Author DeleteAuthor(Author author)
    {
        dbContext.Authors.Remove(author);
        dbContext.SaveChanges();

        return author;
    }

    public async Task<Author> DeleteAuthorAsync(Guid id)
    {
        var model = await GetAuthorAsync(id);

        dbContext.Authors.Remove(model);
        await dbContext.SaveChangesAsync();

        return model;
    }

    public async Task<Author> DeleteAuthorAsync(Author author)
    {
        dbContext.Authors.Remove(author);
        await dbContext.SaveChangesAsync();

        return author;
    }

    public Composition DeleteComposition(Guid id)
    {
        var model = GetComposition(id);

        dbContext.Compositons.Remove(model);
        dbContext.SaveChanges();

        return model;
    }

    public Composition DeleteComposition(Composition composition)
    {
        dbContext.Compositons.Remove(composition);
        dbContext.SaveChanges();

        return composition;
    }

    public async Task<Composition> DeleteCompositionAsync(Guid id)
    {
        var model = await GetCompositionAsync(id);

        dbContext.Compositons.Remove(model);
        await dbContext.SaveChangesAsync();

        return model;
    }

    public async Task<Composition> DeleteCompositionAsync(Composition composition)
    {
        dbContext.Compositons.Remove(composition);
        await dbContext.SaveChangesAsync();

        return composition;
    }

    public Author GetAuthor(Guid id)
    {
        var model = dbContext.Authors.Find(id);

        if (model is null)
            throw new Exception($"Author with id: {id} not found!");

        return model;
    }

    public async Task<Author> GetAuthorAsync(Guid id)
    {
        var model = await dbContext.Authors.FindAsync(id);

        if (model is null)
            throw new Exception($"Author with id: {id} not found!");

        return model;
    }

    public Composition GetComposition(Guid id)
    {
        var model = dbContext.Compositons.Find(id);

        if (model is null)
            throw new Exception($"Composition with id: {id} not found!");

        return model;
    }

    public async Task<Composition> GetCompositionAsync(Guid id)
    {
        var model = await dbContext.Compositons.FindAsync(id);

        if (model is null)
            throw new Exception($"Composition with id: {id} not found!");

        return model;
    }

    public Author ModifyAuthor(Guid id, AuthorModifyPayload payload)
    {
        var model = GetAuthor(id);

        if (payload.Name is not null) model.Name = payload.Name;
        if (payload.Description is not null) model.Description = payload.Description;
        if (payload.ImageUri is not null) model.ImageUri = payload.ImageUri;

        dbContext.SaveChanges();

        return model;
    }

    public async Task<Author> ModifyAuthorAsync(Guid id, AuthorModifyPayload payload)
    {
        var model = await GetAuthorAsync(id);

        if (payload.Name is not null) model.Name = payload.Name;
        if (payload.Description is not null) model.Description = payload.Description;
        if (payload.ImageUri is not null) model.ImageUri = payload.ImageUri;

        await dbContext.SaveChangesAsync();

        return model;
    }

    public Composition ModifyComposition(Guid id, CompositionModifyPayload payload)
    {
        var model = GetComposition(id);

        if (payload.Name is not null) model.Name = payload.Name;

        if (payload.AuthorId is not null)
        {
            model.Author.Writings.Remove(model);

            model.Author = GetAuthor(payload.AuthorId.Value);

            model.Author.Writings.Add(model);
        }

        if (payload.Description is not null) model.Description = payload.Description;

        dbContext.SaveChanges();

        return model;
    }

    public async Task<Composition> ModifyCompositionAsync(Guid id, CompositionModifyPayload payload)
    {
        var model = await GetCompositionAsync(id);

        if (payload.Name is not null) model.Name = payload.Name;

        if (payload.AuthorId is not null)
        {
            model.Author.Writings.Remove(model);

            model.Author = await GetAuthorAsync(payload.AuthorId.Value);

            model.Author.Writings.Add(model);
        }

        if (payload.Description is not null) model.Description = payload.Description;

        await dbContext.SaveChangesAsync();

        return model;
    }
}