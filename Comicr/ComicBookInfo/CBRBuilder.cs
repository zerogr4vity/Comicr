using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ComicBookInfo
{
    /// <summary>A <see cref="ComicInfo.Builder" /> that creates rar(.cbr) archives.</summary>
    public class CBRBuilder : ComicInfo.Builder
    {
        #region Constructors
        /// <inheritdoc />
        public CBRBuilder(string directory, string title = null)
            : base(directory, title) { }
        #endregion

        #region ComicInfo.Builder Overrides
        /// <inheritdoc />
        public override async Task GenerateComicAsync(CancellationToken ct = default)
        {
            // generate the metadata file.
            await base.GenerateComicAsync(ct);

            // only select files that are not .cbz archives.
            Regex regex = new("^\\.cbr$", RegexOptions.IgnoreCase);
            var files = Directory.GetFiles(Directory.GetCurrentDirectory())
                .Where(file => !regex.IsMatch(Path.GetExtension(file)));

            // open a new zip archive.
            string cbrPath = Path.Combine(Directory.GetCurrentDirectory(), Title + ".cbr");
            
            // TODO: generate RAR file
        }
        #endregion
    }
}
