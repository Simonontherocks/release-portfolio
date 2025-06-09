using CineVault.DataAccessLayer.Repositories;
using CineVault.ModelLayer.ModelMovie;

namespace CineVault.BusinessLogic.Service
{
    /// <summary>
    /// - Serviceklasse verantwoordelijk voor het beheren van Director-objecten,
    /// - inclusief CRUD-operaties via de repositorylaag.
    /// </summary>

    public class DirectorService
    {
        // ToDo = nog af te werken
        private IDirectorRepository _directorRepository; // Interface voor toegang tot regisseur-data.

        public DirectorService(IDirectorRepository directorRepository)
        {
            _directorRepository = directorRepository; // Injectie van de director repository.
        }

        public List<Director> GetAll()
        {
            return _directorRepository.GetAll(); // Haal alle regisseurs op uit de repository.
        }

        public void Insert(Director director)
        {
            if (director == null)
            {
                throw new ArgumentNullException(nameof(director)); // Controle op null input.
            }

            Director existingDirector = _directorRepository.GetById(director.Id); // Controle of regisseur al bestaat.
            if (existingDirector != null)
            {
                return; // Reeds bestaand, niets invoegen.
            }

            _directorRepository.Insert(director); // Voeg regisseur toe via repository.
        }

        public void Delete(Director director)
        {
            _directorRepository.GetById(director.Id); // Controle of regisseur bestaat via ID.

            if (director == null)
            {
                throw new ArgumentNullException("Director does not exist in database and therefore cannot be deleted."); // Indien null, gooi fout.
            }

            _directorRepository.Delete(director); // Verwijder regisseur via repository.
        }

        public void SaveChanges()
        {
            _directorRepository.SaveChanges(); // Sla wijzigingen op in de databank.
        }

        public Director GetById(int id)
        {
            if (id.Equals(0))
            {
                throw new ArgumentException("ongeldige id"); // Controle op ongeldige ID.
            }

            Director requestedDirector = _directorRepository.GetById(id); // hier wordt de director opgehaald via de repository uit de database.

            if (requestedDirector == null)
            {
                throw new ArgumentNullException(nameof(requestedDirector)); // Controle of regisseur niet null is.
            }

            return requestedDirector; // Retourneer gevonden regisseur.
        }
    }
}
