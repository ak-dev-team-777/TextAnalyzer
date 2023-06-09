using TextAnalyzer.Core.Services.WordExtractor;

namespace TextAnalyzer.Core.Tests.Services.WordCounting
{
    public class WordCountingServiceTests
    {
        private const int EventTimeout = 10;

        public static string GetTestPath(string path) => Path.Combine(Directory.GetCurrentDirectory(), path);

        [Fact]
        public async void EmptyFile()
        {
            // arrange
            var filePath = GetTestPath(@"Services\WordCounting\Data\Empty.txt");
            var expectedReceivedEvents = new List<int> { 0, 100 };

            var service = new WordExtractorService();
            var receivedEvents = new List<int>();
            service.ProgressChanged += (sender, e) =>
            {
                receivedEvents.Add(e.ProgressPercentage);
            };

            // act
            var result = await service.ExtractCountWords(filePath, new CancellationToken());

            // assert
            Assert.Equal(expectedReceivedEvents, receivedEvents);
            Assert.Empty(result);
        }

        [Fact]
        public async void TextWithDifferentSpaces()
        {
            // arrange
            var filePath = GetTestPath(@"Services\WordCounting\Data\DifferentSpaces.txt");
            var expectedReceivedEvents = new List<int> { 0, 100 };
            var expectedResult = new Dictionary<string, int>
            {
                { "test", 3 },
                { "first", 1 },
                { "second", 1 },
                { "third", 1 }
            };
            var service = new WordExtractorService();
            var receivedEvents = new List<int>();
            service.ProgressChanged += (sender, e) =>
            {
                receivedEvents.Add(e.ProgressPercentage);
            };

            // act
            var result = await service.ExtractCountWords(filePath, new CancellationToken());

            // assert
            Assert.Equal(expectedReceivedEvents, receivedEvents);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async void MultilineTextWithDifferentSpaces()
        {
            // arrange
            var filePath = GetTestPath(@"Services\WordCounting\Data\Multiline.txt");

            var expectedReceivedEvents = new List<int>();
            for (var i = 0; i <= 100; i++)
                expectedReceivedEvents.Add(i);

            var expectedResult = new Dictionary<string, int>
            {
                { "test", 1103496 },
                { "first", 367832 },
                { "second", 367832 },
                { "third", 367832 }
            };

            var service = new WordExtractorService();
            var receivedEvents = new List<int>();
            service.ProgressChanged += (sender, e) =>
            {
                receivedEvents.Add(e.ProgressPercentage);
            };

            // act
            var result = await service.ExtractCountWords(filePath, new CancellationToken());

            // assert
            Assert.Equal(expectedReceivedEvents, receivedEvents);
            Assert.Equal(expectedResult, result);
        }
    }
}