using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ComicBookInfo
{
    /// <summary>A <see cref="ComicInfo.Builder" /> that creates zip(.cbz) archives.</summary>
    public class CBZBuilder : ComicInfo.Builder
    {
        #region Constructors
        /// <inheritdoc />
        public CBZBuilder(string directory, string title = null)
            : base(directory, title){ }
        #endregion

        #region ComicInfo.Builder Overrides
        /// <inheritdoc />
        public override async Task GenerateComicAsync(CancellationToken ct = default)
        {
            // generate the metadata file.
            await base.GenerateComicAsync(ct);

            // only select files that are not .cbz archives.
            Regex regex = new("^\\.cbz$", RegexOptions.IgnoreCase);
            var files = Directory.GetFiles(Directory.GetCurrentDirectory())
                .Where(file => !regex.IsMatch(Path.GetExtension(file)));

            // open a new zip archive.
            string cbzPath = Path.Combine(Directory.GetCurrentDirectory(), Title + ".cbz");
            using FileStream zipToOpen = new(cbzPath, FileMode.Create);
            using ZipArchive archive = new(zipToOpen, ZipArchiveMode.Create);
            foreach (var file in files)
            {
                // create the zip archive entry.
                var entryName = Path.GetFileName(file);
                var entry = archive.CreateEntry(entryName);
                entry.LastWriteTime = File.GetLastWriteTime(file);

                // write the file into the archive entry.
                using var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var stream = entry.Open();
                await fs.CopyToAsync(stream, 81920, ct);
            }
        }
        #endregion
    }
}
