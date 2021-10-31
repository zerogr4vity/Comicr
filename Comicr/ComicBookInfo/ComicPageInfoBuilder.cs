using System;

namespace ComicBookInfo
{
    /// <summary>Contains imformation about a comic page.</summary>
    public partial class ComicPageInfo
    {
        /// <summary>Builds a <see cref="ComicPageInfo" />.</summary>
        public class Builder : ComicPageInfo
        {
            #region Constructors
            public Builder(string key, int imageWidth, int imageHeight)
            {
                SetKey(key);
                SetImageWidth(imageWidth);
                SetImageHeight(imageHeight);
            }
            #endregion

            #region Mutators
            public virtual Builder SetKey(string key)
            {
                if (string.IsNullOrWhiteSpace(key))
                    throw new ArgumentNullException(nameof(key), "Must provide a key.");

                Key = key;
                return this;
            }

            public virtual Builder SetImageWidth(int imageWidth = -1)
            {
                ImageWidth = imageWidth;
                return this;
            }

            public virtual Builder SetImageHeight(int imageHeight = -1)
            {
                ImageHeight = imageHeight;
                return this;
            }

            public virtual Builder SetType(ComicPageType type = ComicPageType.Story)
            {
                Type = type;
                return this;
            }

            public virtual Builder SetDoublePage(bool isDoublePage = false)
            {
                DoublePage = isDoublePage;
                return this;
            }

            public virtual Builder SetImageSize(long imageSize = 0)
            {
                ImageSize = imageSize;
                return this;
            }
            #endregion

            #region Methods
            public ComicPageInfo Build() => new(this);
            #endregion
        }

        #region Constructors
        protected ComicPageInfo(Builder builder)
        {
            Key = builder.Key;
            ImageWidth = builder.ImageWidth;
            ImageHeight = builder.ImageHeight;
            Type = builder.Type;
            DoublePage = builder.DoublePage;
            ImageSize = builder.ImageSize;
        }
        #endregion

        #region System.Object Overrides
        public override int GetHashCode()
            => HashCode.Combine(Key, ImageWidth, ImageWidth);

        public override bool Equals(object obj) => obj is ComicPageInfo rhs
            && Key == rhs.Key
            && ImageWidth == rhs.ImageWidth
            && ImageHeight == rhs.ImageHeight;
        #endregion
    }
}
