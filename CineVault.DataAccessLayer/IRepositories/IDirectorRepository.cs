using CineVault.ModelLayer.ModelMovie;

namespace CineVault.DataAccessLayer.Repositories
{
    public interface IDirectorRepository
    {
        List<Director> GetAll();
        Director GetById(int id);
        void Insert(Director director);
        void Update(Director director);
        void Delete(Director director);
        void SaveChanges();
    }
}
