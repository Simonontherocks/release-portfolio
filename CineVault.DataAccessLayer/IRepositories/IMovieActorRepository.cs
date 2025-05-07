using CineVault.ModelLayer.ModelMovie;

namespace CineVault.DataAccessLayer.Repositories
{
    public interface IMovieActorRepository
    {
        List<MovieActor> GetAll();
        MovieActor GetById(int id);
        void Insert(MovieActor movieActor);
        void Update(MovieActor movieActor);
        void Delete(MovieActor movieActor);
    }
}
