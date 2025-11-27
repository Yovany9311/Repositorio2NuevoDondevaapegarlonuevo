using LibrarySofttek.DTOs;
using LibrarySofttek.Exceptions;
using LibrarySofttek.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LibrarySofttek.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly ILogger<BooksController> _logger;

        public BooksController(IBookService bookService, IAuthorService authorService, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _authorService = authorService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var books = _bookService.GetAllBooks();
                return View(books);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error en Index de BooksController: {Message}", ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Create()
        {
            try
            {
                ViewBag.Authors = _authorService.GetAllAuthors();
                return View();
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error en Create de BooksController: {Message}", ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult Create(BookDTO bookDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Authors = _authorService.GetAllAuthors();
                return View(bookDto);
            }

            try
            {
                _bookService.AddBook(bookDto);
                return RedirectToAction("Index");
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Error de negocio en Create: {Message}", ex.Message);
                ViewBag.Error = ex.Message;
                ViewBag.Authors = _authorService.GetAllAuthors();
                return View(bookDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en Create: {Message}", ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        public IActionResult Edit(int id)
        {
            try
            {
                var book = _bookService.GetBookById(id);
                if (book == null)
                {
                    TempData["ErrorMessage"] = "Libro no encontrado.";
                    return RedirectToAction("Index");
                }

                var authors = _authorService.GetAllAuthors()
                  .Select(a => new SelectListItem
                  {
                      Value = a.Id.ToString(),
                      Text = a.FullName,
                      Selected = a.Id == book.AuthorId 
                  }).ToList();

                ViewBag.Authors = authors;

                return View(book);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error al obtener el libro para editar: {Message}", ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, BookDTO bookDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Authors = _authorService.GetAllAuthors();
                return View(bookDto);
            }

            try
            {
                _bookService.UpdateBook(id, bookDto);
                TempData["SuccessMessage"] = "Libro actualizado correctamente.";
                return RedirectToAction("Index");
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Error al actualizar el libro: {Message}", ex.Message);
                ViewBag.Authors = _authorService.GetAllAuthors();
                TempData["ErrorMessage"] = ex.Message;
                return View(bookDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en Edit: {Message}", ex.Message);
                TempData["ErrorMessage"] = "Ocurrió un error inesperado.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var book = _bookService.GetBookById(id);
                if (book == null) return NotFound();

                ViewBag.AuthorName = _authorService.GetAuthorById(book.AuthorId)?.FullName;
                return View(book);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error en Delete GET: {Message}", ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _bookService.DeleteBook(id);
                return RedirectToAction("Index");
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Error en DeleteConfirmed: {Message}", ex.Message);
                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en DeleteConfirmed: {Message}", ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
