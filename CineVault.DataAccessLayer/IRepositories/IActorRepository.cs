using CineVault.ModelLayer.ModelMovie;

namespace CineVault.DataAccessLayer.Repositories
{
    public interface IActorRepository
    {
        List<Actor> GetAll();
        Actor GetById(int id);
        void Insert(Actor actor);
        void Update(Actor actor);
        void Delete(Actor actor);
        void SaveChanges();
    }
}
