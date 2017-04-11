
namespace ComicPresence.Data.RepoModels
{
    public class Chapter
    {
        public int ParentItem { get; set; }
        public int itemType { get; set; }
        public int ChapterId { get; set; }
        public Book Book { get; set; }
        public int ChapterNumber { get; set; }
        public string ChapterTitle { get; set; }
        public string ChapterSlug { get; set; } 
    }
}
