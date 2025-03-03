using CineVault.ModelLayer.ModelMovie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
