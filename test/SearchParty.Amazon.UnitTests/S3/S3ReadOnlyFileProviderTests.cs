using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using NSubstitute;
using SearchParty.Amazon.S3;
using Xunit;

namespace SearchParty.Amazon.UnitTests.S3
{
    public class S3ReadOnlyFileProviderTests
    {
        private readonly IAmazonS3 _s3Client;
        private readonly S3ReadOnlyFileProvider _sut;
        private const string BucketName = "XxX";

        public S3ReadOnlyFileProviderTests()
        {
            _s3Client = Substitute.For<IAmazonS3>();
            _sut = new S3ReadOnlyFileProvider(_s3Client, BucketName);
        }

        [Fact]
        public async Task GetReadStream_ShouldCallGetObjectAsync()
        {
            //Arrange
            const string filename = "filename-test";
            _s3Client.GetObjectStreamAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IDictionary<string, object>>())
                .Returns(GetStream());

            //Act
            var stream = await _sut.GetReadStreamAsync(filename);

            //Assert
            await _s3Client.Received().GetObjectStreamAsync(BucketName, filename, null);
        }

        [Fact]
        public async Task GetReadStream_ShouldThrowExceptionWhenObjectNotFoundAsync()
        {
            _s3Client.GetObjectStreamAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IDictionary<string, object>>())
                .Returns(Task.FromResult((Stream)null));
            
            await Assert.ThrowsAsync<HttpRequestException>(async () => await _sut.GetReadStreamAsync("some file"));
        }

        private static Task<Stream> GetStream()
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("RandomString"));
            return Task.FromResult((Stream)stream);
        }
    }
}
