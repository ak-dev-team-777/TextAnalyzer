using System.ComponentModel;

namespace TextAnalyzer.Core.Services.WordExtractor;

public interface IWordExtractorService
{
    event EventHandler<ProgressChangedEventArgs> ProgressChanged;

    Task<IDictionary<string, int>> ExtractCountWords(string path, CancellationToken cancellationToken);
}