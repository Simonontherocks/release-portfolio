using CineVault.ModelLayer.ModelMovie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
