using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.S3;
using SearchParty.FileProviders;

namespace SearchParty.Amazon.S3
{
    public class S3ReadOnlyFileProvider : IReadOnlyFileProvider
    {
        private readonly string _bucketName;
        private readonly IAmazonS3 _s3Client;

        /// <summary>
        /// Creates a new instance of S3ReadOnlyFileProvider. Requires AWS DI extensions to be set.
        /// </summary>
        /// <param name="s3Client">Amazon S3 Client, normally set up using AWS extension methods.</param>
        /// <param name="bucketName">Base bucket name.</param>
        public S3ReadOnlyFileProvider(IAmazonS3 s3Client, string bucketName)
        {
            if (string.IsNullOrWhiteSpace(bucketName)) throw new ArgumentNullException(nameof(bucketName));
            _bucketName = bucketName;
            _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client), $"{nameof(s3Client)} is null, have you enabled DI - services.AddAWSService<IAmazonS3>()");
        }

        /// <summary>
        /// An open stream read from to get the data from S3. In order to
        /// use this stream without leaking the underlying resource, please
        /// wrap access to the stream within a using block.
        /// </summary>
        public async Task<Stream> GetReadStream(string filename)
        {
            var stream = await _s3Client.GetObjectStreamAsync(_bucketName, filename, null);
            if(stream == null || !stream.CanRead)
                throw new HttpRequestException($"Cannot read stream from '{filename}' in S3 bucket '{_bucketName}'");
            return stream;
        }
    }
}
