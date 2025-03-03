using CineVault.ModelLayer.ModelMovie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
