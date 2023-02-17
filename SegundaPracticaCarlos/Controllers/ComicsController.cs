using Microsoft.AspNetCore.Mvc;
using SegundaPracticaCarlos.Models;
using SegundaPracticaCarlos.Repositories;

namespace SegundaPracticaCarlos.Controllers
{
    public class ComicsController : Controller
    {
        private IRepositoryComics repo;

        public ComicsController(IRepositoryComics repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Comic> comics = this.repo.GetComics();
            return View(comics);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Comic comic)
        {
            this.repo.InsertComic(comic.Nombre, comic.Imagen, comic.Descripcion);
            return RedirectToAction("Index");
        }
    }
}
