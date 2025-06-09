using CineVault.ModelLayer.ModelAbstractClass;

namespace CineVault.ModelLayer.ModelMovie
{
    /// <summary> 
    /// - `Actor` **erft van `Person`**, zodat het alle basis eigenschappen zoals `Id` en `Name` bevat.  
    /// - Een acteur moet geïnstantieerd kunnen worden.  
    /// - NavigationProperty is toegevoegd naar "MovieActor", zodat acteurs gekoppeld kunnen worden aan films.  
    /// - **Hiermee kunnen acteurs worden opgeslagen en opgehaald uit de database**.
    /// </summary>
    public class Actor : Person
    {
        #region Properties

        //  Hier wordt gebruik gemaakt van de Id- en Name properties van de abstracte klasse "Person".

        #endregion

        #region navigationProperty

        public ICollection<MovieActor> MovieActors { get; set; } // Deze property dient om te navigeren naar de klasse Movie. Dit zal dus fungeren als tussenproperty.

        #endregion

    }

}
