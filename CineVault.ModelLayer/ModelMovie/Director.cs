using CineVault.ModelLayer.ModelAbstractClass;

namespace CineVault.ModelLayer.ModelMovie
{
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
