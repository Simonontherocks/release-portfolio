using CineVault.ModelLayer.ModelMovie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
