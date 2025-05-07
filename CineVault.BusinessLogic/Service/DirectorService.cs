using CineVault.DataAccessLayer.Repositories;
using CineVault.ModelLayer.ModelMovie;

namespace CineVault.BusinessLogic.Service
{
    public class DirectorService
    {
        // ToDo = nog af te werken
        private IDirectorRepository _directorRepository;

        public DirectorService(IDirectorRepository directorRepository)
        {
            _directorRepository = directorRepository;
        }

        public List<Director> GetAll()
        {
            return _directorRepository.GetAll();
        }

        public void Insert(Director director)
        {
            _directorRepository.GetById(director.Id); // via de repository wordt nagegaan of de director al niet reeds bestaat.

            // Indien de director een null-waarde heeft, dan wordt er een uitzondering getoond.
            //if (director == null)
            //{
            //    throw new ArgumentNullException(nameof(director));
            //}
            //if (director != null) // Indien de director wel al bestaat, dan dient er niets te gebeuren.
            //{
            //    //Do nothing
            //}
            //else
            //{
            //    _directorRepository.Insert(director);
            //}
            _directorRepository.Insert(director);
        
        }

        public void Delete(Director director)
        {
            _directorRepository.GetById(director.Id); // via de repository wordt nagegaan of de director al niet reeds bestaat.

            // Indien de director een null-waarde heeft, dan wordt er een uitzondering getoond.
            if (director == null)
            {
                throw new ArgumentNullException("Director does not exist in database and therefore cannot be deleted.");
            }

            _directorRepository.Delete(director);
        }

        public void SaveChanges()
        {
            _directorRepository.SaveChanges();
        }

        public Director GetById(int id)
        {
            // Eerste controle is om te kijken of de Id een geldige waarde heeft.
            // Indien de waarde 0 is, dan wordt er een uitzondering getoont.
            if (id.Equals(0))
            {
                throw new ArgumentException("ongeldige id");
            }

            Director requestedDirector = _directorRepository.GetById(id); // hier wordt de director opgehaald via de repository uit de database.

            // Hier gebeurd de controle op de opgevraagde director, om na te gaan of die niet null is.
            // Indien deze null is, dan wordt er een uitzondering getoond.
            if (requestedDirector == null)
            {
                throw new ArgumentNullException(nameof(requestedDirector));
            }

            // De opgevraagde director wordt teruggegeven.
            return requestedDirector;
        }
    }
}
