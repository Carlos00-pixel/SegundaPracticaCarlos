using SegundaPracticaCarlos.Models;

namespace SegundaPracticaCarlos.Repositories
{
    public interface IRepositoryComics
    {
        List<Comic> GetComics();
        void InsertComic(string nombre, string imagen, string descripcion);
    }
}
