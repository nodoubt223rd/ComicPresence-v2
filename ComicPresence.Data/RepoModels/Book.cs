
namespace ComicPresence.Data.RepoModels
{
    public class Book
    {
        public int BookId { get; set; }
        public int AuthorId { get; set; }
        public string BookTitle { get; set; }
        public string BookSlug { get; set; } 
    }
}
