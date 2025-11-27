using LibrarySofttek.DTOs;
using LibrarySofttek.Exceptions;
using LibrarySofttek.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySofttek.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorService _authorService;
        private readonly ILogger<AuthorsController> _logger;
        public AuthorsController(IAuthorService authorService, ILogger<AuthorsController> logger)
        {
            _authorService = authorService;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var authors = _authorService.GetAllAuthors();
            return View(authors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AuthorDTO authorDto)
        {
            if (!ModelState.IsValid)
            {
                return View(authorDto);
            }

            _authorService.AddAuthor(authorDto);
            return RedirectToAction("Index", "Books");
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var author = _authorService.GetAuthorById(id);
                if (author == null)
                {
                    TempData["ErrorMessage"] = "Autor no encontrado.";
                    return RedirectToAction("Index");
                }

                return View(author);
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error al obtener el autor para editar: {Message}", ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, AuthorDTO authorDto)
        {
            if (!ModelState.IsValid)
            {
                return View(authorDto);
            }

            try
            {
                _authorService.UpdateAuthor(id, authorDto);
                TempData["SuccessMessage"] = "Autor actualizado correctamente.";
                return RedirectToAction("Index");
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Error de negocio en Edit: {Message}", ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                return View(authorDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en Edit: {Message}", ex.Message);
                TempData["ErrorMessage"] = "Ocurrió un error inesperado.";
                return View(authorDto);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var author = _authorService.GetAuthorById(id);
                if (author == null)
                {
                    TempData["ErrorMessage"] = "Autor no encontrado.";
                    return RedirectToAction("Index");
                }

                return View(author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el autor para eliminar: {Message}", ex.Message);
                TempData["ErrorMessage"] = "Ocurrió un error inesperado.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _authorService.DeleteAuthor(id);
                TempData["SuccessMessage"] = "Autor eliminado correctamente.";
                return RedirectToAction("Index");
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Error de negocio en Delete: {Message}", ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en Delete: {Message}", ex.Message);
                TempData["ErrorMessage"] = "Ocurrió un error inesperado.";
                return RedirectToAction("Index");
            }
        }
    }
}