using System.IO;
using System.Threading.Tasks;

namespace SearchParty.FileProviders
{
    public interface IReadOnlyFileProvider
    {
        Task<Stream> GetReadStreamAsync(string filename);
    }
}
