using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ComicBookInfo
{
    /// <summary>A <see cref="ComicInfo.Builder" /> that creates 7z(.cb7) archives.</summary>
    public class CB7Builder : ComicInfo.Builder
    {
        #region Constructors
        /// <inheritdoc />
        public CB7Builder(string directory, string title = null)
            : base(directory, title) { }
        #endregion

        #region ComicInfo.Builder Overrides
        /// <inheritdoc />
        public override async Task GenerateComicAsync(CancellationToken ct = default)
        {
            // generate the metadata file.
            await base.GenerateComicAsync(ct);

            // only select files that are not .cbz archives.
            Regex regex = new("^\\.cb7$", RegexOptions.IgnoreCase);
            var files = Directory.GetFiles(Directory.GetCurrentDirectory())
                .Where(file => !regex.IsMatch(Path.GetExtension(file)));

            // open a new zip archive.
            string cb7Path = Path.Combine(Directory.GetCurrentDirectory(), Title + ".cb7");

            // TODO: generate 7z file
        }
        #endregion
    }
}
