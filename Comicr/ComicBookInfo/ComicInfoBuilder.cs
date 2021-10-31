using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ComicBookInfo
{
    /// <summary>Contains information to make a comic.</summary>
    public partial class ComicInfo
    {
        /// <summary>Builds a <see cref="ComicInfo" />.</summary>
        public abstract class Builder : ComicInfo
        {
            #region Private Fields
            private List<ComicPageInfo> _pages;
            #endregion

            #region Constructors
            /// <summary>Creates a <see cref="Builder" /> from a previous comic workspace.</summary>
            /// <param name="directory">The workspace.</param>
            /// <param name="title">The comic title. If provided, the builder will open a new comic workspace.</param>
            protected Builder(string directory, string title = null)
            {
                OpenWorkspace(directory, title);
            }
            #endregion

            #region Mutators
            public virtual Builder SetTitle(string title)
            {
                // ensure title is not empty.
                if (string.IsNullOrWhiteSpace(title))
                    throw new ArgumentNullException(nameof(title), "Must provide a comic title.");

                Title = title;
                return this;
            }

            public virtual Builder SetSeries(string series)
            {
                Series = series;
                return this;
            }

            public virtual Builder SetNumber(string number)
            {
                Number = number;
                return this;
            }

            public virtual Builder SetCount(int count = -1)
            {
                Count = count;
                return this;
            }

            public virtual Builder SetVolume(int volume = -1)
            {
                Volume = volume;
                return this;
            }

            public virtual Builder SetAlternateSeries(string alternateSeries)
            {
                AlternateSeries = alternateSeries;
                return this;
            }

            public virtual Builder SetAlternateNumber(string alternateNumber)
            {
                AlternateNumber = alternateNumber;
                return this;
            }

            public virtual Builder SetAlternateCount(int alternateCount = -1)
            {
                AlternateCount = alternateCount;
                return this;
            }

            public virtual Builder SetSummary(string summary)
            {
                Summary = summary;
                return this;
            }

            public virtual Builder SetNotes(string notes)
            {
                Notes = notes;
                return this;
            }

            protected virtual Builder SetYear(int year = -1)
            {
                Year = year;
                return this;
            }

            protected virtual Builder SetMonth(int month = -1)
            {
                Month = month;
                return this;
            }

            public virtual Builder SetDate(DateTime date = default)
            {
                // ensure date is not the default value.
                if (date == default)
                    date = DateTime.Now;

                // set the date.
                SetYear(date.Year);
                SetMonth(date.Month);

                return this;
            }

            public virtual Builder SetWriter(string writer)
            {
                Writer = writer;
                return this;
            }

            public virtual Builder SetPenciller(string penciller)
            {
                Penciller = penciller;
                return this;
            }

            public virtual Builder SetInker(string inker)
            {
                Inker = inker;
                return this;
            }

            public virtual Builder SetColorist(string colorist)
            {
                Colorist = colorist;
                return this;
            }

            public virtual Builder SetLetterer(string letterer)
            {
                Letterer = letterer;
                return this;
            }

            public virtual Builder SetCoverArtist(string coverArtist)
            {
                CoverArtist = coverArtist;
                return this;
            }

            public virtual Builder SetEditor(string editor)
            {
                Editor = editor;
                return this;
            }

            public virtual Builder SetPublisher(string publisher)
            {
                Publisher = publisher;
                return this;
            }

            public virtual Builder SetImprint(string imprint)
            {
                Imprint = imprint;
                return this;
            }

            public virtual Builder SetGenre(string genre)
            {
                Genre = genre;
                return this;
            }

            public virtual Builder SetWeb(string web)
            {
                Web = web;
                return this;
            }

            protected virtual Builder SetPageCount(int pageCount = 0)
            {
                PageCount = pageCount;
                return this;
            }

            public virtual Builder SetLanguageISO(string languageISO)
            {
                LanguageISO = languageISO;
                return this;
            }

            public virtual Builder SetFormat(string format)
            {
                Format = format;
                return this;
            }

            public virtual Builder SetBlackAndWhite(YesNo isBlackAndWhite = YesNo.Unknown)
            {
                BlackAndWhite = isBlackAndWhite;
                return this;
            }

            public virtual Builder SetManga(Manga manga = Manga.Unknown)
            {
                Manga = manga;
                return this;
            }

            public virtual Builder SetCharacters(string characters)
            {
                Characters = characters;
                return this;
            }

            public virtual Builder SetTeams(string teams)
            {
                Teams = teams;
                return this;
            }

            public virtual Builder SetLocations(string locations)
            {
                Locations = locations;
                return this;
            }

            public virtual Builder SetScanInformation(string scanInfo)
            {
                ScanInformation = scanInfo;
                return this;
            }

            public virtual Builder SetStoryArc(string storyArc)
            {
                StoryArc = storyArc;
                return this;
            }

            public virtual Builder SetSeriesGroup(string seriesGroup)
            {
                SeriesGroup = seriesGroup;
                return this;
            }

            public virtual Builder SetAgeRating(AgeRating ageRating = AgeRating.Unknown)
            {
                AgeRating = ageRating;
                return this;
            }

            /// <summary>Overwrites the current list of pages.</summary>
            /// <param name="pages">The new comic pages.</param>
            public virtual Builder SetPages(ComicPageInfo[] pages)
            {
                _pages = pages.Length > 0 ? new(pages) : new();
                return SetPageCount(_pages.Count);
            }
            #endregion

            #region Methods
            public virtual void OpenWorkspace(string directory, string title = "")
            {
                // ensure a workspace directory is given.
                if (string.IsNullOrWhiteSpace(directory))
                    throw new ArgumentNullException(nameof(directory), "Must provide a directory to open.");

                // ensure the directory is on the device.
                if (Directory.Exists(directory))
                {
                    // open any existing metadata file and write the data to this object.
                    if (ReadMetadataFile(directory) is ComicInfo info)
                    {
                        SetTitle(info.Title);
                        SetSeries(info.Series);
                        SetNumber(info.Number);
                        SetCount(info.Count);
                        SetVolume(info.Volume);
                        SetAlternateSeries(info.AlternateSeries);
                        SetAlternateNumber(info.AlternateNumber);
                        SetAlternateCount(info.AlternateCount);
                        SetSummary(info.Summary);
                        SetNotes(info.Notes);
                        SetYear(info.Year);
                        SetMonth(info.Month);
                        SetWriter(info.Writer);
                        SetPenciller(info.Penciller);
                        SetInker(info.Inker);
                        SetColorist(info.Colorist);
                        SetLetterer(info.Letterer);
                        SetCoverArtist(info.CoverArtist);
                        SetEditor(info.Editor);
                        SetPublisher(info.Publisher);
                        SetImprint(info.Imprint);
                        SetGenre(info.Genre);
                        SetWeb(info.Web);
                        SetPageCount(info.PageCount);
                        SetLanguageISO(info.LanguageISO);
                        SetFormat(info.Format);
                        SetBlackAndWhite(info.BlackAndWhite);
                        SetManga(info.Manga);
                        SetCharacters(info.Characters);
                        SetTeams(info.Teams);
                        SetLocations(info.Locations);
                        SetScanInformation(info.ScanInformation);
                        SetStoryArc(info.StoryArc);
                        SetSeriesGroup(info.SeriesGroup);
                        SetAgeRating(info.AgeRating);
                        SetPages(info.Pages);
                    }
                }
                else
                {
                    // create a new workspace and give it a name.
                    Directory.CreateDirectory(directory);
                    SetTitle(title);
                }

                // open the directory
                Directory.SetCurrentDirectory(directory);
            }

            protected virtual ComicPageInfo GenerateComicPageInfo(string key, int imageWidth, int imageHeight, ComicPageType type = ComicPageType.Story, bool isDoublePage = false, long imageSize = 0)
            {
                return new ComicPageInfo.Builder(key, imageWidth, imageHeight)
                    .SetType(type)
                    .SetDoublePage(isDoublePage)
                    .SetImageSize(imageSize)
                    .Build();
            }

            public virtual void AddPage(string key, int imageWidth, int imageHeight, ComicPageType type = ComicPageType.Story, bool isDoublePage = false, long imageSize = 0)
            {
                ComicPageInfo page = GenerateComicPageInfo(key, imageWidth, imageHeight, type, isDoublePage, imageSize);
                AddPage(page);
            }

            public virtual void AddPage(ComicPageInfo page)
            {
                if (!_pages.Contains(page))
                {
                    _pages.Add(page);
                }
            }

            public virtual void AddPageFromSource(string sourceDirectory, string key, int imageWidth, int imageHeight, ComicPageType type = ComicPageType.Story, bool isDoublePage = false, long imageSize = 0)
            {
                ComicPageInfo page = GenerateComicPageInfo(key, imageWidth, imageHeight, type, isDoublePage, imageSize);
                AddPageFromSource(sourceDirectory, page);
            }

            public virtual void AddPageFromSource(string sourceDirectory, ComicPageInfo page)
            {
                // ensure the source directory exists on the device.
                if (string.IsNullOrWhiteSpace(sourceDirectory))
                    throw new ArgumentNullException(nameof(sourceDirectory), "Must provide a directory to open.");
                else if (!Directory.Exists(sourceDirectory))
                    throw new DirectoryNotFoundException($"\"{sourceDirectory}\" does not exist on the device.");

                // ensure the image is at the source.
                string sourcePath = Path.Combine(sourceDirectory, page.Key);
                if (!File.Exists(sourcePath))
                    throw new FileNotFoundException("The page was not found.", page.Key);

                // if page is not in the workspace, move image to the workspace.
                if (!Directory.GetFiles(Directory.GetCurrentDirectory()).Contains(page.Key))
                {
                    string targetPath = Path.Combine(Directory.GetCurrentDirectory(), page.Key);
                    File.Copy(sourcePath, targetPath);
                }

                // add page to the comic.
                AddPage(page);
            }

            public virtual void MovePageToFront(ComicPageInfo pageInfo)
            {
                if (_pages.IndexOf(pageInfo) > 0)
                {
                    _pages.Remove(pageInfo);
                    _pages.Prepend(pageInfo);
                }
            }

            public virtual void MovePageForward(ComicPageInfo pageInfo)
            {
                int index = _pages.IndexOf(pageInfo);
                if (index > 0)
                {
                    ComicPageInfo temp = _pages[index - 1];
                    _pages[index - 1] = pageInfo;
                    _pages[index] = temp;
                }
            }

            public virtual void MovePageBackward(ComicPageInfo pageInfo)
            {
                int index = _pages.IndexOf(pageInfo);
                if (index < _pages.Count - 1)
                {
                    ComicPageInfo temp = _pages[index + 1];
                    _pages[index + 1] = pageInfo;
                    _pages[index] = temp;
                }
            }

            public virtual void MovePageToBack(ComicPageInfo pageInfo)
            {
                int index = _pages.IndexOf(pageInfo);
                if (index > -1 && index < _pages.Count - 1)
                {
                    _pages.Remove(pageInfo);
                    _pages.Add(pageInfo);
                }
            }

            public virtual bool RemovePage(ComicPageInfo page)
            {
                return _pages.Remove(page);
            }

            public virtual ComicInfo Build()
            {
                // finalize the comic pages and count.
                if (_pages?.Any() ?? false)
                {
                    Pages = _pages.Select((page, index) =>
                    {
                        page.Image = index + 1;
                        return page;
                    }).ToArray();
                }
                else
                {
                    Pages = Array.Empty<ComicPageInfo>();
                }
                SetPageCount(Pages.Length);

                return new(this);
            }
            
            public virtual Task GenerateComicAsync(CancellationToken ct = default)
            {
                // export ComicInfo to metadata file.
                ComicInfo info = Build();
                info.WriteMetadataFile(Directory.GetCurrentDirectory());

                return Task.CompletedTask;
            }

            public virtual void CloseWorkspace()
            {
                // we must call build because we need a parameterless constructor.
                Build().WriteMetadataFile(Directory.GetCurrentDirectory());
            }
            #endregion
        }

        #region Constructors
        private ComicInfo(Builder builder)
        {
            Title = builder.Title;
            Series = builder.Series;
            Number = builder.Number;
            Count = builder.Count;
            Volume = builder.Volume;
            AlternateSeries = builder.AlternateSeries;
            AlternateNumber = builder.AlternateNumber;
            AlternateCount = builder.AlternateCount;
            Summary = builder.Summary;
            Notes = builder.Notes;
            Year = builder.Year;
            Month = builder.Month;
            Writer = builder.Writer;
            Penciller = builder.Penciller;
            Inker = builder.Inker;
            Colorist = builder.Colorist;
            Letterer = builder.Letterer;
            CoverArtist = builder.CoverArtist;
            Editor = builder.Editor;
            Publisher = builder.Publisher;
            Imprint = builder.Imprint;
            Genre = builder.Genre;
            Web = builder.Web;
            PageCount = builder.PageCount;
            LanguageISO = builder.LanguageISO;
            Format = builder.Format;
            BlackAndWhite = builder.BlackAndWhite;
            Manga = builder.Manga;
            Characters = builder.Characters;
            Teams = builder.Teams;
            Locations = builder.Locations;
            ScanInformation = builder.ScanInformation;
            StoryArc = builder.StoryArc;
            SeriesGroup = builder.SeriesGroup;
            AgeRating = builder.AgeRating;
            Pages = builder.Pages;
        }
        #endregion

        #region Methods
        public virtual ComicInfo ReadMetadataFile(string directory)
        {
            ComicInfo result = null;
            
            string path = Path.Combine(directory, $"{nameof(ComicInfo)}.xml");

            if (File.Exists(path))
            {
                using var fileStream = File.OpenRead(path);

                XmlSerializer reader = new(typeof(ComicInfo));
                result = (ComicInfo)reader.Deserialize(fileStream);
            }

            return result;
        }

        public virtual void WriteMetadataFile(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string path = Path.Combine(directory, $"{nameof(ComicInfo)}.xml");
            using var fileStream = File.OpenWrite(path);

            XmlSerializer writer = new(typeof(ComicInfo));
            writer.Serialize(fileStream, this);
        }
        #endregion
    }
}