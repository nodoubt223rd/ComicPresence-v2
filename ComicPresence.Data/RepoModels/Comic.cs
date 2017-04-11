using System;
using System.Collections.Generic;

namespace ComicPresence.Data.RepoModels
{
    /// <summary>
    /// Represents a comic object.
    /// </summary>
    public class Comic
    {
        public int ParentItem { get; set; }
        public int ItemType { get; set; }
        public List<Membership> Authors { get; set; }
        public Chapter Chapter { get; set; }
        public string Title { get; set; }
        public string LongDescription { get; set; }
        public string ShortDescription { get; set; }
        public string ComicImage { get; set; }
        public string ThumbImage { get; set; }
        public string BannerImage { get; set; }
        public bool IsArchived { get; set; }
        public bool IsLastComic { get; set; }
        public bool OnHomePage { get; set; }
        public DateTime ComicDate { get; set; }
        public TimeSpan PublishTime { get; set; }
        public string ComicSlug { get; set; }
    }
}
