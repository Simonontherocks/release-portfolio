using CineVault.ModelLayer.ModelAbstractClass;

namespace CineVault.ModelLayer.ModelMovie
{
    /// <summary> 
    /// - `Director` **erft van `Person`**, zodat het alle basis eigenschappen zoals `Id` en `Name` bevat.  
    /// - Een regisseur moet geïnstantieerd kunnen worden.  
    /// - NavigationProperty is toegevoegd naar "MovieDirector", zodat regisseurs gekoppeld kunnen worden aan films.  
    /// - **Hiermee kunnen regisseurs worden opgeslagen en opgehaald uit de database**.
    /// </summary>
    public class Director : Person
    {
        #region Properties

        //  Hier wordt gebruik gemaakt van de Id- en Name properties van de abstracte klasse "Person".

        #endregion

        #region navigationProperty

        public ICollection<MovieDirector> MovieDirectors { get; set; } // Deze property dient om te navigeren naar de klasse Movie. Dit zal dus fungeren als tussenproperty.

        #endregion

    }

}
