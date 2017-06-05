using System;
using System.Resources;
using System.Threading.Tasks;
using Xunit;

namespace SearchParty.FileProviders.UnitTests
{
    public class FileSystemReadOnlyFileProviderTests
    {
        [Fact]
        public void Constructor_Fails_WithWhitespace()
        {
            Assert.Throws<ArgumentNullException>(() => 
                new FileSystemReadOnlyFileProvider("")
            );
        }

        [Fact]
        public void Constructor_Works_WithRelativePath()
        {
            var path = String.Format("..{0}", System.IO.Path.PathSeparator);
            var sut = new FileSystemReadOnlyFileProvider(path);

            Assert.NotNull(sut);
        }

    }
}