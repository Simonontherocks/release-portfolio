using CineVault.DataAccessLayer.Repositories;
using CineVault.ModelLayer.ModelMovie;

namespace CineVault.BusinessLogic.Service
{
    public class ActorService
    {
        // ToDo = nog af te werken
        private IActorRepository _actorRepository;

        public ActorService(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository;
        }

        public List<Actor> GetAll()
        {
            return _actorRepository.GetAll().OrderBy(a => a.Name).ToList();
        }

        public Actor GetById(int id)
        {
            return _actorRepository.GetById(id);
        }

        public void Insert(Actor actor)
        {
            // add businessLogic

            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            // ToDo => nog aan te vullen.

            _actorRepository.Insert(actor);
        }

        public void Delete(Actor actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            _actorRepository.Delete(actor);
        }

        public void SaveChanges()
        {            
            _actorRepository.SaveChanges();
        }

    }

}
