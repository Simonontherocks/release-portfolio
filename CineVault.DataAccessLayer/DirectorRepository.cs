using CineVault.DataAccessLayer.Repositories;
using CineVault.ModelLayer.ModelMovie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineVault.DataAccessLayer
{
    public class DirectorRepository : IDirectorRepository
    {
        #region Field

        private readonly AppDBContext _context;

        #endregion

        #region Constructor

        public DirectorRepository(AppDBContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods               

        public List<Director> GetAll()
        {
            return _context.Directors.ToList();
        }

        public Director GetById(int id)
        {
           return _context.Directors.FirstOrDefault(d => d.Id == id);
        }

        public void Insert(Director director)
        {
            _context.Directors.Add(director);
        }        

        public void Update(Director director)
        {
            if (director == null)
            {
                throw new ArgumentNullException(nameof(director));
            }
            _context.Directors.Update(director);
        }

        public void Delete(Director director)
        {
            if(director == null)
            {
                throw new ArgumentNullException(nameof(director));
            }

            _context.Directors.Remove(director);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        #endregion

    }

}
