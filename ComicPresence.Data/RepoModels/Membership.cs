using System;
namespace ComicPresence.Data.RepoModels
{
    public class Membership
    {
        public Guid ApplicationId { get; set; }
        public Guid UserId { get; set; }
        public int AuthorId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
    }
}
