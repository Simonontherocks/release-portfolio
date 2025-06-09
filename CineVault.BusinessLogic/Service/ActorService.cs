using CineVault.DataAccessLayer.Repositories;
using CineVault.ModelLayer.ModelMovie;
using System.IO;

namespace CineVault.BusinessLogic.Service
{
    public class ActorService
    {
        /// <summary>
        /// - Serviceklasse verantwoordelijk voor het beheren van Actor-objecten,
        /// - inclusief ophalen, toevoegen, verwijderen en opslaan van wijzigingen via de repositorylaag.
        /// </summary>

        private IActorRepository _actorRepository; // Interface voor toegang tot actor-data.

        public ActorService(IActorRepository actorRepository)
        {
            _actorRepository = actorRepository; // Injectie van de actor repository.
        }

        public List<Actor> GetAll()
        {
            return _actorRepository.GetAll().OrderBy(a => a.Name).ToList(); // Alle acteurs ophalen en sorteren op naam.
        }

        public Actor GetById(int id)
        {
            return _actorRepository.GetById(id); // Een specifieke actor ophalen op basis van ID.
        }

        public void Insert(Actor actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor)); // Controle op null input.
            }

            Actor existingActor = _actorRepository.GetById(actor.Id); // Controleer of actor al bestaat.
            if (existingActor != null)
            {                
                return; // Actor bestaat al, dus sla het invoegen over.
            }

            _actorRepository.Insert(actor); // Voeg actor toe via repository.
        }

        public void Delete(Actor actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor)); // Controle op null actor.
            }

            _actorRepository.Delete(actor); // Verwijder actor via repository.
        }

        public void SaveChanges()
        {            
            _actorRepository.SaveChanges(); // Sla wijzigingen op in de databank.
        }

    }

}
