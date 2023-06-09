using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace TextAnalyzer.Core.Services.WordExtractor
{
    public class WordExtractorService : IWordExtractorService
    {
        private int? _NotifiedProgress;

        public event EventHandler<ProgressChangedEventArgs>? ProgressChanged;

        public async Task<IDictionary<string, int>> ExtractCountWords(string filePath, CancellationToken cancellationToken)
        {
            try
            {
                OnProgressChanged(0);

                await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using var reader = new StreamReader(stream, Encoding.ASCII);
                if (reader.EndOfStream)
                {
                    OnProgressChanged(100);
                    return new Dictionary<string, int>(); ;
                }

                var result = new Dictionary<string, int>();
                while (!reader.EndOfStream)
                {
                    if (cancellationToken.IsCancellationRequested)
                        throw new OperationCanceledException();

                    var line = await reader.ReadLineAsync(cancellationToken);
                    if (line == null)
                        continue;

                    var words = line.Split(new [] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var word in words)
                    {
                        if (result.ContainsKey(word))
                        {
                            result[word] += 1;
                        }
                        else
                        {
                            result.Add(word, 1);
                        }
                    }

                    OnProgressChanged(Convert.ToInt32((double)stream.Position / stream.Length * 100));
                }
                return result.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            }
            catch (OperationCanceledException)
            {
                OnProgressChanged(0);
                return new Dictionary<string, int>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new Dictionary<string, int>();
            }
        }

        protected virtual void OnProgressChanged(int progress)
        {
            if (_NotifiedProgress == progress)
                return;

            ProgressChanged?.Invoke(this, new ProgressChangedEventArgs(progress, null));
            _NotifiedProgress = progress;
        }
    }
}
