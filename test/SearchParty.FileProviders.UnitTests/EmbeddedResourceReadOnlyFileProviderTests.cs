using System.Resources;
using System.Threading.Tasks;
using Xunit;

namespace SearchParty.FileProviders.UnitTests
{
    public class EmbeddedResourceReadOnlyFileProviderTests
    {
        private readonly EmbeddedResourceReadOnlyFileProvider _sut;

        public EmbeddedResourceReadOnlyFileProviderTests()
        {
            _sut = new EmbeddedResourceReadOnlyFileProvider($"{nameof(SearchParty)}.{nameof(FileProviders)}.{nameof(UnitTests)}");
        }

        [Fact]
        public async Task GetReadStream_ShouldReturnStreamAsync_WhenEmbeddedResourceInResourcesDirectory()
        {
            var stream = await _sut.GetReadStream("TestResourceFile.txt");

            Assert.NotNull(stream);
        }

        [Fact]
        public async Task GetReadStream_ShouldThrowException_WhenResourceNotFoundAsync()
        {
            await Assert.ThrowsAsync<MissingManifestResourceException>(async () => await _sut.GetReadStream("No resource here"));
        }

        [Fact]
        public async Task GetReadStream_ShouldAccessFilesInFolders_WithForwardSlash()
        {
            var stream = await _sut.GetReadStream("AnotherFolder/TestResourceFile.txt");

            Assert.NotNull(stream);
        }

        [Fact]
        public async Task GetReadStream_ShouldAccessFilesInFolders_WithBackSlash()
        {
            var stream = await _sut.GetReadStream("AnotherFolder\\TestResourceFile.txt");

            Assert.NotNull(stream);
        }
    }
}
