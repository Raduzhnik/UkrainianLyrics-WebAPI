using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using UkrainianLyrics.Shared.Models;
using UkrainianLyrics.Shared.Payloads;
using LuceneDirectory = Lucene.Net.Store.Directory;

namespace UkrainianLyrics.WebAPI.Data;

public class LuceneSearchEngine
{
    public const LuceneVersion Version = LuceneVersion.LUCENE_48;
    private readonly ILogger<LuceneSearchEngine> logger;
    private readonly IServiceProvider serviceProvider;

    public static string IndexDirectoryPath { get => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UkrainianLyrics-WebAPI", "Search-Index"); }

    public static string IndexPath { get => Path.Combine(IndexDirectoryPath, "Ukrainian-Lyrics-Index"); }

    private Analyzer standartAnalyzer = new StandardAnalyzer(Version);

    private IndexWriter writer;

    public LuceneSearchEngine(ILogger<LuceneSearchEngine> logger, IServiceProvider serviceProvider)
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;

        logger.LogInformation("Search index is based at: {Search index path}", IndexPath);

        LuceneDirectory indexDirectory = FSDirectory.Open(IndexPath);

        IndexWriterConfig indexConfig = new IndexWriterConfig(Version, standartAnalyzer);
        indexConfig.OpenMode = OpenMode.CREATE_OR_APPEND;
        writer = new IndexWriter(indexDirectory, indexConfig);
    }

    public void AddToIndex(Author author)
    {
        Document doc = new();

        doc.Add(new StringField("Type", "Author", Field.Store.YES));
        doc.Add(new StringField("Id", author.Id.ToString(), Field.Store.YES));
        doc.Add(new TextField("Name", author.Name, Field.Store.YES));
        doc.Add(new TextField("Description", author.Description, Field.Store.YES));

        writer.AddDocument(doc);
        writer.Commit();
    }

    public void AddToIndex(Composition composition)
    {
        Document doc = new();

        doc.Add(new StringField("Type", "Composition", Field.Store.YES));
        doc.Add(new StringField("Id", composition.Id.ToString(), Field.Store.YES));
        doc.Add(new TextField("Name", composition.Name, Field.Store.YES));
        doc.Add(new TextField("Description", composition.Description, Field.Store.YES));

        writer.AddDocument(doc);
        writer.Commit();
    }

    public void RemoveFromIndex(Guid id)
    {
        writer.DeleteDocuments(new Term("Id", id.ToString()));
        writer.Commit();
    }

    public void ModifyIndex(Composition composition)
    {
        Document doc = new();

        doc.Add(new StringField("Type", "Composition", Field.Store.YES));
        doc.Add(new StringField("Id", composition.Id.ToString(), Field.Store.YES));
        doc.Add(new TextField("Name", composition.Name, Field.Store.YES));
        doc.Add(new TextField("Description", composition.Description, Field.Store.YES));

        writer.UpdateDocument(new Term("Id", composition.Id.ToString()), doc);
        writer.Commit();
    }

    public void ModifyIndex(Author author)
    {
        Document doc = new();

        doc.Add(new StringField("Type", "Author", Field.Store.YES));
        doc.Add(new StringField("Id", author.Id.ToString(), Field.Store.YES));
        doc.Add(new TextField("Name", author.Name, Field.Store.YES));
        doc.Add(new TextField("Description", author.Description, Field.Store.YES));

        writer.UpdateDocument(new Term("Id", author.Id.ToString()), doc);
        writer.Commit();
    }

    public IEnumerable<object> TextSearch(string searchTerms, int paging = 50)
    {
        using DirectoryReader reader = writer.GetReader(true);
        IndexSearcher searcher = new(reader);

        MultiPhraseQuery query = new();

        query.Add(new Term[]
        {
            new Term("Id", searchTerms),
            new Term("Name", searchTerms),
            new Term("Description", searchTerms)
        });

        TopDocs topDocs = searcher.Search(query, paging);

        var result = new List<object>();

        foreach (var scoreDoc in topDocs.ScoreDocs)
        {
            Document doc = searcher.Doc(scoreDoc.Doc);
            string docType = doc.Get("Type");

            if (docType == "Composition")
                result.Add(serviceProvider.GetRequiredService<IAppRepository>().GetComposition(Guid.Parse(doc.Get("Id"))));
            else if (docType == "Author")
                result.Add(serviceProvider.GetRequiredService<IAppRepository>().GetAuthor(Guid.Parse(doc.Get("Id"))));
            else
                throw new Exception($"Something went really wrong");
        }

        return result;
    }
}