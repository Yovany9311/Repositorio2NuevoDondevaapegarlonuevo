using System.ComponentModel.DataAnnotations;
namespace LibrarySofttek.Domain
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public string City { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public List<Book> Books { get; set; } = new();
    }
}
