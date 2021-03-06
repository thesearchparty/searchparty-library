﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace SearchParty.FileProviders
{
    /// <remarks>
    /// Modified from https://github.com/aspnet/FileSystem/blob/dev/src/Microsoft.Extensions.FileProviders.Physical/PhysicalFileProvider.cs
    /// </remarks>
    public class FileSystemReadOnlyFileProvider : IReadOnlyFileProvider
    {
        public string Root { get; }

        private static readonly char[] PathSeparators = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

        public FileSystemReadOnlyFileProvider(string root)
        {
            if (string.IsNullOrWhiteSpace(root)) throw new ArgumentNullException(nameof(root));

            // When we do matches in GetFullPath, we want to only match full directory names.
            Root = EnsureTrailingSlash(root);
            if (!Directory.Exists(Root))
            {
                throw new DirectoryNotFoundException(Root);
            }
        }

        public Task<Stream> GetReadStreamAsync(string filename)
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException(nameof(filename));

            // Relative paths starting with leading slashes are okay
            filename = filename.TrimStart(PathSeparators);

            var fullPath = Path.Combine(Root, filename);

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            if (stream == null || !stream.CanRead)
                throw new FileNotFoundException($"Cannot read file '{fullPath}'");

            return Task.FromResult((Stream)stream);
        }

        private static string EnsureTrailingSlash(string path)
        {
            if (!string.IsNullOrEmpty(path) &&
                path[path.Length - 1] != Path.DirectorySeparatorChar)
            {
                return path + Path.DirectorySeparatorChar;
            }

            return path;
        }
    }
}
