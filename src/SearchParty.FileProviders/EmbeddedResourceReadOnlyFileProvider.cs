using System.IO;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;

namespace SearchParty.FileProviders
{
    public class EmbeddedResourceReadOnlyFileProvider : IReadOnlyFileProvider
    {
        private readonly string _assemblyName;
        public string ResourceFolder { get; }

        public EmbeddedResourceReadOnlyFileProvider(string assemblyName)
        {
            _assemblyName = assemblyName;
            ResourceFolder = "Resources";
        }

        /// <summary>
        /// Gets a stream for the resource in the given assembly in the 'Resources' folder.
        /// </summary>
        public Task<Stream> GetReadStreamAsync(string filename)
        {
            filename = CleanFileName(filename);

            var assembly = Assembly.Load(new AssemblyName(_assemblyName));
            var stream = assembly.GetManifestResourceStream($"{_assemblyName}.{ResourceFolder}.{filename}");
            if (stream == null) throw new MissingManifestResourceException($"Resource '{filename}' not found in '{_assemblyName}.{ResourceFolder}'.");

            return Task.FromResult(stream);
        }

        private static string CleanFileName(string filename)
        {
            return filename
                .Replace('/', '.')
                .Replace('\\', '.');
        }
    }
}
