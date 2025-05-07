using CineVault.ModelLayer.ModelAbstractClass;


namespace CineVault.ModelLayer.ModelUser
{
    public class User : Person
    {
        #region Properties

        // Using the Id- and Name properties from the abstract class "Person"

        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DateOfCreation { get; set; }

        #endregion

        #region constructor

        #endregion

    }

}
