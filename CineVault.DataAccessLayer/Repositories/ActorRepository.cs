using CineVault.DataAccessLayer.Repositories;
using CineVault.ModelLayer.ModelMovie;
using CineVault.DataAccessLayer.Context;

namespace CineVault.DataAccessLayer
{
    // ToDo => nog te controleren op NULL in alle repo's
    public class ActorRepository : IActorRepository
    {
        #region Field

        private readonly AppDBContext _dbContext;

        #endregion

        #region Constructor

        public ActorRepository(AppDBContext context)
        {
            _dbContext = context;
        }

        #endregion

        #region Methods        

        public List<Actor> GetAll()
        {
            return _dbContext.Actors.ToList();
        }

        public Actor GetById(int id)
        {
            return _dbContext.Actors.FirstOrDefault(a => a.Id == id);
        }

        public void Insert(Actor actor)
        {
            _dbContext.Actors.Add(actor);
        }

        public void Update(Actor actor)
        {
            _dbContext.Actors.Update(actor);
        }

        public void Delete(Actor actor)
        {
            _dbContext.Actors.Remove(actor);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }        

        #endregion
    }

}
