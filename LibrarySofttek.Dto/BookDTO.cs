using System.ComponentModel.DataAnnotations;

namespace LibrarySofttek.DTOs
{
    public class BookDTO
    {
        [Required(ErrorMessage = "El título del libro es obligatorio.")]
        [StringLength(200, ErrorMessage = "El título no puede exceder los 200 caracteres.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "El año de publicación es obligatorio.")]
        [Range(1000, 2100, ErrorMessage = "El año debe estar entre 1000 y 2100.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "El género es obligatorio.")]
        [StringLength(100, ErrorMessage = "El género no puede exceder los 100 caracteres.")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "La cantidad de páginas es obligatoria.")]
        [Range(1, 10000, ErrorMessage = "El libro debe tener al menos 1 página y no más de 10,000.")]
        public int PageCount { get; set; }

        [Required(ErrorMessage = "El autor es obligatorio.")]
        public int AuthorId { get; set; }
    }
}
