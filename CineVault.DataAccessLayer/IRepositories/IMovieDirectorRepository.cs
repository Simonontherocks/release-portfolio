using CineVault.ModelLayer.ModelMovie;

namespace CineVault.DataAccessLayer
{
    public interface IMovieDirectorRepository
    {
        List<MovieDirector> GetAll();
        MovieDirector GetById(int id);
        void Insert(MovieDirector movieDirector);
        void Update(MovieDirector movieDirector);
        void Delete(MovieDirector movieDirector);
    }
}
